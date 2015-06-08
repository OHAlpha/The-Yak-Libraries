using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using JSON;
using OAlphaCollections;
using TCPConnectors;

namespace RecognitionServer
{

    class Program : TCPConnector
    {

        private SpeechRecognitionEngine sre;

        private Semaphore operationalSemaphore = new Semaphore(0, 1);

        private Semaphore grammarSemaphore = new Semaphore(1, 1);

        private Semaphore loadedSemaphore = new Semaphore(0, 1);

        private Semaphore exitSemaphore = new Semaphore(0, 1);

        private Map<string, Grammar> grammars = new SinglyLinkedMap<string, Grammar>();

        public Program()
            : base(201, 200)
        {
            logger += Console.Write;
            commandHandler += HandleCommand;
            messageHandler += MessageReceived;
            requestHandler += HandleRequest;
        }

        public void StartRecognition()
        {

            Start(-1);

            // Create a new SpeechRecognitionEngine instance.
            sre = new SpeechRecognitionEngine();

            // Set timeout
            sre.InitialSilenceTimeout = new TimeSpan(0, 0, 0, 2, 0);

            // Configure the input to the recognizer.
            sre.SetInputToDefaultAudioDevice();

            // Register a handler for the LoadGrammarCompleted event.
            sre.LoadGrammarCompleted +=
              new EventHandler<LoadGrammarCompletedEventArgs>(sre_LoadGrammarCompleted);

            sre.SpeechDetected +=
                new EventHandler<SpeechDetectedEventArgs>(sre_SpeechDetected);

            sre.SpeechHypothesized +=
                new EventHandler<SpeechHypothesizedEventArgs>(sre_SpeechHypothesized);

            // Register a handler for the SpeechRecognized event.
            sre.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            //Load grammar from file
            Console.Write("loading grammar\n");
            Console.Write("AddGrammar(\"base-commands\",\"base_commands\"): " + AddGrammar("base-commands", "base_commands"));
            Console.Write("LoadGrammar(\"base-commands\"): " + LoadGrammar("base-commands"));
            Console.Write("waiting for load\n");
            loadedSemaphore.WaitOne();
            Console.Write("returned from load\n");

            // Start recognition
            sre.RecognizeAsync(RecognizeMode.Multiple);
            Console.Write("waiting on exit\n");
            exitSemaphore.WaitOne();
            Console.Write("returning from exit\n");

            sre.RecognizeAsyncCancel();
            Console.Write("shutting down\n");
            Shutdown();
            Console.Write("shut down\n");

        }

        // Create a simple handler for the LoadGrammarCompleted event.
        void sre_LoadGrammarCompleted(object sender, LoadGrammarCompletedEventArgs e)
        {
            JSONObject msg = new JSONObject();
            msg.AddString("message", "SpeechEvent");
            msg.AddString("event", "LoadGrammarCompleted");
            msg.AddString("grammar", e.Grammar.Name);
            Message(msg, false);
            if (e.Grammar.Name.Equals("base-commands"))
            {
                loadedSemaphore.Release();
                operationalSemaphore.Release();
            }
        }

        // Create a simple handler for the SpeechDetected event.
        void sre_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            JSONObject msg = new JSONObject();
            msg.AddString("message", "SpeechEvent");
            msg.AddString("event", "SpeechDetected");
            Message(msg, false);
        }

