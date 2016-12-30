
using System;
using System.Drawing;
using System.Linq;
using Foundation;
using UIKit;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonoTouch.Dialog;
using Dropins.Chooser.iOS;

namespace KeyboardCompanion
{
    public partial class KeyboardVC : UIViewController 
	{
		KeySource _keySourceData;
		bool _editModeEnabled = false;
		EditKeyVC _editKeyViewController;
		KeyConfiguration _loadedConfiguration;
		UIPopoverController _configListPopOver;
		ConfigurationsListSource _configsSource;
		RemoteConfigsListVC _remoteConfigsVC;
		UIActivityIndicatorView _activityIndicator;
		ServerLookUp _serverVC;
        NSIndexPath _selIndPath;

        public KeyboardVC(IntPtr handle) : base (handle) 
		{
            
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();			
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			bannerView.Hidden = true;

			UIApplication.SharedApplication.SetStatusBarHidden (true, false);
			Util.SetColorsOnStartup ();
			this.KeysCollectionView.BackgroundColor = Util.BackgroundColor;

			ClearButton (ExportConfigBtn);
			ExportConfigBtn.SetImage (Images.ExportConfigImg, UIControlState.Normal);
			ClearButton (ImportConfigBtn);
			ImportConfigBtn.SetImage (Images.ImportConfigImg, UIControlState.Normal);
			ClearButton (NewConfigBtn);
			NewConfigBtn.SetImage (Images.NewConfigImg, UIControlState.Normal);
			ClearButton (SearchConfigBtn);
			SearchConfigBtn.SetImage (Images.SearchConfigImg, UIControlState.Normal);
			ClearButton (SelectConfigBtn);
			SelectConfigBtn.SetImage (Images.ListConfigsImg, UIControlState.Normal);
			ClearButton (SettingsBtn);
			SettingsBtn.SetImage (Images.SettingsImg , UIControlState.Normal);
			ClearButton (EditBtn);
			EditBtn.SetImage (Images.EditConfigImg , UIControlState.Normal);
			//EditBtn.SetImage(Images.EditConfigImgSelected,UIControlState.Selected);

			//load last used configuration or default 
			if (Util.GetLastConfigurationUsed () == null) { //this is the first run ever
				DBManager.Instance.CreateTables ();
				_keySourceData = new KeySource (new List<FPKey> ());
			} else {
				_loadedConfiguration = ConfigsEngine.LoadConfiguration (Util.GetLastConfigurationUsed ());
				_keySourceData = new KeySource (_loadedConfiguration.Keys);
				ConfigNameLbl.Text = _loadedConfiguration.Name.ToUpper();
			}

			//TODO add iphone
			KeysCollectionView.RegisterNibForCell(UINib.FromName ("KeyCell_iPad", NSBundle.MainBundle), KeyCell.Key);

			_keySourceData.KeyPressed += HandleKeyPressed;
            _keySourceData.OnKeyMoved += HandleKeyMoved;
			KeysCollectionView.Source = _keySourceData;
			KeysCollectionView.DelaysContentTouches = false;

            var longPressGesture = new UILongPressGestureRecognizer(HandleLongGesture);
            KeysCollectionView.AddGestureRecognizer(longPressGesture);

			#region Config popOver
			UIViewController configListViewController = new UIViewController ();
			configListViewController.View = ConfigListTableView;
			configListViewController.View.BackgroundColor = UIColor.White;
			_configListPopOver = new UIPopoverController (configListViewController);
			_configListPopOver.BackgroundColor = UIColor.White;

			_configsSource = new ConfigurationsListSource();
			_configsSource.OnConfigurationSelected += OnConfigurationSelected;
			_configsSource.OnConfigurationDeleted += OnConfigurationDeleted;

			ConfigListTableView.Source = _configsSource;
			ConfigListTableView.ReloadData ();
			ConfigListTableView.TableFooterView = new UIView ();
			#endregion

			Util.OnEnableNightMode += OnNightModeChanged;

#if !DISCONNECTED
			CommsEngine.Instance.OnServerDisconnected += HandleOnServerDisconnected;
			if (Util.GetServerIP () == null) { //first Time
				PresentServerSelection();
			} else {
				CommsEngine.Instance.OnClientConnected += HandleOnClientConnected;
				ConnectToLastKnownServer();
			}
#else
			if (Util.GetLastConfigurationUsed() == null) {
				LoadDefaultConfiguration();
			}
			#endif 
		}

