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

        public TCPServerThread.RequestHandler commandHandler;

        public TCPServerThread.RequestHandler messageHandler;

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

        void DelegateCommand(JSONObject command, JSONObject response)
        {
            commandHandler(command, response);
        }

        void DelegateMessage(JSONObject message, JSONObject response)
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

        public void Start(int retries)
        {

            Log("connecting\n");
            server.Start();
            client.Connect(retries);
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

        void StopListening(JSONObject command, JSONObject response)
        {
            if(command.GetString("command").Equals("Disconnect"))
            {
                server.Shutdown();
                if (response.HasKey("responseType"))
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