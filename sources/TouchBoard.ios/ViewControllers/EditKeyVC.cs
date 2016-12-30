
using System;
using System.Drawing;

using Foundation;
using UIKit;
using Dropins.Chooser.iOS;
using AdvancedColorPicker;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SafariServices;

namespace KeyboardCompanion
{
	public partial class EditKeyVC : UIViewController
	{
		enum ColorSelection:int{
			TITLE = 0,
			SUBTITLE,
			BACKGROUND,
			TITLE_SEL,
			SUBTITLE_SEL,
			BACKGROUND_SEL,
		};

		enum IconSelection:int{
			DEFAULT,
			SELECTED,
		};

		public FPKey _loadedKey { get;set;}
		public bool _isOsx {get;set;}
		ColorPickerViewController _colorPicker;
		ColorSelection _colorSelection; //to know where to apply the selected color once the color picker has been dismissed
		UINavigationController _pickerNav;
		UIActivityIndicatorView _activityIndicator;
		List<int> _actionKeys;
		IconSelection _iconSelection; //similar to colorselection, used when the dropbox view controller returns 

		string _loadedDefaultIconName; //used to save the name of the image before saving
		string _loadedSelectedIconName;
		bool _modifierKeyboardActive;

		public delegate void DismissViewcontrollerDelegate(bool save, bool applyStyleToAll);
		public event DismissViewcontrollerDelegate OnDismissViewController;


		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public EditKeyVC ()
			: base (UserInterfaceIdiomIsPhone ? "EditKeyVC_iPhone" : "EditKeyVC_iPad", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.

			_colorPicker = new ColorPickerViewController ();
			_colorPicker.ColorPicked += HandleColorPicked;

			_pickerNav = new UINavigationController(_colorPicker);
			_pickerNav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			_activityIndicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
			_activityIndicator.Color = Util.TB_BLUE_COLOR;
		
			var doneBtn = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			_colorPicker.NavigationItem.RightBarButtonItem = doneBtn;
			doneBtn.Clicked += HandlePickerDismissed;

			MainTitleTbox.ShouldReturn += DismissKeyboardAndSave;
			EdgesForExtendedLayout = UIRectEdge.None;

			ActionTbox.InputAccessoryView = ModifierToolbar;
			ModifierToolbarBtn.Title = "More".Localize ();
			ModifierKeysView.Frame = new CoreGraphics.CGRect (0, 0, Util.ScreenWidth, Util.iPadKeyboardHeight);
			ActionTbox.AutocorrectionType = UITextAutocorrectionType.No;

			var item = ActionTbox.InputAssistantItem;
			item.LeadingBarButtonGroups = null;
			item.TrailingBarButtonGroups = null;

			CancelBarBtn.Clicked += (object sender, EventArgs e) => {
				OnDismissViewController (false, false);
			};

			HelpToolbarBtn.Clicked += (sender, e) => {
				InvokeOnMainThread(() =>
				{
					var controller = new SFSafariViewController(new NSUrl("http://www.timelabs.io/touchboard"));
					this.PresentViewController(controller, true, null);
				});
			};

			UIBarButtonItem saveBarbtn = new UIBarButtonItem("Save".Localize (), UIBarButtonItemStyle.Done,  (s, e) => {
				if (Save())
					OnDismissViewController(true,false);
				else
					new UIAlertView("Error".Localize(),"Please check the action".Localize(),null,"Ok".Localize()).Show();
			});

			UIBarButtonItem saveApplyAllBarButton = new UIBarButtonItem("Apply to All  -".Localize (), UIBarButtonItemStyle.Done,  (s, e) => {
				UIAlertView alert = new UIAlertView("Warning".Localize(),"This will apply the current style to every key".Localize(),null,"Cancel".Localize(),"Apply".Localize());
				alert.Show();
				alert.Clicked += (object s1, UIButtonEventArgs e1) => 
				{
					if (e1.ButtonIndex == 1)
					{
						if (Save())
							OnDismissViewController(true,true);
						else
							new UIAlertView("Error".Localize(),"Please check the action".Localize(),null,"Ok".Localize()).Show();
					}};
			});

			UINavigationItem navItem = new UINavigationItem ("Customize".Localize());
			navItem.RightBarButtonItems = new UIBarButtonItem[]{ saveBarbtn, saveApplyAllBarButton };
			navItem.LeftBarButtonItem = CancelBarBtn;

			navItem.RightBarButtonItem.TintColor = UIColor.White;
			NavBar.Items = new UINavigationItem[] { navItem };
			NavBar.TitleTextAttributes = new UIStringAttributes() {
				ForegroundColor = UIColor.White
			};

			TitleColorBtn.Tag = (int)ColorSelection.TITLE;
			SubtitleColorBtn.Tag = (int)ColorSelection.SUBTITLE;
			BackgroundColorBtn.Tag = (int)ColorSelection.BACKGROUND;
			TitleSelColorBtn.Tag = (int)ColorSelection.TITLE_SEL;
			SubtitleSelColorBtn.Tag = (int)ColorSelection.SUBTITLE_SEL;
			BackroundSelColorBtn.Tag = (int)ColorSelection.BACKGROUND_SEL;

			CustomizeColorButton(TitleColorBtn);
			CustomizeColorButton(SubtitleColorBtn);
			CustomizeColorButton(BackgroundColorBtn);
			CustomizeColorButton(TitleSelColorBtn);
			CustomizeColorButton(SubtitleSelColorBtn);
			CustomizeColorButton(BackroundSelColorBtn);
			CustomizeColorButton(SelectedIconBtn);
			CustomizeColorButton(DefaultIconBtn);
			SelectedIconBtn.BackgroundColor = UIColor.Clear;
			DefaultIconBtn.BackgroundColor = UIColor.Clear;
			CustomizeColorButton(RemoveDefaultIconBtn);
			CustomizeColorButton(RemoveSelectedIconBtn);

			_modifierKeyboardActive = false;
			CustomizeKeyboardButtons ();
			ActionTbox.InputView = ModifierKeysView;
			ActionTbox.InputAssistantItem.LeadingBarButtonGroups = null;
			_actionKeys = new List<int> ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			MainTitleTbox.Text = _loadedKey.MainTitle;
			SubTitleTbox.Text = _loadedKey.SubTitle;
			PersistentSwitch.On = _loadedKey.PersistSelected;
			if (_loadedKey.DefaultIcon != null && _loadedKey.DefaultIcon != string.Empty) {
				if (System.IO.File.Exists(ConfigsEngine.GetImageForCurrentConfiguration(_loadedKey.DefaultIcon))) {


					DefaultIconBtn.SetImage(UIImage.FromFile(ConfigsEngine.GetImageForCurrentConfiguration(_loadedKey.DefaultIcon)), UIControlState.Normal);
					_loadedDefaultIconName = _loadedKey.DefaultIcon;
				}
			}
			else{
				DefaultIconBtn.SetImage (null, UIControlState.Normal);
				_loadedDefaultIconName = string.Empty;
			}
			if (!string.IsNullOrEmpty (_loadedKey.SelectedIcon)) {
				SelectedIconBtn.SetImage (UIImage.FromFile (ConfigsEngine.GetImageForCurrentConfiguration (_loadedKey.SelectedIcon)), UIControlState.Normal);
				_loadedSelectedIconName = _loadedKey.SelectedIcon;
			} else {
				SelectedIconBtn.SetImage (null, UIControlState.Normal);
				_loadedKey.SelectedIcon = string.Empty;
			}

			TitleColorBtn.BackgroundColor = Util.GetUicolorFromRGBString (_loadedKey.MainTitleColor);
			SubtitleColorBtn.BackgroundColor = Util.GetUicolorFromRGBString (_loadedKey.SubTitleColor);
			TitleSelColorBtn.BackgroundColor = Util.GetUicolorFromRGBString (_loadedKey.SelectedMainTitleColor);
			SubtitleSelColorBtn.BackgroundColor = Util.GetUicolorFromRGBString (_loadedKey.SelectedSubTitleColor);
			BackgroundColorBtn.BackgroundColor = Util.GetUicolorFromRGBString (_loadedKey.BackgroundColor);
			BackroundSelColorBtn.BackgroundColor = Util.GetUicolorFromRGBString (_loadedKey.SelectedBackgroundColor);
			ActionTbox.Text = "";
			_actionKeys.Clear ();

			if (_loadedKey.Action != null)
			{
				try
				{
					_actionKeys = JsonConvert.DeserializeObject<List<int>>(_loadedKey.Action);
				}
				catch (Exception e)
				{
					FPLog.Instance.WriteLine("Error deserializing key");
				}
			}

			if (_actionKeys != null && _actionKeys.Count > 0)
				PopulateActionTextBox ();
		

			SetupKeyboard ();
		}


		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			//Save ();

		}