		private async Task<bool> LoadDefaultConfiguration()
		{
			_loadedConfiguration = await ConfigsEngine.LoadDefaultConfiguration(Util.IsOSxServer);
			Util.SetLastConfigurationUsed(_loadedConfiguration.Name);
			LoadConfiguration(_loadedConfiguration);
			return true;
		}

        private void HandleKeyMoved (NSIndexPath destination)
        {
            var sourceCell = _loadedConfiguration.Keys[_selIndPath.Row];
            _loadedConfiguration.Keys.RemoveAt(_selIndPath.Row);
            _loadedConfiguration.Keys.Insert(destination.Row, sourceCell);
			KeysCollectionView.ReloadData();
			ConfigsEngine.SaveConfiguration(_loadedConfiguration);
        }

		//used to remove colors and text of an image button, they are colored in xib otherwise i lose them!
		private void ClearButton(UIButton button)
		{
			button.BackgroundColor = UIColor.Clear;
			button.SetTitle ("", UIControlState.Normal);
			button.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			button.ContentMode = UIViewContentMode.ScaleAspectFit;
			button.ImageView.TintColor = Util.TB_BLUE_COLOR;
		}

		private void ConnectToLastKnownServer()
		{			
			DisplayConnectingView ();
			new Task (() => { 
				CommsEngine.Instance.StartClient (Util.GetServerIP (), false);
			}).Start ();
		}

		private void PresentServerSelection(bool hideCancelButton = true)
		{
			if (_serverVC == null) {

                var storyBoard = UIStoryboard.FromName ("MainStoryBoard", null);
                _serverVC = storyBoard.InstantiateViewController ("ServerLookUp") as ServerLookUp;
                _serverVC.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
                _serverVC.OnServerSelected += HandleOnServerSelected;
				_serverVC.hideCancelButton(hideCancelButton);

			}
			this.PresentViewController (_serverVC, true, null);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.NavigationController.SetNavigationBarHidden (true, true);

			KeysCollectionView.ReloadData ();
		}

		/// <summary>
		/// A configuration has been selected from the list of configurations tableview
		/// </summary>
		/// <param name="name">the configuration name</param>
		void OnConfigurationSelected (string name)
		{
			FPLog.Instance.WriteLine ("Configuration Selected: {0}", FPLog.LoggerLevel.LOG_INFORMATION, name);
			var config = ConfigsEngine.LoadConfiguration (name);
			if (config != null)
				LoadConfiguration (config);
			else {
				UIAlertView alertError = new UIAlertView("Error".Localize(),"Error Loading Configuration".Localize(),null,"Ok".Localize());
				alertError.Show();
			}
			_configListPopOver.Dismiss (true);
		} 

		/// <summary>
		/// A configuration has been deleted from the list of configurations tableview
		/// </summary>
		/// <param name="name">the configuration name</param>
		async void OnConfigurationDeleted (string name)
		{
			FPLog.Instance.WriteLine ("Configuration Deleted: {0}", FPLog.LoggerLevel.LOG_INFORMATION, name);
			if (_loadedConfiguration.Name == name)
				new UIAlertView ("Error".Localize (), "Configuration In Use".Localize (), null, "Ok".Localize ()).Show ();
			else {
				ConfigsEngine.DeleteConfiguration (name);
				_configsSource.UpdateData (await ConfigsEngine.GetListOfConfigurations ());
				ConfigListTableView.ReloadData ();
			}

		}

		/// <summary>
		/// Load the selected configuration on the screen
		/// </summary>
		/// <param name="configuration">the configuration object</param>
		private void LoadConfiguration(KeyConfiguration configuration)
		{
			InvokeOnMainThread (() => {
				_loadedConfiguration = configuration;
				Util.SetLastConfigurationUsed (_loadedConfiguration.Name);
				_keySourceData.UpdateSource (_loadedConfiguration.Keys);
				KeysCollectionView.ReloadData ();
				ConfigNameLbl.Text = _loadedConfiguration.Name.ToUpper ();
			});
		}

