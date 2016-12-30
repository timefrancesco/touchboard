using System;
using System.Collections.Generic;
using System.Linq;
using Dropins.Chooser.iOS;

using Foundation;
using UIKit;

namespace KeyboardCompanion
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.

	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			
			UIApplication.SharedApplication.IdleTimerDisabled = true;
			FPLog.Instance.InitLog(FPLog.LoggerLevel.LOG_INFORMATION, FPLog.LoggerLevel.LOG_ERROR, FPLog.LoggerLevel.LOG_ERROR, true);
			return true;
		}

		public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			if (DBChooser.DefaultChooser.HandleOpenUrl (url)) {
				// This was a Chooser response and handleOpenURL automatically ran the
				// completion handler   
				return true;
			}
			return false;
		}

		public override void DidEnterBackground (UIApplication application)
		{
			UIApplication.SharedApplication.IdleTimerDisabled = false;
		}

		public override void WillEnterForeground (UIApplication application)
		{
			UIApplication.SharedApplication.IdleTimerDisabled = true;
		}

        public override UIWindow Window {
            get;
            set;
        }
	}
}

