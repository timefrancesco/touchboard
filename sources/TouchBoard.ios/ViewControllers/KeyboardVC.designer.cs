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
	[Register ("KeyboardVC")]
	partial class KeyboardVC
	{
		[Outlet]
		UIKit.UIView bannerView { get; set; }

		[Outlet]
		UIKit.UITableView ConfigListTableView { get; set; }

		[Outlet]
		UIKit.UILabel ConfigNameLbl { get; set; }

		[Outlet]
		UIKit.UIButton EditBtn { get; set; }

		[Outlet]
		UIKit.UIButton ExportConfigBtn { get; set; }

		[Outlet]
		UIKit.UIButton ImportConfigBtn { get; set; }

		[Outlet]
		UIKit.UICollectionView KeysCollectionView { get; set; }

		[Outlet]
		UIKit.UIButton NewConfigBtn { get; set; }

		[Outlet]
		UIKit.UIButton SearchConfigBtn { get; set; }

		[Outlet]
		UIKit.UIButton SelectConfigBtn { get; set; }

		[Outlet]
		UIKit.UIButton SettingsBtn { get; set; }

		[Action ("EditModeChange:")]
		partial void EditModeChange (Foundation.NSObject sender);

		[Action ("ExportConfiguration:")]
		partial void ExportConfiguration (Foundation.NSObject sender);

		[Action ("ImportConfiguration:")]
		partial void ImportConfiguration (Foundation.NSObject sender);

		[Action ("NewConfiguration:")]
		partial void NewConfiguration (Foundation.NSObject sender);

		[Action ("OnSelectConfigurationBtnTouchUpInside:")]
		partial void OnSelectConfigurationBtnTouchUpInside (Foundation.NSObject sender);

		[Action ("OpenSettings:")]
		partial void OpenSettings (Foundation.NSObject sender);

		[Action ("SearchConfigOnline:")]
		partial void SearchConfigOnline (Foundation.NSObject sender);

		[Action ("SelectConfiguration:")]
		partial void SelectConfiguration (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bannerView != null) {
				bannerView.Dispose ();
				bannerView = null;
			}

			if (ConfigListTableView != null) {
				ConfigListTableView.Dispose ();
				ConfigListTableView = null;
			}

			if (ConfigNameLbl != null) {
				ConfigNameLbl.Dispose ();
				ConfigNameLbl = null;
			}

			if (EditBtn != null) {
				EditBtn.Dispose ();
				EditBtn = null;
			}

			if (ExportConfigBtn != null) {
				ExportConfigBtn.Dispose ();
				ExportConfigBtn = null;
			}

			if (ImportConfigBtn != null) {
				ImportConfigBtn.Dispose ();
				ImportConfigBtn = null;
			}

			if (KeysCollectionView != null) {
				KeysCollectionView.Dispose ();
				KeysCollectionView = null;
			}

			if (NewConfigBtn != null) {
				NewConfigBtn.Dispose ();
				NewConfigBtn = null;
			}

			if (SearchConfigBtn != null) {
				SearchConfigBtn.Dispose ();
				SearchConfigBtn = null;
			}

			if (SelectConfigBtn != null) {
				SelectConfigBtn.Dispose ();
				SelectConfigBtn = null;
			}

			if (SettingsBtn != null) {
				SettingsBtn.Dispose ();
				SettingsBtn = null;
			}
		}
	}
}