		/// <summary>
		/// Enable, disable night mode
		/// </summary>
		/// <param name="nightModeEnabled">self explanatory</param>
		void OnNightModeChanged (bool nightModeEnabled)
		{
			this.KeysCollectionView.BackgroundColor = Util.BackgroundColor;

			this.View.SetNeedsDisplay ();
			KeysCollectionView.SetNeedsDisplay ();
			ConfigNameLbl.SetNeedsDisplay ();

			KeysCollectionView.ReloadData ();
		}

		/// <summary>
		/// A key on the keycollectionview has been pressed
		/// </summary>
		/// <param name="key">The pressed Key.</param>
		void HandleKeyPressed (FPKey key)
		{
			if (_editModeEnabled) {
				if (_editKeyViewController == null) {
					_editKeyViewController = new EditKeyVC ();
					_editKeyViewController.OnDismissViewController += HandleDismissEditKeyVC;
				}
				_editKeyViewController.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				_editKeyViewController.SetKey (key);
				_editKeyViewController._isOsx = _loadedConfiguration.OSx;
				this.PresentViewController (_editKeyViewController, true, null);
			} else {
				CommsEngine.Instance.Send(key.Action);
				if (key.PersistSelected) //needs to change the color
					KeysCollectionView.ReloadData ();
			}
		}

		/// <summary>
		/// The edit key view controller has been dismissed
		/// </summary>
		/// <param name="save">If set to <c>true</c> reload the configuration .</param>
		/// <param name="applyStyleToAll">If set to <c>true</c> apply the style to all keys</param>
		void HandleDismissEditKeyVC (bool save, bool applyStyleToAll)
		{
			if (save) {
				if (applyStyleToAll)
					ApplyStyleToAllKeys (_editKeyViewController._loadedKey);
				ConfigsEngine.SaveConfiguration (_loadedConfiguration);
				KeysCollectionView.ReloadData();

			}
			_editKeyViewController.DismissViewController (true, null);
		}

        void HandleLongGesture(UIGestureRecognizer gesture)
        {
            if (_editModeEnabled) return;

            switch (gesture.State)
            {
                case UIGestureRecognizerState.Began:
                    _selIndPath = KeysCollectionView.IndexPathForItemAtPoint(gesture.LocationInView(View));
                    if (_selIndPath != null){
                        KeysCollectionView.BeginInteractiveMovementForItem(_selIndPath);
                    }                   
                    break;
                case UIGestureRecognizerState.Changed:                    
                    KeysCollectionView.UpdateInteractiveMovement(gesture.LocationInView(View));
                    break;
                case UIGestureRecognizerState.Ended:
                    KeysCollectionView.EndInteractiveMovement();
                    break;
                default:
                    KeysCollectionView.CancelInteractiveMovement();
                    break;
            }
        }

		/// <summary>
		/// Given on key, it apply the same style to all the other keys
		/// </summary>
		/// <param name="key">The Key or origin</param>
		private void ApplyStyleToAllKeys(FPKey key)
		{
			if (key == null) return;

			foreach (var ckey in _loadedConfiguration.Keys) {
			
				ckey.BackgroundColor = key.BackgroundColor;
				ckey.MainTitleColor = key.MainTitleColor;
				ckey.SelectedMainTitleColor = key.SelectedMainTitleColor;
				ckey.SelectedBackgroundColor = key.SelectedBackgroundColor;
				ckey.SubTitleColor = key.SubTitleColor;
				ckey.SelectedSubTitleColor = key.SelectedSubTitleColor;
			}
		}

		#region Buttons Actions

		/// <summary>
		/// Switch the keys from edit mode to normal mode and vice-versa. 
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void EditModeChange (NSObject sender)
		{
			_editModeEnabled = !_editModeEnabled;

			EditBtn.Selected = _editModeEnabled;
			_keySourceData.SetEditMode(_editModeEnabled);
			KeysCollectionView.ReloadData();
			DisableActionButtons(_editModeEnabled);
		}