		private void CustomizeColorButton(UIButton button) {
			button.Layer.CornerRadius = 5;
			button.Layer.BorderWidth = 1;
			button.Layer.BorderColor = Util.TB_BLUE_COLOR.CGColor;
		}

		private bool DismissKeyboardAndSave(UITextField textField)
		{
			textField.ResignFirstResponder ();
			return true;
		}

		private void LoadImageFromDropbox()
		{
			DBChooser.DefaultChooser.OpenChooser (DBChooserLinkType.Direct, this, (results) => {

				if (results == null)
				{
					// results is null if the user cancels
					//new UIAlertView ("Cancelled", "User cancelled!", null, "Continue").Show ();
				}
				else {
					// Do something with the results
					if (_iconSelection == IconSelection.DEFAULT)
						_activityIndicator.Frame = DefaultIconBtn.Frame;
					else if (_iconSelection == IconSelection.SELECTED)
						_activityIndicator.Frame = SelectedIconBtn.Frame;

					_activityIndicator.StartAnimating();
					this.View.Add(_activityIndicator);

					new Task (() => { 
						DownloadImageTask(results[0].Link,results[0].Name);
					}).Start();


				} 
			});

		}

		//this is called after the user Select the image from dropbox
		private  void DownloadImageTask(NSUrl imageLink, string imageName)
		{
			try{
				UIImage img = Util.FromNSUrl (imageLink);
				if (img != null) {
					ConfigsEngine.SaveImageForCurrentConfiguration(imageName,img);
					InvokeOnMainThread (() => {
						if (_iconSelection == IconSelection.DEFAULT)
						{
							DefaultIconBtn.SetImage(img,UIControlState.Normal);
							DefaultIconBtn.BackgroundColor= UIColor.Clear;
							_loadedDefaultIconName = imageName;

						}
						else if (_iconSelection == IconSelection.SELECTED)
						{
							SelectedIconBtn.SetImage(img,UIControlState.Normal);
							SelectedIconBtn.BackgroundColor= UIColor.Clear;
							_loadedSelectedIconName = imageName;

						}

						_activityIndicator.StopAnimating();
						_activityIndicator.RemoveFromSuperview();
					});

				}
			}
			catch (Exception) {
				InvokeOnMainThread (() => {
					var alert = new UIAlertView ("Error", "Error downloading image from dropbox", null, "Ok");
					alert.Show();
				});
			}
		}


