using System;
using MonoTouch.Dialog;
using UIKit;
using MessageUI;
using Foundation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace KeyboardCompanion
{
	public class SettingsVC:UIViewController
	{
		public delegate void EmptyDelegate();
		public event EmptyDelegate ShowServerSelectionEvent;

		public SettingsVC ()
		{
		}

		public override void ViewDidLoad()
		{
			this.View.BackgroundColor = UIColor.White;
		}

		public RootElement SetUpScreen()
		{
			
			var menu = new RootElement ("Settings".Localize ()) {
				
				new Section () {
					
					new StyledStringElement ("Select Server".Localize (), () => {
						#if !DISCONNECTED		
						if (ShowServerSelectionEvent != null )
							ShowServerSelectionEvent();
                 
						#endif
					}){ Font = Util.SettingsFont, TextColor = Util.TB_BLUE_COLOR},

					new StyledStringElement ("Rate TouchBoard".Localize (), () => {
						UIApplication.SharedApplication.OpenUrl(new NSUrl("itms-apps://itunes.apple.com/app/id1187810998"));
					}){ Font = Util.SettingsFont , TextColor = Util.TB_BLUE_COLOR},
				}
			};

			return menu;

		}
	}

	public class SettingsFooter:UIView
	{
		public SettingsFooter(CoreGraphics.CGRect frame):base(frame)
		{
			UILabel iconsLabel = new UILabel (frame);// (new CoreGraphics.CGRect (0,FPHelpers.ScreenWidth-30, frame.Width, 30));
			iconsLabel.Text = "Icons by http://www.icons8.com";
			iconsLabel.Font = UIFont.FromName ("HelveticaNeue-Light", 16f);
			iconsLabel.TextAlignment = UITextAlignment.Center;
			iconsLabel.TextColor = UIColor.Green;//FPTEMP FPHelpers.GetPrimaryFontColor ();

			this.AddSubview (iconsLabel);
		}
	}

	public class FPRadioElement:RadioElement
	{
		Action<FPRadioElement, EventArgs> OnCLick;

		public FPRadioElement (string caption, string group, Action<FPRadioElement, EventArgs> onCLick) : base(string.Empty,string.Empty)
		{
			Caption = caption;
			Group = group;
			OnCLick = onCLick;
		}

		public override UIKit.UITableViewCell GetCell (UIKit.UITableView tv)
		{
			var cell = base.GetCell(tv);
			cell.TextLabel.Font = UIFont.FromName ("HelveticaNeue-Light", 19f);

			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			base.Selected (dvc, tableView, path);
			if (OnCLick != null)
				OnCLick (this, EventArgs.Empty);
		}
	}
}