		partial void NewConfiguration (NSObject sender)
		{
			UIAlertView alert = new UIAlertView("New Configuration".Localize(),"Please Enter Name".Localize(),null,"Cancel".Localize(),"Create Empty".Localize(), "Copy Current".Localize());
			alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

			alert.Clicked += (object s, UIButtonEventArgs e) => 
			{
				if (e.ButtonIndex != 0)
				{
					string name = alert.GetTextField(0).Text;
					if (name == "")
						return;

					KeyConfiguration conf = null;


					if (e.ButtonIndex == 2) //copy
						conf = ConfigsEngine.CreateNewConfiguration(name, _loadedConfiguration);
					else if (e.ButtonIndex == 1) //empty
						conf = ConfigsEngine.CreateNewConfiguration(name, null);
					

					if (conf != null){
						LoadConfiguration(conf);
					}
					else
					{
						UIAlertView alertError = new UIAlertView("Error".Localize(),"Configuration already exists".Localize(),null,"Ok".Localize());
						alertError.Show();
					}
				}
			};

			alert.Show();
		}

		/// <summary>
		/// load the list of configurations
		/// </summary>
		/// <param name="sender">Sender.</param>
		/*async partial void SelectConfiguration (NSObject sender)
		{
			
		}*/

		async partial void OnSelectConfigurationBtnTouchUpInside(NSObject sender)
		{
			if (_configListPopOver.PopoverVisible)
				return;

			_configListPopOver.PresentFromRect(SelectConfigBtn.Frame, View, UIPopoverArrowDirection.Up, true);
			ConfigListTableView.Hidden = true;
			UIActivityIndicatorView activityIndicator = new UIActivityIndicatorView(new CoreGraphics.CGRect(ConfigListTableView.Frame.Width / 2 - 25, ConfigListTableView.Frame.Height / 2 - 25, 50, 50));
			activityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
			activityIndicator.StartAnimating();
			ConfigListTableView.AddSubview(activityIndicator);

			var confs = await ConfigsEngine.GetListOfConfigurations();
			_configsSource.UpdateData(confs);
			ConfigListTableView.ReloadData();
			ConfigListTableView.Hidden = false;
			activityIndicator.RemoveFromSuperview();
		}

		/// <summary>
		/// Exports the configuration. Create a zip file which can be shared
		/// </summary>
		/// <param name="sender">Sender.</param>
		async partial void ExportConfiguration (NSObject sender)
		{
			UIActivityIndicatorView activityIndicator = new UIActivityIndicatorView(ExportConfigBtn.Frame);
			activityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
			activityIndicator.StartAnimating();
			this.View.AddSubview(activityIndicator);
			ExportConfigBtn.Hidden = true;
			string zip = await ConfigsEngine.CreateFileForExport(_loadedConfiguration);
			activityIndicator.StopAnimating();
			activityIndicator.RemoveFromSuperview();
			ExportConfigBtn.Hidden = false;

			if (zip == string.Empty)
			{
				UIAlertView alertError = new UIAlertView("Error".Localize(),"Error Creating Zip File".Localize(),null,"Ok".Localize());
				alertError.Show();
				return;
			}

			var viewer = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(zip));
			viewer.PresentOpenInMenu (ExportConfigBtn.Frame, this.View, true);
		}

		/// <summary>
		/// Opens the settings view
		/// </summary>
		/// <param name="sender">Sender.</param>
		SettingsVC settingsVC = new SettingsVC();
		DialogViewController dvc; 
		partial void OpenSettings (NSObject sender)
		{
			settingsVC.View.BackgroundColor = UIColor.White;
			dvc = new DialogViewController (settingsVC.SetUpScreen (), true);
			dvc.View.BackgroundColor = UIColor.White;
			dvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			settingsVC.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			settingsVC.ShowServerSelectionEvent += HandleSettingPresentServerSelection;

			dvc.NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Done, (s, e) => {
				settingsVC.ShowServerSelectionEvent -= HandleSettingPresentServerSelection;
				this.DismissViewController(true,null);
			});
			dvc.NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Back".Localize (), UIBarButtonItemStyle.Plain, null);

