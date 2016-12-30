using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TouchBoardServerComms
{
    public class ConnectivityServer
    {

        const int PORT = 15084;
        const int DISCOVERY_PORT = 13085;
        const int _bufferSize = 1024;
        private List<xConnection> _sockets;
        private System.Net.Sockets.Socket _serverSocket;
        public delegate void OnMsgReceivedDelegate(string message);
        public event OnMsgReceivedDelegate OnMessageReceived;
        private Socket _discoverySocket;

        public delegate void EmptyDelegate();
        public event EmptyDelegate ClientConnected;
        public event EmptyDelegate ClientDisconnected;
        public event EmptyDelegate OnDiscoveryTimeOut;

        public delegate void ConnectionRequestDelegate(xConnection conn);
        public event ConnectionRequestDelegate OnClientAskForConnection;

        List<AuthClient> _authorizedClients;
		bool _isOsx;

		public ConnectivityServer(bool isOsx)
        {
            _sockets = new List<xConnection>();
			_isOsx = isOsx;
        }

        public void StartServer()
        {
			
				//  IPHostEntry localhost = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
				IPEndPoint serverEndPoint;
				serverEndPoint = new IPEndPoint(IPAddress.Any, PORT);
				_serverSocket = new Socket(serverEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

				_serverSocket.Bind(serverEndPoint);
				_serverSocket.Listen(10);
				_serverSocket.BeginAccept(new AsyncCallback(OnClientConnect), _serverSocket);

				_authorizedClients = ClientUtil.GetClients();
				Console.WriteLine("[SRV] server started");

        }


        public bool Send(string message, xConnection conn)
        {
            if (conn != null && conn.socket.Connected)
            {
                lock (conn.socket)
                {
                    conn.socket.Send(Encoding.UTF8.GetBytes(message), message.Length, SocketFlags.None);
                }
            }
            else
                return false;
            return true;
        }

        public void SendToAll(string message)
        {
            foreach (var conn in _sockets)
            {
                new Thread(() =>
                {
                    Send(message, conn);
                }).Start();
            }
        }

        public int ConnectedClients()
        {
            return _sockets.Count;
        }

        public void Close()
        {
            foreach (var conn in _sockets)
            {
                // conn.socket.Disconnect(true);
                conn.socket.Close();
            }
            _sockets.Clear();
			if (_serverSocket != null )
			_serverSocket.Close();
			if (_discoverySocket != null)
			_discoverySocket.Close();
        }

        private void OnClientConnect(IAsyncResult result)
        {
            xConnection conn = new xConnection();
            try
            {
                //Finish accepting the connection
                Socket s = (Socket)result.AsyncState;
                conn = new xConnection();
                conn.socket = s.EndAccept(result);
                conn.buffer = new byte[_bufferSize];
                lock (_sockets)
                {
                    _sockets.Add(conn);
                }

               
               // if (ClientConnected != null)
                 //   ClientConnected();

                Console.WriteLine("[SRV] client connected");

                //Queue recieving of data from the connection
                conn.socket.BeginReceive(conn.buffer, 0, conn.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveData), conn);
                //Queue the accept of the next incomming connection
                _serverSocket.BeginAccept(new AsyncCallback(OnClientConnect), _serverSocket);
            }
            catch (Exception e)
            {
                if (conn.socket != null)
                {
                    conn.socket.Close();
                    lock (_sockets)
                    {
                        _sockets.Remove(conn);
                    }
                }
                //Queue the next accept, think this should be here, stop attacks based on killing the waiting listeners
                _serverSocket.BeginAccept(new AsyncCallback(OnClientConnect), _serverSocket);
            }

        }

        private void ReceiveData(IAsyncResult result)
        {
            xConnection conn = (xConnection)result.AsyncState;
            try
            {

                //Grab our buffer and count the number of bytes receives
                int bytesRead = conn.socket.EndReceive(result);
                //make sure we've read something, if we haven't it supposadly means that the client disconnected
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(conn.buffer, 0, bytesRead);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() +  " [SRV] message received: " + message);
                    if (message.Contains("NinjaConnect")) //it's a message from the client who knows my ip
                    {
                       
                            Send("NinjaAck", conn);

                            ClientConnected();
                            //Queue the next receive
                            conn.socket.BeginReceive(conn.buffer, 0, conn.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveData), conn);
                        
                    }
                    else if  (message.Contains("NinjaTestConnect")) //the client first connection request
                    {
                        //we need to ask permission
                         string address = conn.socket.RemoteEndPoint.ToString();
                         address = address.Remove(address.IndexOf(':'));
                        //this is to check if the client is in the authorized clients list
                        if (_authorizedClients != null && _authorizedClients.Where(cl => cl.IPaddress.Contains(address)).FirstOrDefault() != null)
                        {
                            SetAuth(true, conn);
                        }
                        else
                            OnClientAskForConnection(conn);
                    }
                    else if (message.Contains("NinjaKey="))
                    {
                        string key = message.Replace("NinjaKey=", "");

                        if (OnMessageReceived != null)
                            OnMessageReceived(key);

                        conn.socket.BeginReceive(conn.buffer, 0, conn.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveData), conn);
                    }
                    else
                    {
                        //message from someone else
                        conn.socket.Close();
                        _sockets.Remove(conn);
                    }
                }
                else
                {
                    //Callback run but no data, close the connection
                    //supposadly means a disconnect
                    //and we still have to close the socket, even though we throw the event later
                    conn.socket.Close();
                    lock (_sockets)
                    {
                        _sockets.Remove(conn);
                    }
                    if (ClientDisconnected != null)
                        ClientDisconnected();
                }
            }
            catch (SocketException e)
            {
                //Something went terribly wrong
                //which shouldn't have happened
                if (conn.socket != null)
                {
                    conn.socket.Close();
                    lock (_sockets)
                    {
                        _sockets.Remove(conn);
                    }
                }
            }
        }

        public void SetAuth(bool accepted, xConnection conn)
        {
            if (accepted)
            {
                Console.WriteLine("[SRV] Sending auth True");
                Send("NinjaAck", conn);
                ClientConnected();
                conn.socket.BeginReceive(conn.buffer, 0, conn.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveData), conn);
            }
            else
            {
                Console.WriteLine("[SRV] Sending auth false");
                Send("NinjaNACK", conn);
                conn.socket.Close();
                _sockets.Remove(conn);
            }
        }

        public void ReceiveBroadcast()
        {
            Console.WriteLine("[SRV] receive");
            byte[] buffer = new byte[1024];
            _discoverySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var iep = new IPEndPoint(IPAddress.Any, DISCOVERY_PORT);
            _discoverySocket.Bind(iep);
            _discoverySocket.ReceiveTimeout = 60000;
			_discoverySocket.SendTimeout = 5000;

			try
			{
                var ep = iep as EndPoint;
                //_discoverySocket.BeginReceiveFrom(buffer,0,buffer.Length, SocketFlags.None, ref ep, new AsyncCallback(DiscoveryCallback),conn);
                _discoverySocket.ReceiveFrom(buffer, ref ep);


                var data = Encoding.UTF8.GetString(buffer);
                Console.WriteLine("[SRV] Received broadcast: " + data + " from: " + ep.ToString());

				string server = _isOsx?"NinjaServer OSX":"NinjaServer Windows";

				byte[] buffer2 = UTF8Encoding.UTF8.GetBytes(server);
                _discoverySocket.SendTo(buffer2, ep);

                Console.WriteLine("[SRV] Sent reply to broadcast");

				StopBroadcast();
                OnDiscoveryTimeOut();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("[SRV] timeout?");
				StopBroadcast();
                OnDiscoveryTimeOut();
            }

        }

        public void StopBroadcast()
        {
            Console.WriteLine("[SRV] stopping broadcast");
			if (_discoverySocket != null)
			{
				_discoverySocket.Close();
				_discoverySocket.Dispose();
			}
        }

    }

    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class xConnection
    {
        public byte[] buffer;
        public System.Net.Sockets.Socket socket;
    }
}
