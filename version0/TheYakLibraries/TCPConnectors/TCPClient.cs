using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using JSON;
using Logging;

namespace TCPConnectors
{
    public class TCPClient : LoggingClass
    {

        private int fieldPort;

        private Socket sender;

        public TCPClient(int port)
        {
            fieldPort = port;
        }

        public bool Connect()
        {

            bool connected = false;

            while (!connected)
                try
                {

                    IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, fieldPort);

                    sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    sender.Connect(remoteEP);
                    connected = true;

                }
                catch (Exception e)
                {

                    Log(e.ToString());
                    return false;

                }
            return connected;

        }

        public void Disconnect()
        {
            JSONObject msg = new JSONObject();
            msg.AddString("type", "Command");
            msg.AddString("command", "Disconnect");
            Message(msg, true);
        }

        public bool Shutdown()
        {

            try
            {

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                return true;

            }
            catch (Exception e)
            {

                Log(e.ToString());
                return false;

            }

        }

        public JSONObject Message(JSONObject msg, bool endConnection)
        {

            JSONObject wrapper = new JSONObject();
            wrapper.AddString("requestType", "request");
            wrapper.AddJSON("request", msg);
            string text = wrapper.ToString() + (endConnection ? "<EOF>" : "<EOM>");
            Log(text + "\n");
            byte[] outBytes, inBytes = new byte[1024];

            outBytes = Encoding.ASCII.GetBytes(text);
            sender.Send(outBytes);

            int inCount = sender.Receive(inBytes);
            string inString = Encoding.ASCII.GetString(inBytes, 0, inCount);
            Log(inString + "\n");

            return JSONParser.ParseString(inString);

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