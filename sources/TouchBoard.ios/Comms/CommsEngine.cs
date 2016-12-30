using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace KeyboardCompanion
{
	public class CommsEngine
	{
		private static CommsEngine _instance;

		const int PORT = 15084;
		const int DISCOVERY_PORT = 13085;

		Socket _clientSocket;
		//bool _waitingForAck;
		private  ManualResetEvent connectDone = new ManualResetEvent(false);
		private  ManualResetEvent sendDone =  new ManualResetEvent(false);
		//private  ManualResetEvent receiveDone = new ManualResetEvent(false);

		public delegate void ServerDiscoveryDelegate (string serverAddress, bool isOsx);
		public event ServerDiscoveryDelegate OnServerReply;

		public delegate void ConnectDelegate(bool connected);
		public event ConnectDelegate OnClientConnected;

		public delegate void EmptyDelegate();
		public event EmptyDelegate OnServerDisconnected;

		public CommsEngine ()
		{

		}

		public static CommsEngine Instance
		{
			get
			{
				if (_instance == null)
					_instance = new CommsEngine();

				return _instance;
			}
		}   
	
		public void StartClient(string ipAddress, bool firstConnection) 
		{
			Console.WriteLine ("Start Client");
			try 
			{
				if (_clientSocket != null)
				{
					_clientSocket.Close();
					_clientSocket.Dispose();
					_clientSocket = null;
					connectDone.Reset();
				}

				_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				_clientSocket.ReceiveTimeout = 2000;
				_clientSocket.SendTimeout = 2000;
			
				_clientSocket.BeginConnect( ipAddress,PORT, new AsyncCallback(ConnectCallback), _clientSocket);
				var  conn = connectDone.WaitOne(5000);
				if (!conn)
				{
					if (OnServerDisconnected != null)
						OnServerDisconnected();
					return;
				}
				if (firstConnection){
					Send("NinjaTestConnect");
				}
				else{
					Send("NinjaConnect");
				}

				sendDone.WaitOne();

				Receive(_clientSocket);

			} catch (Exception e) {
				Console.WriteLine(e.ToString());
				if (OnServerDisconnected != null)
					OnServerDisconnected();
			}
		}

		public void StopClient()
		{
			if (_clientSocket.Connected) {
				_clientSocket.Shutdown (SocketShutdown.Both);
				_clientSocket.Disconnect (true);
			}
			_clientSocket.Close();
		}

		private void ReceivedData(string received)
		{
			Console.WriteLine("data received = " + received	);

			if (received.Contains ("NinjaAck")) {
				OnClientConnected (true);
			} else
				OnClientConnected (false);
		}

		private void ConnectCallback(IAsyncResult ar) 
		{
			try 
			{
				Socket client = (Socket) ar.AsyncState;
				client.EndConnect(ar);

				Console.WriteLine("Socket connected to {0}",client.RemoteEndPoint.ToString());

				// Signal that the connection has been made.
				connectDone.Set();
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}

		private void Receive(Socket client) 
		{
			try 
			{
				StateObject state = new StateObject();
				state.workSocket = client;

				// Begin receiving the data from the remote device.
				client.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,new AsyncCallback(ReceiveCallback), state);


			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}

		private void ReceiveCallback( IAsyncResult ar ) {
			try {
				StateObject state = (StateObject) ar.AsyncState;
				Socket client = state.workSocket;

				int bytesRead = client.EndReceive(ar);

				if (bytesRead > 0) {
					ReceivedData(Encoding.UTF8.GetString(state.buffer));
				} else {
					Console.WriteLine("Disconnect?");
				}
			} 
			catch (Exception e) {
				Console.WriteLine(e.ToString());
				if (OnServerDisconnected != null)
					OnServerDisconnected();
			}
		}

		public void Send(String data) 
		{
			#if !DISCONNECTED
			string newData = "NinjaKey=" + data;

			Console.WriteLine ("sending: " + newData);

			byte[] byteData = Encoding.UTF8.GetBytes(newData);
			try{
				_clientSocket.BeginSend(byteData, 0, byteData.Length, 0,new AsyncCallback(SendCallback), _clientSocket);
				//timer = new Timer(OnSendTimerElapsed, null, 1000, Timeout.Infinite);
			}
			catch (SocketException e)
			{
				if (OnServerDisconnected != null)
					OnServerDisconnected();
			}
			#endif
		}

		void OnSendTimerElapsed(object obj)
		{
			
		}

		private void SendCallback(IAsyncResult ar) 
		{
			try 
			{
				Socket client = (Socket) ar.AsyncState;

				int bytesSent = client.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to server.", bytesSent);

				sendDone.Set();
			} 
			catch (SocketException e) {
				if (OnServerDisconnected != null)
					OnServerDisconnected ();
				Console.WriteLine(e.ToString());
			}
		}



		public void SendBroadcast()
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

			socket.Connect(new IPEndPoint(IPAddress.Broadcast, DISCOVERY_PORT));


			new Thread (() => { 
				BegineReceiveBroadcastResponse(socket.LocalEndPoint);
			}).Start();

			Thread.Sleep (200);

			SendBroadcastMessage (socket);
		}

		private void SendBroadcastMessage(Socket socket)
		{
			socket.Send(System.Text.UTF8Encoding.UTF8.GetBytes("Ninja client here!"));
			socket.Close();
			Console.WriteLine ("Broadcast Message Sent");
		}



		public void BegineReceiveBroadcastResponse(EndPoint ep)
		{
			byte[] buffer = new byte[256];
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket.ReceiveTimeout = 10000;
			bool waitOneMin = true;

			Console.WriteLine ("Started Receive Broadcast Task");

			socket.Bind(ep);

			EndPoint remote_ep = new IPEndPoint(IPAddress.Any, 0);

			//socket.Bind(remote_ep);

			var startTime = DateTime.Now;

			while (waitOneMin) {
				try{
				socket.ReceiveFrom (buffer, ref remote_ep);
				}catch (SocketException) {
					Console.WriteLine ("discovery timeout");
					waitOneMin = false;
					socket.Close ();

					if (OnServerReply != null) 
						OnServerReply (null, false);
				}
			

				//socket.Receive(buffer);
				var data = UTF8Encoding.UTF8.GetString (buffer);

				if (Encoding.UTF8.GetString (buffer).Contains ("NinjaServer")) {
					Console.WriteLine ("Got reply from " + remote_ep.ToString () + " " + data);

					string addr = remote_ep.ToString ().Replace (":" + DISCOVERY_PORT, ""); 

					bool isOsx = data.Contains ("OSX");

					if (OnServerReply != null) {
						OnServerReply (addr,isOsx);
						//waitOneMin = false;
					}
				}
				else
					Console.WriteLine ("Got reply but not a ninja client " + remote_ep.ToString () + " " + data);

				if (DateTime.Now > startTime.AddSeconds(10))
					waitOneMin = false;
			}
			Console.WriteLine (" CLOSING DISCONVERY SOCKET");
			socket.Close();
			//if (OnServerReply != null) 
			//	OnServerReply (null);
		}



	}

	public class StateObject 
	{
		public Socket workSocket = null; // Client socket.
		public const int BufferSize = 512; // Size of receive buffer.
		public byte[] buffer = new byte[BufferSize]; // Receive buffer.
		public StringBuilder sb = new StringBuilder(); // Received data string.
	}

/*	public interface ClientListener
	{
		//method to be called by client when data has been received from server
		void ReceivedDataFromServer();

		//method to be called by the client when a Socket error occurs
		void ClientErrorOccured();
	}*/
}

