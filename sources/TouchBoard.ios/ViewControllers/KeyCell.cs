
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace KeyboardCompanion
{
	public partial class KeyCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString ("KeyCell");
		public static readonly UINib Nib;

		static KeyCell ()
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				Nib = UINib.FromName ("KeyCell_iPhone", NSBundle.MainBundle);
			else
				Nib = UINib.FromName ("KeyCell_iPad", NSBundle.MainBundle);
		}

		public KeyCell (IntPtr handle) : base (handle)
		{
		}

		public static KeyCell Create ()
		{
			return (KeyCell)Nib.Instantiate (null, null) [0];
		}

		public void UpdateContent(FPKey key, bool editmode)
		{
			if (key.PersistSelected && key.CurrentlySelected)
				UpdateKeyLayoutSelected (key);
			else
				UpdateKeyLayout (key);
			

			if (editmode) 
				SetEditModeON (key);
			else 
				SetEditModeOFF (key);

			if (MainTitle.Text.Length > 4)
				MainTitle.Text = MainTitle.Text.Remove (3);
			if (SubTitle.Text.Length > 15)
				SubTitle.Text = SubTitle.Text.Remove (14);

		}

		public void SwitchKeyOnOff(FPKey key)
		{			
			key.CurrentlySelected = !key.CurrentlySelected;

		}

		//View is in Edit mode
		private void SetEditModeON(FPKey key)
		{			
			if (key.MainTitle == "" && key.SubTitle == "" && key.DefaultIcon == string.Empty) { //it's a blank key
				MainTitle.Text = "+";
				SubTitle.Text = "ADD";
			} 
			this.Layer.BorderColor = Util.TB_BLUE_COLOR.CGColor;
		}

		//Standard Key View
		private void SetEditModeOFF(FPKey key)
		{	
			if (key.MainTitle == "" && key.SubTitle == "" && key.DefaultIcon == string.Empty) { //it's a blank key
				MainTitle.Text = "";
				SubTitle.Text = "";
			} 
		}

		public void UpdateKeyLayout(FPKey key)
		{
			if (key.DefaultIcon != null && key.DefaultIcon != string.Empty) { //if there is an icon, we display an icon
				Icon.Image = UIImage.FromFile (ConfigsEngine.GetImageForCurrentConfiguration (key.DefaultIcon));
				Icon.Hidden = false;
				MainTitle.Hidden = true;
			} else {
				MainTitle.Text = key.MainTitle;
				Icon.Hidden = true;
				MainTitle.Hidden = false;
			}

			SubTitle.Text = key.SubTitle;
			SubTitle.TextColor = Util.GetUicolorFromRGBString (key.SubTitleColor);
			MainTitle.TextColor = Util.GetUicolorFromRGBString (key.MainTitleColor);

			this.Layer.BorderColor =   Util.BorderColor;

			if (key.BackgroundColor == null && key.Action == null)
				this.BackgroundColor = Util.CellBackgroundColor;
			else if (key.BackgroundColor == null && key.Action != null)
				this.BackgroundColor = UIColor.White;
			else
				this.BackgroundColor = Util.GetUicolorFromRGBString (key.BackgroundColor);

		}

		public override bool Highlighted {
			get {
				return base.Highlighted;
			}
			set {			

				base.Highlighted = value;
				if (base.Highlighted)
					this.BackgroundColor = UIColor.FromRGB (244, 233, 207);
				
			}
		}	

		public void UpdateKeyLayoutSelected(FPKey key)
		{
			if (key.SelectedIcon != null && key.SelectedIcon != string.Empty ) { //if there is an icon, we display an icon
				Icon.Image = UIImage.FromFile(ConfigsEngine.GetImageForCurrentConfiguration(key.SelectedIcon));
			} else
				MainTitle.Text = key.MainTitle;
			
			SubTitle.Text = key.SubTitle;
			SubTitle.TextColor =  Util.GetUicolorFromRGBString (key.SelectedSubTitleColor);
			MainTitle.TextColor = Util.GetUicolorFromRGBString (key.SelectedMainTitleColor);

			if (key.BackgroundColor == null)
				this.BackgroundColor = UIColor.Cyan;
			else
				this.BackgroundColor = Util.GetUicolorFromRGBString (key.SelectedBackgroundColor);
			
			this.Layer.BorderColor = UIColor.Green.CGColor;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			//Icon.Hidden = true;


			this.Layer.BorderWidth = 1f;
			//this.Layer.CornerRadius = 4;

			this.Layer.ShadowOpacity = 0.4f;
			this.Layer.ShadowRadius = 0.6f;
			this.Layer.ShadowColor = UIColor.Black.CGColor;
			Layer.MasksToBounds = false;

			this.Layer.ShadowOffset =   new CoreGraphics.CGSize (0.7f,0.7f );

			this.Layer.ShadowPath   = UIBezierPath.FromRect (this.Layer.Bounds).CGPath;

			this.Layer.ShouldRasterize = true;
		}
	}
}

