using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MoonTaxi.Network
{
    internal class Server
    {
        private TcpListener listener;
        private List<Client> clients;
        private delegate void HandleMessageDelegate(Message message);
        private Dictionary<byte, HandleMessageDelegate> delegates;
        public Server()
        {
            clients = new List<Client>();
            delegates = new Dictionary<byte, HandleMessageDelegate>();
        }

        public void Start()
        {
            listener = new TcpListener(1234);
            listener.Start();
            BeginAccept();
        }
        public void Stop()
        {
            listener.Stop();
            foreach (Client client in clients)
                client.Close();
        }
        private void BeginAccept()
        {
            listener.BeginAcceptTcpClient(EndGetClient, null);
        }
        private void EndGetClient(IAsyncResult res)
        {
            try
            {
                TcpClient tcpClient = listener.EndAcceptTcpClient(res);
                Client client = new Client(tcpClient);
                client.DataReceived += Client_DataReceived;

                HandshakeRequest req = new HandshakeRequest(1);
                client.Send(req);

                clients.Add(client);
            }
            catch (SocketException ex)
            {
                //if (ex.Err)
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            BeginAccept();
        }

        private void Client_DataReceived(Client sender, byte[] buffer, int count)
        {
            Message message = MessageManager.Deserialize(buffer, count);
            if (message == null)
            {
                sender.Close();
                clients.Remove(sender);
                return;
            }
            InterpretMessage(message);
            Console.WriteLine("Message: " + System.Text.Encoding.Default.GetString(buffer, 0, count));
        }
        private void InterpretMessage(Message message)
        {
            if (delegates.ContainsKey(message.ID))
                delegates[message.ID](message);
        }
    }
}
