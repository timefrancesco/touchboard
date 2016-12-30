
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace KeyboardCompanion
{
	public partial class ServerListTableCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("ServerListTableCell");
		public static readonly UINib Nib;

		static ServerListTableCell ()
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				Nib = UINib.FromName ("ServerListTableCell_iPhone", NSBundle.MainBundle);
			else
				Nib = UINib.FromName ("ServerListTableCell_iPad", NSBundle.MainBundle);
		}

		public ServerListTableCell (IntPtr handle) : base (handle)
		{
		}

		public static ServerListTableCell Create ()
		{
			return (ServerListTableCell)Nib.Instantiate (null, null) [0];
		}

		public void UpdateData(string ip)
		{
			_ipAddressLbl.Text = ip;
		}
	}
}

