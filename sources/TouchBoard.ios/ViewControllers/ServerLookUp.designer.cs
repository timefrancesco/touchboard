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
	[Register ("ServerLookUp")]
	partial class ServerLookUp
	{
		[Outlet]
		UIKit.UIActivityIndicatorView _activityIndicator { get; set; }

		[Outlet]
		UIKit.UILabel _selectSrvLbl { get; set; }

		[Outlet]
		UIKit.UITableView _serversTable { get; set; }

		[Outlet]
		UIKit.UIButton _startBtn { get; set; }

		[Outlet]
		UIKit.UIButton cancelButton { get; set; }

		[Outlet]
		UIKit.UIButton downloadHelperButton { get; set; }

		[Outlet]
		UIKit.UIButton refreshButton { get; set; }

		[Outlet]
		UIKit.UIView startupView { get; set; }

		[Action ("BeginServerLookUp:")]
		partial void BeginServerLookUp (Foundation.NSObject sender);

		[Action ("cancelButtonTouchUpInside:")]
		partial void cancelButtonTouchUpInside (Foundation.NSObject sender);

		[Action ("onDownloadHelperTouchUpInside:")]
		partial void onDownloadHelperTouchUpInside (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (refreshButton != null) {
				refreshButton.Dispose ();
				refreshButton = null;
			}

			if (_activityIndicator != null) {
				_activityIndicator.Dispose ();
				_activityIndicator = null;
			}

			if (_selectSrvLbl != null) {
				_selectSrvLbl.Dispose ();
				_selectSrvLbl = null;
			}

			if (_serversTable != null) {
				_serversTable.Dispose ();
				_serversTable = null;
			}

			if (_startBtn != null) {
				_startBtn.Dispose ();
				_startBtn = null;
			}

			if (cancelButton != null) {
				cancelButton.Dispose ();
				cancelButton = null;
			}

			if (downloadHelperButton != null) {
				downloadHelperButton.Dispose ();
				downloadHelperButton = null;
			}

			if (startupView != null) {
				startupView.Dispose ();
				startupView = null;
			}
		}
	}
}
