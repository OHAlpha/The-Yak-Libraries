using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using JSON;

namespace CSTcpClient
{
    class Program
    {

        class StateSocket
        {

            public Socket workSocket;

            public const int BufferSize = 1024;

            public byte[] buffer = new byte[BufferSize];

            public StringBuilder sb = new StringBuilder();

        }

        private static int inPort;

        private static int outPort;

        private static ManualResetEvent allDone = new ManualResetEvent(false);

        private static Socket listener;

        private static Thread server;

        private static bool listening = false;

        public static void Serve()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            Console.Write("Server entry retrieved" + "\n");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            Console.Write("Server address retrieved" + "\n");
            IPEndPoint localEP = new IPEndPoint(ipAddress, inPort);
            Console.Write("Server Endpoint created" + "\n");


            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.Write("listener created" + "\n");

            try
            {
                listener.Bind(localEP);
                Console.Write("listener bound" + "\n");
                listener.Listen(10);

                listening = true;
                while (listening)
                {
                    Console.Write("listening" + "\n");
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            Console.Write("Press ENTER to continue...");
            Console.Read();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            Console.Write("accepted" + "\n");
            allDone.Set();
            Socket handler = listener.EndAccept(ar);

            StateSocket state = new StateSocket();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateSocket.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            StateSocket state = (StateSocket)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    Console.Write("Read " + content.Length + " bytes from socket.  Data : " + content + "\n");
                    Send(handler, "Diconnecting");

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                else if (content.IndexOf("<EOM>") > -1)
                {
                    Console.Write("Read " + content.Length + " bytes from socket.  Data : " + content + "\n");
                    HandleMessage(content);
                    Send(handler, "Message Received");
                    state.sb = new StringBuilder();

                    handler.BeginReceive(state.buffer, 0, StateSocket.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
                else
                {
                    Send(handler, "Line Received");
                    handler.BeginReceive(state.buffer, 0, StateSocket.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        public static void Send(Socket handler, string msg)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(msg);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        public static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;

                int bytesSent = handler.EndSend(ar);
                Console.Write("Sent " + bytesSent + " bytes to client." + "\n");
            }
            catch (Exception e)
            {
                Console.Write(e.ToString() + "\n");
            }
        }

        public static void HandleMessage(string msg)
        {
        }

        private static Socket sender;

        private static SpeechRecognitionEngine sre;

        private static Semaphore loadedSemaphore = new Semaphore(0, 1);

        private static Semaphore exitSemaphore = new Semaphore(0, 1);

        public static bool Connect()
        {

            bool connected = false;

            while (!connected)
                try
                {

                    IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, outPort);

                    sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    sender.Connect(remoteEP);
                    connected = true;

                }
                catch (Exception e)
                {

                    Console.Write(e.ToString());
                    return false;

                }
            return connected;

        }

        static void Disconnect()
        {
            JSONObject msg = new JSONObject();
            msg.AddString("type", "Command");
            msg.AddString("command", "Disconnect");
            Message(msg, true);
        }

        public static bool Shutdown()
        {

            listening = false;
            try
            {

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                server.Join();
                return true;

            }
            catch (Exception e)
            {

                Console.Write(e.ToString());
                return false;

            }

        }

        static string Message(JSONObject msg, bool endConnection)
        {

            string text = msg.ToString() + (endConnection ? "<EOF>" : "<EOM>");
            Console.Write(text + "\n");
            byte[] outBytes, inBytes = new byte[128];

            outBytes = Encoding.ASCII.GetBytes(text);
            sender.Send(outBytes);

            int inCount = sender.Receive(inBytes);
            string inString = Encoding.ASCII.GetString(inBytes, 0, inCount);

            return inString;

        }

        public static void Start()
        {

            Console.Write("connecting\n");
            server = new Thread(Serve);
            server.Start();
            Connect();
            Console.Write("connected\n");

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
            Disconnect();

        }

        // Create a simple handler for the LoadGrammarCompleted event.
        static void sre_LoadGrammarCompleted(object sender, LoadGrammarCompletedEventArgs e)
        {
            JSONObject msg = new JSONObject();
            msg.AddString("type", "SpeechEvent");
            msg.AddString("event", "LoadGrammarCompleted");
            Message(msg, false);
            loadedSemaphore.Release();
        }

        // Create a simple handler for the SpeechDetected event.
        static void sre_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            JSONObject msg = new JSONObject();
            msg.AddString("type", "SpeechEvent");
            msg.AddString("event", "SpeechDetected");
            Message(msg, false);
        }

        // Create a simple handler for the SpeechHypothesized event.
        static void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            JSONObject msg = new JSONObject();
            msg.AddString("type", "SpeechEvent");
            msg.AddString("event", "SpeechHypothesized");
            msg.AddString("text", e.Result.Text);
            Message(msg, false);
        }

        // Create a simple handler for the SpeechRecognized event.
        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
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
            inPort = 201;
            outPort = 200;
            Console.Write("starting\n");
            Start();
            Console.Write("finished\n");
            Console.Write("shutting down\n");
            Shutdown();
            Console.Write("shut down\n");
            Console.Write("\nPress ENTER to continue...");
            Console.Read();
        }
    }
}
