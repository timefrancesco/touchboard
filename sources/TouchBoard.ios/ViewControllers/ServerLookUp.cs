
using System;
using System.Drawing;

using Foundation;
using UIKit;
using System.Collections.Generic;
using ObjCRuntime;
using System.Net;
using System.Threading;

namespace KeyboardCompanion
{
	public partial class ServerLookUp : UIViewController
	{

		private ServerListSource _serverListSource;
		NinjaServer _selectedServer;
		public delegate void SelectedServerDelegate();
		public event SelectedServerDelegate OnServerSelected;
		bool cancelButtonHidden = true;

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

        public ServerLookUp(IntPtr handle) : base (handle) 
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_serversTable.TableFooterView = new UIView();

			_serverListSource = new ServerListSource(); 
			_serverListSource.OnServerSelected += HandleOnServerSelected;
			_serversTable.Source = _serverListSource;

			_serversTable.Hidden = true;
			startupView.Hidden = false;

			CommsEngine.Instance.OnServerReply += HandleOnServerReply;

			_activityIndicator.Hidden = true;
			_startBtn.Layer.CornerRadius = 5;
			downloadHelperButton.Layer.CornerRadius = 5;
			downloadHelperButton.Layer.BorderWidth = 1;
			downloadHelperButton.Layer.BorderColor = Util.TB_LIGHT_GRAY_COLOR.CGColor;
			// Perform any additional setup after loading the view, typically from a nib.

			refreshButton.SetImage(Images.RefreshImage, UIControlState.Normal);
			cancelButton.Hidden = cancelButtonHidden;
		}

		void HandleOnServerSelected (NinjaServer server)
		{
			_selectedServer = server;

			CommsEngine.Instance.OnClientConnected += HandleOnClientConnected;
			CommsEngine.Instance.StartClient(_selectedServer.IpAddress,true);
		}

		void HandleOnClientConnected (bool connected)
		{
			CommsEngine.Instance.OnClientConnected -= HandleOnClientConnected;
			if (connected) {
				Util.SaveServerIP (_selectedServer.IpAddress, _selectedServer.OsX); 
				Util.GetServerIP (); // used so we set the osx/win variable
				Util.IsOSxServer = _selectedServer.OsX;
				if (OnServerSelected != null)
					OnServerSelected ();
				/*InvokeOnMainThread (() => {
					this.DismissViewController (true, null);
				});*/
			} else {
			
				new UIAlertView ("Error".Localize(), "Server refused the connection".Localize(), null, "Ok".Localize ()).Show();
			
			}
		}

		partial void cancelButtonTouchUpInside(NSObject sender)
		{
			this.DismissViewController(true, null);
		}

		public void hideCancelButton(bool hidden)
		{
			cancelButtonHidden = hidden;
		}

		partial void BeginServerLookUp (NSObject sender)
		{
			_activityIndicator.Hidden = false;
			_activityIndicator.StartAnimating();

			//new Thread (() => { 
			//	CommsEngine.Instance.BegineReceiveBroadcastResponse();
			//}).Start();


			_serverListSource.UpdateSource(new List<NinjaServer>());

			_startBtn.Hidden = true;

			CommsEngine.Instance.SendBroadcast();

		}

		void HandleOnServerReply (string serverAddress, bool isOsx)
		{

			if (serverAddress == null) //timeout
			{
				InvokeOnMainThread (() => {
					_activityIndicator.Hidden = true;
					_activityIndicator.StopAnimating ();
					_startBtn.Hidden = false;

				});
				return;
			}

			FPLog.Instance.WriteLine ("[DSVRY] server reply: {0}", FPLog.LoggerLevel.LOG_INFORMATION, serverAddress);
			NinjaServer ns = new NinjaServer{ IpAddress = serverAddress, OsX = isOsx };
			_serverListSource.AddNewServer (ns);

			InvokeOnMainThread (() => {
				_serversTable.ReloadData ();
				_serversTable.Hidden = false;
				startupView.Hidden = true;
			});

		}

		partial void onDownloadHelperTouchUpInside(NSObject sender)
		{
			UIAlertView fullVersionAlert = new UIAlertView("Helper Download".Localize(), "Browse www.timelabs.io/touchboard from your Mac to download the helper".Localize(), null, "Ok".Localize());
			fullVersionAlert.Show();
		}
	}

	public class ServerListSource: UITableViewSource
	{
		private List<NinjaServer> _data;
		public delegate void ServerSelectedDelegate(NinjaServer server);
		public event ServerSelectedDelegate OnServerSelected;

		public ServerListSource()
		{
			_data = new List<NinjaServer> ();
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{

			ServerListTableCell cell = tableView.DequeueReusableCell ("singleServer") as ServerListTableCell;

			if (cell == null) {			
				var views = NSBundle.MainBundle.LoadNib ("ServerListTableCell_iPad", tableView, null); 
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
					views = NSBundle.MainBundle.LoadNib ("ServerListTableCell_iPhone", tableView, null);

				cell = Runtime.GetNSObject (views.ValueAt (0)) as ServerListTableCell;
			}

			if (indexPath.Row % 2 == 0)
				cell.BackgroundColor = UIColor.FromRGB (230, 230, 230);

			//updates the cell
			cell.UpdateData (_data [indexPath.Row].IpAddress);

			return cell;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _data.Count;
		}

		public void UpdateSource(List<NinjaServer> servers)
		{
			_data = servers;
		}

		public void AddNewServer(NinjaServer server)
		{
			foreach (var serv in _data)
			{
				if (serv.IpAddress == server.IpAddress)
					return;
			}

			_data.Add (server);
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath, false);
			if (OnServerSelected != null)
				OnServerSelected (_data[indexPath.Row]);

		}
	}
}

