
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TouchBoardServerComms;

namespace ninjaKeyserverosx
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		#region Constructors

		ConnectivityServer _newServer;
		System.Timers.Timer _secondsTimer;
		int _discoverySeconds = 60;


		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();

			_newServer = new ConnectivityServer(true);
			_newServer.OnClientAskForConnection += _newServer_OnClientAskForConnection;
			_newServer.ClientConnected += _newServer_ClientConnected;
			_newServer.ClientDisconnected += _newServer_ClientDisconnected;
			_newServer.OnMessageReceived += HandleOnMessageReceived;
			_newServer.StartServer();   

			NSStatusItem sItem = NSStatusBar.SystemStatusBar.CreateStatusItem(25);

			NSStatusBarButton statBtn = sItem.Button;
			statBtn.Image = NSImage.ImageNamed("tbar_BarIcon");

			statBtn.Activated += (sender, e) => {
				NSApplication app = NSApplication.SharedApplication;
				app.ActivateIgnoringOtherApps(true);

				this.Window.MakeKeyAndOrderFront(null);
			};
		}
		
		// Shared initialization code
		void Initialize ()
		{
			_secondsTimer = new System.Timers.Timer();
		}

		#endregion

		partial void onQuitBtnClick(NSObject sender)
		{
			_newServer.Close();
			NSApplication.SharedApplication.Terminate(this);
		}

		void _newServer_ClientConnected()
		{
            _secondsTimer.Stop();
			_newServer.StopBroadcast(); 
			_newServer.OnDiscoveryTimeOut -= _newServer_OnDiscoveryTimeOut;

			InvokeOnMainThread(() => {
				DiscoveryModeBtn.Enabled = true;
				ConnectedClientsLabel.StringValue = _newServer.ConnectedClients().ToString();
				ServerStatusLabel.StringValue = "Server Started";
			});
		}

		public override void WindowDidLoad ()
		{
			base.WindowDidLoad ();
			DiscoveryModeBtn.Title = "Enable Discovery";
			ConnectedClientsLabel.StringValue = "0";
		}

		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}

		void _newServer_OnClientAskForConnection(xConnection conn)
		{
			InvokeOnMainThread (() => {
				var alert = new NSAlert {
					MessageText = "Connection Request",
					AlertStyle = NSAlertStyle.Informational
				};

				alert.AddButton ("Allow");
				alert.AddButton ("Deny");

				var returnValue = alert.RunModal ();

				if (returnValue == 1000)
					_newServer.SetAuth (true, conn);
				else
					_newServer.SetAuth (false, conn);
			});
		}

		void HandleOnMessageReceived(string message)
		{
			var commands = JsonConvert.DeserializeObject<List<int>>(message);
			if (commands != null)
				KeyHandler.SendKey(commands);
		}

		void _newServer_ClientDisconnected()
		{
			
		}

		//method called by the server object when it has received the information
		//from the client
		public void ReceivedDataFromClient()
		{
			//access UI components through main UI thread 
			//since cannot from an external thread and print to server's text view
			//  SendKeys.SendWait(_server.GetMessage());
		}

		partial void StartDiscovery (NSObject sender)
		{
			DiscoveryModeBtn.Enabled = false;
			_newServer.OnDiscoveryTimeOut += _newServer_OnDiscoveryTimeOut;

			_secondsTimer.Interval = 1000;
			_discoverySeconds = 60;
			_secondsTimer.Elapsed += (timersender, e) => { 
				_discoverySeconds--;
				if (_discoverySeconds < 0)
					_discoverySeconds = 0;
				if (_discoverySeconds == 0)
				{
					_secondsTimer.Stop();
					_newServer.StopBroadcast();
					_newServer_OnDiscoveryTimeOut();
					return;
				}

				InvokeOnMainThread(() =>
				{
				ServerStatusLabel.StringValue = "Discovery mode ON for " + _discoverySeconds.ToString() + " seconds";
				});
			};
			_secondsTimer.Start();

			new Task(() =>
				{
					_newServer.ReceiveBroadcast();
				}).Start();
		}

		void _newServer_OnDiscoveryTimeOut()
		{
			Console.WriteLine("Discovery Timeout");

			_secondsTimer.Stop();
			_newServer.OnDiscoveryTimeOut -= _newServer_OnDiscoveryTimeOut;

			InvokeOnMainThread(() =>
			{
				ServerStatusLabel.StringValue = "Server Started";
				DiscoveryModeBtn.Enabled = true;
			});

		}
	}
}

