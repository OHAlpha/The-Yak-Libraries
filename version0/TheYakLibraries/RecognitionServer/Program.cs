using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using JSON;
using TCPConnectors;

namespace RecognitionServer
{

    class Program : TCPConnector
    {

        private SpeechRecognitionEngine sre;

        private Semaphore loadedSemaphore = new Semaphore(0, 1);

        private Semaphore exitSemaphore = new Semaphore(0, 1);

        public Program() : base(201, 200)
        {
            logger += Console.Write;
            commandHandler += MessageReceived;
            messageHandler += MessageReceived;
            requestHandler += MessageReceived;
        }

        public void StartRecognition()
        {

            Start();

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
            sre.LoadGrammarAsync(new Grammar(Environment.CurrentDirectory + "\\test.grxml"));
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
            msg.AddString("type", "SpeechEvent");
            msg.AddString("event", "LoadGrammarCompleted");
            Message(msg, false);
            loadedSemaphore.Release();
        }

        // Create a simple handler for the SpeechDetected event.
        void sre_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            JSONObject msg = new JSONObject();
            msg.AddString("type", "SpeechEvent");
            msg.AddString("event", "SpeechDetected");
            Message(msg, false);
        }

        // Create a simple handler for the SpeechHypothesized event.
        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            JSONObject msg = new JSONObject();
            msg.AddString("type", "SpeechEvent");
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
            msg.AddString("type", "SpeechEvent");
            msg.AddString("event", "SpeechRecognized");
            msg.AddString("text", e.Result.Text);
            msg.AddFloat("confidence", e.Result.Confidence);
            JSONObject semantics = new JSONObject();
            foreach (KeyValuePair<String, SemanticValue> child in e.Result.Semantics)
            {
                semantics.AddString(child.Key, child.Value.Value.ToString());
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

        void MessageReceived(string message, JSONObject response)
        {
            response.AddString("responseType", "message");
            response.AddString("message", "Message Recieved");
        }

        void MessageReceived(JSONObject request, JSONObject response)
        {
            response.AddString("responseType", "message");
            response.AddString("message", "Message Recieved");
        }

    }

}