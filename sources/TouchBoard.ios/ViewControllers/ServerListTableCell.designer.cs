// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace KeyboardCompanion
{
	[Register ("ServerListTableCell")]
	partial class ServerListTableCell
	{
		[Outlet]
		UIKit.UILabel _ipAddressLbl { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_ipAddressLbl != null) {
				_ipAddressLbl.Dispose ();
				_ipAddressLbl = null;
			}
		}
	}
}
