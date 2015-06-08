using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using JSON;
using Logging;

namespace TCPConnectors
{

    public class TCPServerThread : LoggingClass
    {

        public class StateSocket
        {

            public Socket workSocket;

            public const int BufferSize = 1024;

            public byte[] buffer = new byte[BufferSize];

            public StringBuilder sb = new StringBuilder();

            public JSONObject response;

        }

        public delegate void RequestHandler(JSONObject request, JSONObject response);

        private int fieldPort;

        private ManualResetEvent allDone;

        private Socket listener;

        private Thread fieldServer;

        private bool listening;

        public RequestHandler commandHandler;

        public RequestHandler messageHandler;

        public RequestHandler requestHandler;

        public TCPServerThread(int port)
        {
            fieldPort = port;
            allDone = new ManualResetEvent(false);
            listening = false;
        }

        public void Serve()
        {

            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            Log("Server entry retrieved: " + ipHostInfo + "\n");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            Log("Server address retrieved: " + ipAddress + "\n");
            IPEndPoint localEP = new IPEndPoint(ipAddress, fieldPort);
            Log("Server Endpoint created: " + localEP + "\n");

            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Log("listener created: " + listener + "\n");

            try
            {
                listener.Bind(localEP);
                Log("listener bound" + "\n");
                listener.Listen(10);

                listening = true;
                while (listening)
                {
                    Log("listening" + "\n");
                    allDone.Reset();
                    Log("listenet.Connected = " + listener.Connected + "\n");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Log("In Server.Serve\n");
                Log(e.ToString() + "\n");
            }

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Log("accepted" + "\n");
                allDone.Set();
                Socket handler = listener.EndAccept(ar);

                StateSocket state = new StateSocket();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0,
                    StateSocket.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                Log("In Server.AcceptCallback\n");
                Log(e.ToString() + "\n");
            }
        }

        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                StateSocket state = (StateSocket)ar.AsyncState;
                Socket handler = state.workSocket;

                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    content = state.sb.ToString();
                    Log("Read " + content.Length + " bytes from socket.  Data : " + content + "\n");
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        JSONObject response = new JSONObject();
                        response.AddString("responseType", "multi");
                        response.AddEmptyArray("responses");
                        ArrayValue responses = response.GetArray("responses");
                        responses.AddJSON(HandleMessage(JSONParser.ParseString(content.Substring(0, content.IndexOf("<EOF>")))));
                        responses.AddJSON(new JSONObject(new string[] { "responseType", "message" }, new object[] { "message", "Diconnecting" }));
                        state.response = response;
                        Send(state);

                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                    else if (content.IndexOf("<EOM>") > -1)
                    {
                        state.response = HandleMessage(JSONParser.ParseString(content.Substring(0, content.IndexOf("<EOM>"))));
                        Send(state);
                        state.sb = new StringBuilder();

                        handler.BeginReceive(state.buffer, 0, StateSocket.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }
                    else
                    {
                        JSONObject response = new JSONObject();
                        response.AddString("responseType", "message");
                        response.AddString("message", "Line Received");
                        state.response = response;
                        Send(state);
                        handler.BeginReceive(state.buffer, 0, StateSocket.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }
                }
                Log("ReadCallback End\n");
            }
            catch (Exception e)
            {
                Log("In Server.ReadCallback\n");
                Log(e.ToString() + "\n");
            }
        }

        public void Send(StateSocket state)
        {
            try
            {
                Log("sending response: " + state.response.ToString() + "\n");
                Socket handler = state.workSocket;
                byte[] byteData = Encoding.ASCII.GetBytes(state.response.ToString());
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), state);
            }
            catch (Exception e)
            {
                Log("In Server.Send\n");
                Log(e.ToString() + "\n");
            }
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                StateSocket state = (StateSocket)ar.AsyncState;
                Socket handler = state.workSocket;

                int bytesSent = handler.EndSend(ar);
                Log("Sent " + bytesSent + " bytes to client.\n\t" + state.response.ToString() + "\n");
            }
            catch (Exception e)
            {
                Log("In Server.SendCallback\n");
                Log(e.ToString() + "\n");
            }
        }

        public JSONObject HandleMessage(JSONObject msg)
        {
            try
            {
                JSONObject response = new JSONObject();
                if (msg.GetString("requestType").Equals("command"))
                    commandHandler(msg.GetJSON("command"), response);
                else if (msg.GetString("requestType").Equals("message"))
                    messageHandler(msg.GetJSON("message"), response);
                else if (msg.GetString("requestType").Equals("request"))
                    requestHandler(msg.GetJSON("request"), response);
                return response;
            }
            catch (Exception e)
            {
                Log("In Server.HandleMessage\n");
                Log("msg: " + msg.ToString() + "\n");
                Log("msg.requestType: " + msg.GetString("requestType") + "\n");
                Log(e.ToString() + "\n");
                return null;
            }
        }

        public void Start()
        {
            if (server == null)
            {
                fieldServer = new Thread(Serve);
                fieldServer.Start();
            }
        }

        public bool Shutdown()
        {

            listening = false;
            return Join();

        }

        public void Stop()
        {

            listening = false;

        }

        public bool Join()
        {
            if (Thread.CurrentThread.Equals(server))
                return true;
            try
            {

                server.Join();
                return true;

            }
            catch (Exception e)
            {

                Log(e.ToString());
                return false;

            }
        }

        public Thread server
        {
            get
            {
                return fieldServer;
            }
        }

        public int port
        {
            get
            {
                return fieldPort;
            }
        }

    }

}