		private bool Save()
		{
			try{
			_loadedKey.Action = JsonConvert.SerializeObject (_actionKeys);
			}
			catch (Exception ex) {
				return false;
			}

			_loadedKey.MainTitle = MainTitleTbox.Text;
			_loadedKey.SubTitle = SubTitleTbox.Text;
			_loadedKey.DefaultIcon = _loadedDefaultIconName;
			_loadedKey.SelectedIcon = _loadedSelectedIconName;
			_loadedKey.PersistSelected = PersistentSwitch.On;
			_loadedKey.MainTitleColor =  Util.GetColorStringRGB(  TitleColorBtn.BackgroundColor); 
			_loadedKey.SubTitleColor = Util.GetColorStringRGB (SubtitleColorBtn.BackgroundColor);
			_loadedKey.SelectedMainTitleColor = Util.GetColorStringRGB (TitleSelColorBtn.BackgroundColor);
			_loadedKey.SelectedSubTitleColor = Util.GetColorStringRGB (SubtitleSelColorBtn.BackgroundColor);
			_loadedKey.BackgroundColor = Util.GetColorStringRGB (BackgroundColorBtn.BackgroundColor);
			_loadedKey.SelectedBackgroundColor = Util.GetColorStringRGB (BackroundSelColorBtn.BackgroundColor);


			return true;
		
		}

		public void SetKey(FPKey key)
		{
			_loadedKey = key;

		}

		partial void SelectTitleColor (NSObject sender)
		{
			var button = sender as UIButton;
			_colorPicker.Title = "Select Color".Localize();
			_colorPicker.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			_colorSelection = (ColorSelection) ((int)button.Tag);// ColorSelection.TITLE;
			_colorPicker.SelectedColor = button.BackgroundColor;


			this.PresentModalViewController(_pickerNav,true);
		}

		partial void SelectSubtitleColor (NSObject sender)
		{
			/*_colorPicker.Title = "Select Subtitle Color";
			_colorPicker.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			_colorSelection = ColorSelection.SUBTITLE;

			this.PresentModalViewController(_pickerNav,true);*/
		}

		//after returning from the color picker view controller
		void HandleColorPicked ()
		{
			switch (_colorSelection) {
			case ColorSelection.TITLE:
				TitleColorBtn.BackgroundColor = _colorPicker.SelectedColor;
				break;
			case ColorSelection.TITLE_SEL:
				TitleSelColorBtn.BackgroundColor = _colorPicker.SelectedColor;
				break;
			case ColorSelection.SUBTITLE:
				SubtitleColorBtn.BackgroundColor = _colorPicker.SelectedColor;
				break;
			case ColorSelection.SUBTITLE_SEL:
				SubtitleSelColorBtn.BackgroundColor = _colorPicker.SelectedColor;
				break;
			case ColorSelection.BACKGROUND:
				BackgroundColorBtn.BackgroundColor = _colorPicker.SelectedColor;
				break;
			case ColorSelection.BACKGROUND_SEL:
				BackroundSelColorBtn.BackgroundColor = _colorPicker.SelectedColor;
				break;
			}
		}

