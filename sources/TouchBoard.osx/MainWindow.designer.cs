// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace ninjaKeyserverosx
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSTextField ConnectedClientsLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton DiscoveryModeBtn { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton quit { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ServerStatusLabel { get; set; }

		[Action ("onQuitBtnClick:")]
		partial void onQuitBtnClick (MonoMac.Foundation.NSObject sender);

		[Action ("StartDiscovery:")]
		partial void StartDiscovery (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ConnectedClientsLabel != null) {
				ConnectedClientsLabel.Dispose ();
				ConnectedClientsLabel = null;
			}

			if (DiscoveryModeBtn != null) {
				DiscoveryModeBtn.Dispose ();
				DiscoveryModeBtn = null;
			}

			if (ServerStatusLabel != null) {
				ServerStatusLabel.Dispose ();
				ServerStatusLabel = null;
			}

			if (quit != null) {
				quit.Dispose ();
				quit = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		[Outlet]
		MonoMac.AppKit.NSButton StartStopBtn { get; set; }

		[Action ("StartStop:")]
		partial void StartStop (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (StartStopBtn != null) {
				StartStopBtn.Dispose ();
				StartStopBtn = null;
			}
		}
	}
}
