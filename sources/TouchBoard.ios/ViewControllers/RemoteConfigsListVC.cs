
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;

namespace KeyboardCompanion
{
	public partial class RemoteConfigsListVC : UIViewController
	{
		RemoteConfigurationsListSource _source;
		UIActivityIndicatorView _activityIndicator;


		public RemoteConfigsListVC () : base ("RemoteConfigsListVC", null)
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
			
			_source = new RemoteConfigurationsListSource ();
			_source.OnConfigurationSelected += OnConfigSelected;
			RemoteConfigsTable.Source = _source;
			RemoteConfigsTable.ReloadData ();
			RemoteConfigsTable.TableFooterView = new UIView ();

			_activityIndicator = new UIActivityIndicatorView (this.View.Frame);
			_activityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;

			this.NavigationItem.RightBarButtonItem = new  UIBarButtonItem (UIBarButtonSystemItem.Done);
			this.NavigationItem.RightBarButtonItem.Clicked += (object sender, EventArgs e) => this.DismissViewController(true,null);

		}

		private async void OnConfigSelected (RepoConfig config)
		{
			string configZipFile = await ConfigsEngine.ImportFile(config.Path,config.Name);

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			RefreshConfigurationsList ();

		}

		private async void RefreshConfigurationsList()
		{
			RemoteConfigsTable.Hidden = true;
			_activityIndicator.StartAnimating ();
			this.Add (_activityIndicator);
			var configs = await ConfigsEngine.GetServerConfigurations();
			if (configs != null) {
				_source.UpdateData (configs);
				RemoteConfigsTable.ReloadData ();
			}
			RemoteConfigsTable.Hidden = false;
			_activityIndicator.RemoveFromSuperview ();

		}
	}

	public class RemoteConfigurationsListSource : UITableViewSource
	{

		List<RepoConfig> _source;
		public  delegate void ConfigurationSelectedDelegate(RepoConfig config);
		public event ConfigurationSelectedDelegate OnConfigurationSelected; 

		public RemoteConfigurationsListSource ()
		{
			_source = new List<RepoConfig> ();
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{

			UITableViewCell cell = tableView.DequeueReusableCell ("cell");
			// if there are no cells to reuse, create a new one
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, "cell");
			cell.TextLabel.Text = _source [indexPath.Row].Name;
			cell.TextLabel.Font = UIFont.FromName ("HelveticaNeue-Light", 18f);
			//cell.TextLabel.TextColor = Helpers.DefaultBlue;

			return cell;
		}

		public void UpdateData(List<RepoConfig> data)
		{
			_source = data;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _source.Count;
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

