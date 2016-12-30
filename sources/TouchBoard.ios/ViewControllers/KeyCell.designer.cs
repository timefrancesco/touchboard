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
	[Register ("KeyCell")]
	partial class KeyCell
	{
		[Outlet]
		UIKit.UIImageView Icon { get; set; }

		[Outlet]
		UIKit.UILabel MainTitle { get; set; }

		[Outlet]
		UIKit.UILabel SubTitle { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MainTitle != null) {
				MainTitle.Dispose ();
				MainTitle = null;
			}

			if (SubTitle != null) {
				SubTitle.Dispose ();
				SubTitle = null;
			}

			if (Icon != null) {
				Icon.Dispose ();
				Icon = null;
			}
		}
	}
}