		//the user dismissed the color picker
		void HandlePickerDismissed (object sender, EventArgs e)
		{
			HandleColorPicked();
			_pickerNav.DismissModalViewController(true);
		}

		partial void SelectDefaultIcon (NSObject sender)
		{
			_iconSelection = IconSelection.DEFAULT;
			LoadImageFromDropbox();
		}

		partial void SelectSelectedIcon (NSObject sender)
		{
			_iconSelection = IconSelection.SELECTED;
			LoadImageFromDropbox();
		}

		partial void RemoveDefaultIcon (NSObject sender)
		{
			_loadedDefaultIconName = null;
			DefaultIconBtn.SetImage(null,UIControlState.Normal);
		}  

		partial void RemoveSelectedIcon (NSObject sender)
		{
			_loadedSelectedIconName = string.Empty;
			SelectedIconBtn.SetImage(null,UIControlState.Normal);
		}


		partial void SwitchKeyboard (NSObject sender)
		{
			_modifierKeyboardActive = !_modifierKeyboardActive;

			if (_modifierKeyboardActive)
				ActionTbox.InputView = SecondModView;
			else
				ActionTbox.InputView = ModifierKeysView;
			ActionTbox.ReloadInputViews();
		}

		private void CustomizeKeyboardButtons()
		{
			foreach (var btn in ModifierKeysView.Subviews) {
				UIButton button = btn as UIButton;
				button.Layer.CornerRadius = 5f;
				button.Layer.BorderColor = Util.TB_BLUE_COLOR.CGColor;
				button.Layer.BorderWidth = 1f;
				button.SetTitleColor (Util.TB_BLUE_COLOR, UIControlState.Highlighted);
				if (button.Title(UIControlState.Normal) != "DELETE") { //FPTEMP this is the delete button to delete the things
			

					button.TouchUpInside += ModifierKeyTouched;
				}
			}

			foreach (var btn in SecondModView.Subviews) {
				UIButton button = btn as UIButton;
				button.Layer.CornerRadius = 5f;
				button.Layer.BorderColor = Util.TB_BLUE_COLOR.CGColor;
				button.Layer.BorderWidth = 1f;
				button.SetTitleColor (Util.TB_BLUE_COLOR, UIControlState.Highlighted);
				if (button.Title(UIControlState.Normal)  != "DELETE") { //FPTEMP this is the delete button to delete the things
		
					button.TouchUpInside += ModifierKeyTouched;
				}
			}

		}
	
		partial void DeleteEntry (NSObject sender)
		{
			if (_actionKeys.Count == 0)
				return;

			_actionKeys.RemoveAt(_actionKeys.Count - 1);
			PopulateActionTextBox();
		}

		/// <summary>
		/// A key on the keyboard has been pressed
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void ModifierKeyTouched (object sender, EventArgs e)
		{
			var btn = sender as UIButton;
			if (_isOsx)
				_actionKeys.Add(MacKeyCodes.OsxKeysDictionary[(VirtualKeyCodeWIN) ((int)btn.Tag)]);
			else
				_actionKeys.Add ((int)btn.Tag);
			PopulateActionTextBox ();

		}

		partial void CloseKeyboard (NSObject sender)
		{
			ActionTbox.ResignFirstResponder();
		}

		partial void HelpRequested (NSObject sender)
		{
			
		}

		private void PopulateActionTextBox()
		{
			string fullstring = string.Empty;

			foreach (var key in _actionKeys) {
				if (_isOsx) {
					VirtualKeyCodeOSX kvc = (VirtualKeyCodeOSX)key;
					fullstring = fullstring + kvc.ToString () + "+";
				} else {
					VirtualKeyCodeWIN kvc = (VirtualKeyCodeWIN)key;
					fullstring = fullstring + kvc.ToString () + "+";
				}
			}

			if (fullstring.Length > 1 && fullstring[fullstring.Length-1] == '+') 
				fullstring = fullstring.Remove (fullstring.Length - 1);

			ActionTbox.Text = fullstring;
		}

		//setting up the WIN / CMD CHANGE
		private void SetupKeyboard()
		{
			foreach (var view in ModifierKeysView) {

				var btn = view as UIButton;

				if (btn.Tag == (int)VirtualKeyCodeWIN.LWIN) {
					if (_isOsx)
						btn.SetTitle ("CMD", UIControlState.Normal);
					else
						btn.SetTitle ("LWIN", UIControlState.Normal);
				}
				if (btn.Tag == (int)VirtualKeyCodeWIN.RWIN){
					if (_isOsx)
						btn.SetTitle ("CMD", UIControlState.Normal);
					else
						btn.SetTitle ("RWIN", UIControlState.Normal);
				}
			
			}

		}
	}
}