        // Create a simple handler for the SpeechHypothesized event.
        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            JSONObject msg = new JSONObject();
            msg.AddString("message", "SpeechEvent");
            msg.AddString("event", "SpeechHypothesized");
            msg.AddString("text", e.Result.Text);
            Message(msg, false);
        }

        // Create a simple handler for the SpeechRecognized event.
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int type = -1;
            int subtype = -1;
            JSONObject msg = new JSONObject();
            msg.AddString("message", "SpeechEvent");
            msg.AddString("event", "SpeechRecognized");
            msg.AddString("text", e.Result.Text);
            msg.AddFloat("confidence", e.Result.Confidence);
            JSONObject semantics = new JSONObject();
            foreach (KeyValuePair<String, SemanticValue> child in e.Result.Semantics)
            {
                string key = child.Key;
                SemanticValue semValue = child.Value;
                object value = semValue.Value;
                if (value == null)
                    semantics.AddNull(key);
                else
                    semantics.AddString(key, value.ToString());
                if (child.Key.Equals("type"))
                {
                    if (child.Value.Value.ToString().Equals("command"))
                        type = 0;
                    else if (child.Value.Value.ToString().Equals("response"))
                        type = 1;
                }
                else if (child.Key.Equals("command"))
                    if (child.Value.Value.ToString().Equals("exit"))
                        subtype = 0;
            }
            msg.AddJSON("semantics", semantics);
            Message(msg, false);
            if (type == 0 && subtype == 0)
            {
                exitSemaphore.Release();
            }
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            Console.Write("starting\n");
            program.StartRecognition();
            Console.Write("finished\n");
            Console.Write("\nPress ENTER to continue...");
            Console.Read();
        }

        Exception AddGrammar(string name, string filename)
        {
            grammarSemaphore.WaitOne();
            try
            {
                Grammar g;
                try
                {
                    g = new Grammar(Environment.CurrentDirectory + "\\grammars\\" + filename + ".xml");
                }
                catch (Exception e)
                {
                    grammarSemaphore.Release();
                    return e;
                }
                g.Name = name;
                grammars.Put(name, g);
            }
            finally
            {
                grammarSemaphore.Release();
            }
            return null;
        }

        bool LoadGrammar(string name)
        {
            grammarSemaphore.WaitOne();
            bool s;
            try
            {
                Grammar g = grammars.Get(name);
                if (g != null)
                {
                    sre.LoadGrammarAsync(g);
                    s = true;
                }
                else
                    s = false;
            }
            finally
            {
                grammarSemaphore.Release();
            }
            return s;
        }

        bool UnloadGrammar(string name)
        {
            grammarSemaphore.WaitOne();
            bool s;
            try
            {
                Grammar g = grammars.Get(name);
                if (g != null)
                {
                    sre.UnloadGrammar(g);
                    s = true;
                }
                else
                    s = false;
            }
            finally
            {
                grammarSemaphore.Release();
            }
            return s;
        }

        void MessageReceived(JSONObject message, JSONObject response)
        {
            operationalSemaphore.WaitOne();
            try
            {
                response.AddString("responseType", "message");
                response.AddString("message", "Message Recieved");
            }
            finally
            {
                operationalSemaphore.Release();
            }
        }

        void HandleCommand(JSONObject command, JSONObject response)
        {
            operationalSemaphore.WaitOne();
            try
            {
                string name, filename;
                Exception ex;
                bool success;
                switch (command.GetString("command"))
                {
                    case "AddGrammar":
                        name = command.GetString("name");
                        filename = command.GetString("filename");
                        ex = AddGrammar(name, filename);
                        if (ex == null)
                        {
                            response.AddString("responseType", "message");
                            response.AddString("message", "Grammar Added");
                            response.AddString("name", name);
                            response.AddString("filename", filename);
                        }
                        else
                        {
                            response.AddString("responseType", "message");
                            response.AddString("message", "Exception Creating Grammar");
                            response.AddString("name", name);
                            response.AddString("filename", filename);
                            response.AddJSON("exception", ExceptionJSON(ex));
                        }
                        break;
                    case "LoadGrammar":
                        name = command.GetString("name");
                        success = LoadGrammar(name);
                        if (success)
                        {
                            response.AddString("responseType", "message");
                            response.AddString("message", "Grammar Load Started");
                            response.AddString("name", name);
                        }
                        else
                        {
                            response.AddString("responseType", "message");
                            response.AddString("message", "Error Loading Grammar");
                            response.AddString("name", name);
                            response.AddString("reason", "No Such Grammar");
                        }
                        break;
                    case "UnloadGrammar":
                        name = command.GetString("name");
                        success = UnloadGrammar(name);
                        if (success)
                        {
                            response.AddString("responseType", "message");
                            response.AddString("message", "Grammar Unloaded");
                            response.AddString("name", name);
                        }
                        else
                        {
                            response.AddString("responseType", "message");
                            response.AddString("message", "Error Unloading Grammar");
                            response.AddString("name", name);
                            response.AddString("reason", "No Such Grammar");
                        }
                        break;
                    default:
                        response.AddString("responseType", "message");
                        response.AddString("message", "Invalid Command");
                        break;
                }
            }
            finally
            {
                operationalSemaphore.Release();
            }
        }

        void HandleRequest(JSONObject request, JSONObject response)
        {
            operationalSemaphore.WaitOne();
            try
            {
                switch (request.GetString("request"))
                {
                    case "":
                        response.AddString("responseType", "message");
                        response.AddString("message", "Request Handled");
                        break;
                    default:
                        response.AddString("responseType", "message");
                        response.AddString("message", "Unknown Request");
                        break;
                }
            }
            finally
            {
                operationalSemaphore.Release();
            }
        }

        JSONObject ExceptionJSON(Exception ex)
        {
            JSONObject exception = new JSONObject();
            exception.AddString("message", ex.Message);
            exception.AddString("source", ex.Source);
            exception.AddString("helpLink", ex.HelpLink);
            exception.AddString("stackTrace", ex.StackTrace);
            if (ex.InnerException != null)
                exception.AddJSON("innerException", ExceptionJSON(ex.InnerException));
            return exception;
        }

    }

}