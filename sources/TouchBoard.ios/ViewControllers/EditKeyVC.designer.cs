// WARNING
//
// This file has been generated automatically by Xamarin Studio Business to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace KeyboardCompanion
{
	[Register ("EditKeyVC")]
	partial class EditKeyVC
	{
		[Outlet]
		UIKit.UITextField ActionTbox { get; set; }

		[Outlet]
		UIKit.UIButton BackgroundColorBtn { get; set; }

		[Outlet]
		UIKit.UIButton BackroundSelColorBtn { get; set; }

		[Outlet]
		UIKit.UIButton BackspaceBtn { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem CancelBarBtn { get; set; }

		[Outlet]
		UIKit.UIButton CapsLockBtn { get; set; }

		[Outlet]
		UIKit.UIToolbar CloseToolbarBtn { get; set; }

		[Outlet]
		UIKit.UIButton DefaultIconBtn { get; set; }

		[Outlet]
		UIKit.UIButton DelBtn { get; set; }

		[Outlet]
		UIKit.UIButton DeleteEntryBtn { get; set; }

		[Outlet]
		UIKit.UIButton DownBtn { get; set; }

		[Outlet]
		UIKit.UIButton EndBtn { get; set; }

		[Outlet]
		UIKit.UIButton EnterBtn { get; set; }

		[Outlet]
		UIKit.UIButton F10Btn { get; set; }

		[Outlet]
		UIKit.UIButton F11Btn { get; set; }

		[Outlet]
		UIKit.UIButton F12Btn { get; set; }

		[Outlet]
		UIKit.UIButton F1Btn { get; set; }

		[Outlet]
		UIKit.UIButton F2Btn { get; set; }

		[Outlet]
		UIKit.UIButton F3Btn { get; set; }

		[Outlet]
		UIKit.UIButton F4Btn { get; set; }

		[Outlet]
		UIKit.UIButton F5Btn { get; set; }

		[Outlet]
		UIKit.UIButton F6Btn { get; set; }

		[Outlet]
		UIKit.UIButton F7Btn { get; set; }

		[Outlet]
		UIKit.UIButton F8Btn { get; set; }

		[Outlet]
		UIKit.UIButton F9Btn { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem HelpToolbarBtn { get; set; }

		[Outlet]
		UIKit.UIButton HomeBtn { get; set; }

		[Outlet]
		UIKit.UIButton InsBtn { get; set; }

		[Outlet]
		UIKit.UIButton LaltBtn { get; set; }

		[Outlet]
		UIKit.UIButton LctrlBtn { get; set; }

		[Outlet]
		UIKit.UIButton LeftBtn { get; set; }

		[Outlet]
		UIKit.UIButton LshiftBtn { get; set; }

		[Outlet]
		UIKit.UITextField MainTitleTbox { get; set; }

		[Outlet]
		UIKit.UIView ModifierKeysView { get; set; }

		[Outlet]
		UIKit.UIToolbar ModifierToolbar { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem ModifierToolbarBtn { get; set; }

		[Outlet]
		UIKit.UINavigationBar NavBar { get; set; }

		[Outlet]
		UIKit.UISwitch PersistentSwitch { get; set; }

		[Outlet]
		UIKit.UIButton PgDwnBtn { get; set; }

		[Outlet]
		UIKit.UIButton PgUpBtn { get; set; }

		[Outlet]
		UIKit.UILabel PreviewBackground { get; set; }

		[Outlet]
		UIKit.UIImageView PreviewIcon { get; set; }

		[Outlet]
		UIKit.UILabel PreviewSubtitle { get; set; }

		[Outlet]
		UIKit.UILabel PreviewTitle { get; set; }

		[Outlet]
		UIKit.UIButton RaltBtn { get; set; }

		[Outlet]
		UIKit.UIButton RctrlBtn { get; set; }

		[Outlet]
		UIKit.UIButton RemoveDefaultIconBtn { get; set; }

		[Outlet]
		UIKit.UIButton RemoveSelectedIconBtn { get; set; }

		[Outlet]
		UIKit.UIButton RightBtn { get; set; }

		[Outlet]
		UIKit.UIButton RshiftBtn { get; set; }

		[Outlet]
		UIKit.UIView SecondModView { get; set; }

		[Outlet]
		UIKit.UIButton SelectedIconBtn { get; set; }

		[Outlet]
		UIKit.UIButton SubtitleColorBtn { get; set; }

		[Outlet]
		UIKit.UIButton SubtitleSelColorBtn { get; set; }

		[Outlet]
		UIKit.UITextField SubTitleTbox { get; set; }

		[Outlet]
		UIKit.UIButton SuperBtn { get; set; }

		[Outlet]
		UIKit.UIButton TabBtn { get; set; }

		[Outlet]
		UIKit.UIButton TitleColorBtn { get; set; }

		[Outlet]
		UIKit.UIButton TitleSelColorBtn { get; set; }

		[Outlet]
		UIKit.UIButton UpBtn { get; set; }

		[Action ("CloseKeyboard:")]
		partial void CloseKeyboard (Foundation.NSObject sender);

		[Action ("DeleteEntry:")]
		partial void DeleteEntry (Foundation.NSObject sender);

		[Action ("HelpRequested:")]
		partial void HelpRequested (Foundation.NSObject sender);

		[Action ("RemoveDefaultIcon:")]
		partial void RemoveDefaultIcon (Foundation.NSObject sender);

		[Action ("RemoveSelectedIcon:")]
		partial void RemoveSelectedIcon (Foundation.NSObject sender);

		[Action ("SelectDefaultIcon:")]
		partial void SelectDefaultIcon (Foundation.NSObject sender);

		[Action ("SelectSelectedIcon:")]
		partial void SelectSelectedIcon (Foundation.NSObject sender);

		[Action ("SelectSubtitleColor:")]
		partial void SelectSubtitleColor (Foundation.NSObject sender);

		[Action ("SelectTitleColor:")]
		partial void SelectTitleColor (Foundation.NSObject sender);

		[Action ("SwitchKeyboard:")]
		partial void SwitchKeyboard (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ActionTbox != null) {
				ActionTbox.Dispose ();
				ActionTbox = null;
			}

			if (BackgroundColorBtn != null) {
				BackgroundColorBtn.Dispose ();
				BackgroundColorBtn = null;
			}

			if (BackroundSelColorBtn != null) {
				BackroundSelColorBtn.Dispose ();
				BackroundSelColorBtn = null;
			}

			if (BackspaceBtn != null) {
				BackspaceBtn.Dispose ();
				BackspaceBtn = null;
			}

			if (CancelBarBtn != null) {
				CancelBarBtn.Dispose ();
				CancelBarBtn = null;
			}

			if (CapsLockBtn != null) {
				CapsLockBtn.Dispose ();
				CapsLockBtn = null;
			}

			if (CloseToolbarBtn != null) {
				CloseToolbarBtn.Dispose ();
				CloseToolbarBtn = null;
			}

			if (DefaultIconBtn != null) {
				DefaultIconBtn.Dispose ();
				DefaultIconBtn = null;
			}

			if (DelBtn != null) {
				DelBtn.Dispose ();
				DelBtn = null;
			}

			if (DeleteEntryBtn != null) {
				DeleteEntryBtn.Dispose ();
				DeleteEntryBtn = null;
			}

			if (DownBtn != null) {
				DownBtn.Dispose ();
				DownBtn = null;
			}

			if (EndBtn != null) {
				EndBtn.Dispose ();
				EndBtn = null;
			}

			if (EnterBtn != null) {
				EnterBtn.Dispose ();
				EnterBtn = null;
			}

			if (F10Btn != null) {
				F10Btn.Dispose ();
				F10Btn = null;
			}

			if (F11Btn != null) {
				F11Btn.Dispose ();
				F11Btn = null;
			}

			if (F12Btn != null) {
				F12Btn.Dispose ();
				F12Btn = null;
			}

			if (F1Btn != null) {
				F1Btn.Dispose ();
				F1Btn = null;
			}

			if (F2Btn != null) {
				F2Btn.Dispose ();
				F2Btn = null;
			}

			if (F3Btn != null) {
				F3Btn.Dispose ();
				F3Btn = null;
			}

			if (F4Btn != null) {
				F4Btn.Dispose ();
				F4Btn = null;
			}

			if (F5Btn != null) {
				F5Btn.Dispose ();
				F5Btn = null;
			}

			if (F6Btn != null) {
				F6Btn.Dispose ();
				F6Btn = null;
			}

			if (F7Btn != null) {
				F7Btn.Dispose ();
				F7Btn = null;
			}

			if (F8Btn != null) {
				F8Btn.Dispose ();
				F8Btn = null;
			}

			if (F9Btn != null) {
				F9Btn.Dispose ();
				F9Btn = null;
			}

			if (HelpToolbarBtn != null) {
				HelpToolbarBtn.Dispose ();
				HelpToolbarBtn = null;
			}

			if (HomeBtn != null) {
				HomeBtn.Dispose ();
				HomeBtn = null;
			}

			if (InsBtn != null) {
				InsBtn.Dispose ();
				InsBtn = null;
			}

			if (LaltBtn != null) {
				LaltBtn.Dispose ();
				LaltBtn = null;
			}

			if (LctrlBtn != null) {
				LctrlBtn.Dispose ();
				LctrlBtn = null;
			}

			if (LeftBtn != null) {
				LeftBtn.Dispose ();
				LeftBtn = null;
			}

			if (LshiftBtn != null) {
				LshiftBtn.Dispose ();
				LshiftBtn = null;
			}

			if (MainTitleTbox != null) {
				MainTitleTbox.Dispose ();
				MainTitleTbox = null;
			}

			if (ModifierKeysView != null) {
				ModifierKeysView.Dispose ();
				ModifierKeysView = null;
			}

			if (ModifierToolbar != null) {
				ModifierToolbar.Dispose ();
				ModifierToolbar = null;
			}

			if (ModifierToolbarBtn != null) {
				ModifierToolbarBtn.Dispose ();
				ModifierToolbarBtn = null;
			}

			if (NavBar != null) {
				NavBar.Dispose ();
				NavBar = null;
			}

			if (PersistentSwitch != null) {
				PersistentSwitch.Dispose ();
				PersistentSwitch = null;
			}

			if (PgDwnBtn != null) {
				PgDwnBtn.Dispose ();
				PgDwnBtn = null;
			}

			if (PgUpBtn != null) {
				PgUpBtn.Dispose ();
				PgUpBtn = null;
			}

			if (PreviewBackground != null) {
				PreviewBackground.Dispose ();
				PreviewBackground = null;
			}

			if (PreviewIcon != null) {
				PreviewIcon.Dispose ();
				PreviewIcon = null;
			}

			if (PreviewSubtitle != null) {
				PreviewSubtitle.Dispose ();
				PreviewSubtitle = null;
			}

			if (PreviewTitle != null) {
				PreviewTitle.Dispose ();
				PreviewTitle = null;
			}

			if (RaltBtn != null) {
				RaltBtn.Dispose ();
				RaltBtn = null;
			}

			if (RctrlBtn != null) {
				RctrlBtn.Dispose ();
				RctrlBtn = null;
			}

			if (RemoveDefaultIconBtn != null) {
				RemoveDefaultIconBtn.Dispose ();
				RemoveDefaultIconBtn = null;
			}

			if (RemoveSelectedIconBtn != null) {
				RemoveSelectedIconBtn.Dispose ();
				RemoveSelectedIconBtn = null;
			}

			if (RightBtn != null) {
				RightBtn.Dispose ();
				RightBtn = null;
			}

			if (RshiftBtn != null) {
				RshiftBtn.Dispose ();
				RshiftBtn = null;
			}

			if (SecondModView != null) {
				SecondModView.Dispose ();
				SecondModView = null;
			}

			if (SelectedIconBtn != null) {
				SelectedIconBtn.Dispose ();
				SelectedIconBtn = null;
			}

			if (SubtitleColorBtn != null) {
				SubtitleColorBtn.Dispose ();
				SubtitleColorBtn = null;
			}

			if (SubtitleSelColorBtn != null) {
				SubtitleSelColorBtn.Dispose ();
				SubtitleSelColorBtn = null;
			}

			if (SubTitleTbox != null) {
				SubTitleTbox.Dispose ();
				SubTitleTbox = null;
			}

			if (SuperBtn != null) {
				SuperBtn.Dispose ();
				SuperBtn = null;
			}

			if (TabBtn != null) {
				TabBtn.Dispose ();
				TabBtn = null;
			}

			if (TitleColorBtn != null) {
				TitleColorBtn.Dispose ();
				TitleColorBtn = null;
			}

			if (TitleSelColorBtn != null) {
				TitleSelColorBtn.Dispose ();
				TitleSelColorBtn = null;
			}

			if (UpBtn != null) {
				UpBtn.Dispose ();
				UpBtn = null;
			}
		}
	}
}
