using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON;
using Logging;

namespace TCPConnectors
{

    public class TCPConnector : LoggingClass
    {

        private TCPServerThread server;

        private TCPClient client;

        public TCPServerThread.MessageHandler commandHandler;

        public TCPServerThread.MessageHandler messageHandler;

        public TCPServerThread.RequestHandler requestHandler;

        public TCPConnector(int inPort, int outPort)
        {
            server = new TCPServerThread(inPort);
            client = new TCPClient(outPort);
            commandHandler += StopListening;
            server.commandHandler = DelegateCommand;
            server.messageHandler = DelegateMessage;
            server.requestHandler = DelegateRequest;
            server.logger = DelegateLog;
            client.logger = DelegateLog;
        }

        void DelegateCommand(string command, JSONObject response)
        {
            commandHandler(command, response);
        }

        void DelegateMessage(string message, JSONObject response)
        {
            messageHandler(message, response);
        }

        void DelegateRequest(JSONObject request, JSONObject response)
        {
            requestHandler(request, response);
        }

        void DelegateLog(string message)
        {
            logger(message);
        }

        public JSONObject Message(JSONObject msg, bool endConnection)
        {
            return client.Message(msg, endConnection);
        }

        public void Start()
        {

            Log("connecting\n");
            server.Start();
            client.Connect();
            Log("connected\n");

        }

        public void Shutdown()
        {
            JSONObject msg = new JSONObject();
            msg.AddString("requestType", "command");
            msg.AddString("command", "Disconnect");
            client.Message(msg, true);
            client.Shutdown();
            server.Join();
        }

        void StopListening(string command, JSONObject response)
        {
            if(command.Equals("Disconnect"))
            {
                server.Stop();
                if (response.hasKey("responseType"))
                {
                    if (response.GetString("responseType").Equals("multi"))
                    {
                        JSONObject msg = new JSONObject();
                        msg.AddString("responseType", "message");
                        msg.AddString("message", "Disconnecting");
                        response.GetArray("messages").AddJSON(msg);
                    }
                    else
                    {
                        JSONObject old = response.Clone();
                        response.Clear();
                        response.AddString("responseType", "multi");
                        response.AddEmptyArray("responses");
                        ArrayValue responses = response.GetArray("responses");
                        JSONObject msg = new JSONObject();
                        msg.AddString("responseType", "message");
                        msg.AddString("message", "Disconnecting");
                        responses.AddJSON(old);
                        responses.AddJSON(msg);
                    }
                }
                else
                {
                    response.AddString("responseType", "message");
                    response.AddString("message", "Disconnecting");
                }
            }
        }

    }

}