			UINavigationController settingsNav = new UINavigationController(dvc);
			settingsNav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			this.PresentViewController (settingsNav, true,null);
		}

		void HandleSettingPresentServerSelection()
		{
			settingsVC.ShowServerSelectionEvent -= HandleSettingPresentServerSelection;
			dvc.DismissViewController(true, null);
			PresentServerSelection(false);
		}

		/// <summary>
		/// Import a configuration from dropbox
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void ImportConfiguration (NSObject sender)
		{
			DBChooser.DefaultChooser.OpenChooser (DBChooserLinkType.Direct, this, async  (results)  =>  {

				if (results == null)
				{
					// results is null if the user cancels
					//new UIAlertView ("Cancelled", "User cancelled!", null, "Continue").Show ();
				}
				else {
					var confs = await ConfigsEngine.ImportFile(results[0].Link.ToString(), results[0].Name);
					InvokeOnMainThread (() => {
						new UIAlertView ("Import", confs, null, "Ok".Localize()).Show ();
					});
				} 
			});
		}

		partial void SearchConfigOnline (NSObject sender)
		{
			if (_remoteConfigsVC == null)
			{
				_remoteConfigsVC = new RemoteConfigsListVC();
				UINavigationController navc = new UINavigationController(_remoteConfigsVC);
				navc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				this.PresentViewController(navc,true,null);
			}
		}

		#endregion

		#region Comms
		public void ReceivedDataFromServer()
		{
		
		
		}

		public void ClientErrorOccured()
		{

		}

		//connection was successfull
		void HandleOnClientConnected (bool connected)
		{
			if (connected) {
				CommsEngine.Instance.OnClientConnected -= HandleOnClientConnected;
				HideConnectingView ();
			} else {
				//handled in server selection popup
			} 
		}

		async void HandleOnServerSelected ()
		{
			if (_loadedConfiguration == null) //firt time
			{
				await LoadDefaultConfiguration();
			}

			InvokeOnMainThread (() => {
				_serverVC.DismissViewController (true, null);
			});
			_serverVC.OnServerSelected -= HandleOnServerSelected;

			HideConnectingView ();
		}

		UIAlertView disconnectedAlert;
		void HandleOnServerDisconnected ()
		{
			InvokeOnMainThread (() => {
				if (disconnectedAlert != null && disconnectedAlert.Visible)
				{
					return;
				}
				disconnectedAlert = new UIAlertView ("Connection Error".Localize (), "Server is unreachable".Localize (), null, "Try to reconnect".Localize (), "Select new server".Localize ());
				disconnectedAlert.Show ();

				disconnectedAlert.Clicked += (object s, UIButtonEventArgs e) => {
					if (e.ButtonIndex == 0) {
						CommsEngine.Instance.OnClientConnected += HandleOnClientConnected;
						DisplayConnectingView ();
						ConnectToLastKnownServer ();
					} else{
						PresentServerSelection ();
					}
				};
			});
		}

		#endregion 

		//it disables the buttons (add config, etc,) except the edit config button
		private void DisableActionButtons(bool disabled)
		{
			ExportConfigBtn.Enabled = !disabled;
			ImportConfigBtn.Enabled = !disabled;
			NewConfigBtn.Enabled = !disabled;
			SearchConfigBtn.Enabled = !disabled;
			SelectConfigBtn.Enabled = !disabled;
			SettingsBtn.Enabled = !disabled;
		}

		//when connecting to a server it shows the view connecting
		private void DisplayConnectingView()
		{
			InvokeOnMainThread (() => {
				if (_activityIndicator == null){
					_activityIndicator = new UIActivityIndicatorView(this.View.Frame);
					_activityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
					_activityIndicator.BackgroundColor = UIColor.DarkGray;
					_activityIndicator.Alpha = 0.6f;
				}
				this.View.AddSubview(_activityIndicator);
				_activityIndicator.StartAnimating();
				KeysCollectionView.UserInteractionEnabled = false;

			});
		}

		private void HideConnectingView()
		{
			InvokeOnMainThread (() => {
				if (_activityIndicator != null)
					_activityIndicator.RemoveFromSuperview ();
				KeysCollectionView.UserInteractionEnabled = true;
			});		
		}

		private void ShowFullVersionAlert()
		{
			UIAlertView fullVersionAlert = new UIAlertView("Full Version".Localize(), "To create a new configuration you need to unlock the full version. Do you want to unlock it now?".Localize(), null, "No".Localize(), "Yes".Localize());
			fullVersionAlert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

			fullVersionAlert.Clicked += (object s, UIButtonEventArgs e) =>
			{
				if (e.ButtonIndex != 0)
				{

				}

			};

			fullVersionAlert.Show();
		}
	}

	public class KeySource:UICollectionViewSource
	{
		List<FPKey> _datasource;
		bool _editmode = false;

		public delegate void KeySelectedDelegate(FPKey key);
		public event KeySelectedDelegate KeyPressed;

        public delegate void KeyMovedDelegate(NSIndexPath destination);
        public event KeyMovedDelegate OnKeyMoved;

		public KeySource(List<FPKey> source)
		{
			_datasource = source;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			var cell = (KeyCell)collectionView.DequeueReusableCell (KeyCell.Key, indexPath);
			cell.Layer.ShouldRasterize = true;
			cell.Layer.RasterizationScale = UIScreen.MainScreen.Scale;

			if (_datasource.Count > 0)
				cell.UpdateContent (_datasource [indexPath.Row],_editmode);
			else
				cell.UpdateContent (new FPKey(),false); //first time


			return cell;
		}

		public void UpdateSource(List<FPKey> keys)
		{
			_datasource = keys;
		}

		public override nint NumberOfSections (UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return 20;
		}

		public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
		{
			return true;
		}

		public override void ItemHighlighted (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (!_editmode ) {
				var cell = (KeyCell)collectionView.DequeueReusableCell (KeyCell.Key, indexPath);
				if (_datasource [indexPath.Row].PersistSelected) //turn on/off
					cell.SwitchKeyOnOff (_datasource [indexPath.Row]);

				//cell.TestSelect ();
				//cell.SetNeedsDisplay ();
				//collectionView.ReloadItems(new NSIndexPath[]{indexPath});
			}
				
			/*var cell2 = collectionView.CellForItem (indexPath) as KeyCell;
			cell2.BackgroundColor = UIColor.Purple;
			collectionView.ReloadItems(new NSIndexPath[]{indexPath});*/

			Console.WriteLine ("highlighted");
		}

		public override void ItemUnhighlighted (UICollectionView collectionView, NSIndexPath indexPath)
		{
			

			if (KeyPressed != null)
				KeyPressed (_datasource [indexPath.Row]);

			collectionView.ReloadItems (new NSIndexPath[]{ indexPath });

			Console.WriteLine ("unhightlighted");
		}

		public override void ItemDeselected (UICollectionView collectionView, NSIndexPath indexPath)
		{

			Console.WriteLine ("deselected");

		}

        public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            if (OnKeyMoved != null ){
                OnKeyMoved(destinationIndexPath);
            }
        }
            

		public void SetEditMode(bool editmode)
		{
			_editmode = editmode;
		}
	}

	public class ConfigurationsListSource : UITableViewSource
	{
		List<string> _source;
		public  delegate void ConfigurationSelectedDelegate(string name);
		public event ConfigurationSelectedDelegate OnConfigurationSelected; 
		public event ConfigurationSelectedDelegate OnConfigurationDeleted; 

		public ConfigurationsListSource ()
		{
			_source = new List<string> ();
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{


			UITableViewCell cell = tableView.DequeueReusableCell ("cell");
			// if there are no cells to reuse, create a new one
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, "cell");
			cell.TextLabel.Text = _source [indexPath.Row];
			//cell.TextLabel.Font = UIFont.FromName ("HelveticaNeue-Light", 18f);
			cell.TextLabel.TextColor = Util.TB_BLUE_COLOR;

			return cell;
		}

		public void UpdateData(List<String> data)
		{
			_source = data;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _source.Count;
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			//tableView.DeleteRows (new NSIndexPath[]{ indexPath }, UITableViewRowAnimation.Fade);
			if (OnConfigurationDeleted != null)
				OnConfigurationDeleted (_source [indexPath.Row]);
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath,false);
			if (OnConfigurationSelected != null) {
				OnConfigurationSelected (_source [indexPath.Row]);
			}

		}
	}

}

