using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MoonTaxi.Network
{
    class Client
    {
        private const int BUFFER_SIZE = 1024;

        public delegate void DataReceivedDelegate(Client sender, byte[] buffer, int count);
        public event DataReceivedDelegate DataReceived;

        private TcpClient client;
        private Stream stream;
        private byte[] buffer = new byte[BUFFER_SIZE];
        private delegate void HandleMessageDelegate(Message message);
        private Dictionary<byte, HandleMessageDelegate> delegates;
        public Client(string username)
        {
            Username = username;
            client = new TcpClient();
            InitDelegates();
        }
        public Client(TcpClient client)
        {
            this.client = client;
            InitDelegates();
            stream = client.GetStream();
            BeginRead();
        }
        public string Username { get;private set; }
        private void InitDelegates()
        {
            delegates = new Dictionary<byte, HandleMessageDelegate>();
            delegates.Add(1, HandleHandshakeRequest);
            delegates.Add(2, HandleHandshakeResponse);
        }
        public void Connect(IPAddress address, int port)
        {
            client.BeginConnect(address, port, EndConnect, null);
        }
        public void Send(byte[] buffer, int count)
        {
            stream.Write(buffer, 0, count);
        }
        public void Send(Message message)
        {
            if (message.PayLoad == null)
            {
                stream.WriteByte(message.ID);
            }
            else
            {
                byte[] buffer = new byte[1 + message.PayLoad.Length];
                buffer[0] = message.ID;
                Array.Copy(message.PayLoad, 0,buffer , 1, message.PayLoad.Length);
                stream.Write(buffer,0, buffer.Length);
            }
        }
        public void Close()
        {
            try
            {
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {

            }
        }
        private void EndConnect(IAsyncResult res)
        {
            try
            {
                client.EndConnect(res);
                stream = client.GetStream();
                BeginRead();
            }
            catch (Exception ex)
            {

            }
        }
        private void BeginRead()
        {
            stream.BeginRead(buffer, 0, BUFFER_SIZE, EndRead, null);
        }
        private void EndRead(IAsyncResult res)
        {
            int count = -1;
            try
            {
                count = stream.EndRead(res);

                InterpretMessage(MessageManager.Deserialize(buffer, count));
            }
            catch (IOException ex)
            {
                Close();
                return;
            }
            
            if (DataReceived != null)
                DataReceived(this, buffer, count);

            BeginRead();
        }
        private void InterpretMessage(Message message)
        {
            if (message == null)
            {
                Close();
                return;
            }
            if (delegates.ContainsKey(message.ID))
                delegates[message.ID](message);
        }
        private void HandleHandshakeRequest(Message message)
        {
            HandshakeRequest req = (HandshakeRequest)message;
            Send(new HandshakeResponse(Username));
            Console.WriteLine("Handshake requested");
        }
        private void HandleHandshakeResponse(Message message)
        {
            HandshakeResponse res = (HandshakeResponse)message;
            Username = res.Username;
            Console.WriteLine(Username + " joined");
        }
    }
}
