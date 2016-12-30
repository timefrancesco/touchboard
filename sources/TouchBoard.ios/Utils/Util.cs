using System;
using UIKit;
using Foundation;
using System.IO;
using System.Linq;
using MiniZip.ZipArchive;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KeyboardCompanion
{
	public static class Util
	{
		public static UIColor TB_BLUE_COLOR = UIColor.FromRGB(0, 117, 213);
		public static UIColor TB_LIGHT_GRAY_COLOR = UIColor.FromRGB(234, 234, 234);
		public static UIColor TB_LIGHT_GRAY_IDLE_COLOR = UIColor.FromRGB(238, 238, 238);

		public static UIFont SettingsFont = UIFont.FromName("HelveticaNeue-Light", 19f);

		//colors
		/*public static UIColor BackgroundColorDay = UIColor.White;
		public static UIColor BackgroundColorNight = UIColor.Black;
		public static UIColor ForegroundColorDay = UIColor.Black;
		public static UIColor ForegroundColorNight = UIColor.Orange;
		public static CoreGraphics.CGColor BorderColorDay =  UIColor.LightGray.CGColor;
		public static CoreGraphics.CGColor BorderColorNight =  UIColor.Orange.CGColor;*/

		public static UIColor BackgroundColor = UIColor.White;
		public static UIColor CellBackgroundColor = UIColor.FromRGB(244,244,244);
		public static UIColor ForegroundColor = UIColor.Black;
		public static CoreGraphics.CGColor BorderColor =  UIColor.FromRGB(234, 234, 234).CGColor;

		public static nfloat ScreenHeight = UIScreen.MainScreen.Bounds.Height;
		public static nfloat ScreenWidth = UIScreen.MainScreen.Bounds.Width;

		public static nfloat iPadKeyboardHeight = 373; //the modifier keyboard

		public delegate void EnableNightModeDelegate(bool nightModeEnabled);
		public static event EnableNightModeDelegate OnEnableNightMode;

		public static string CacheDirectory =  NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User) [0].Path;

		public static bool IsOSxServer { get; set; }

		public static bool IOS8()
		{
			int SystemVersion = Convert.ToInt16 (UIDevice.CurrentDevice.SystemVersion.Split ('.') [0]);
			if (SystemVersion >= 8)
				return true;
			else
				return false;
		
		}

		public static bool USING_IPHONE = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;

		public static void EnableDisableNightMode()
		{
			var nightmode = NSUserDefaults.StandardUserDefaults.BoolForKey ("nightMode");
			nightmode = !nightmode;
			if (OnEnableNightMode != null) {
				if (nightmode) {
					BackgroundColor = UIColor.Black;
					CellBackgroundColor = UIColor.Black;
					ForegroundColor = UIColor.Green;
					BorderColor = UIColor.Green.CGColor;
				} else {
					BackgroundColor = UIColor.White;
					CellBackgroundColor = UIColor.FromRGB (238, 238, 238);
					ForegroundColor = UIColor.Black;
					BorderColor = UIColor.LightGray.CGColor;
				}

				OnEnableNightMode (nightmode);
				NSUserDefaults.StandardUserDefaults.SetBool (nightmode, "nightMode");
			}
		}

		public static void SetColorsOnStartup()
		{
			var nightmode = NSUserDefaults.StandardUserDefaults.BoolForKey ("nightMode");
			if (nightmode) {
				BackgroundColor = UIColor.Black;
				CellBackgroundColor = UIColor.Black;
				ForegroundColor = UIColor.Green;
				CoreGraphics.CGColor BorderColor = UIColor.Green.CGColor;
			} else {
				BackgroundColor = UIColor.White;
				CellBackgroundColor = UIColor.FromRGB (238, 238, 238);
				ForegroundColor = UIColor.Black;
				CoreGraphics.CGColor BorderColor = UIColor.LightGray.CGColor;
			}
		}


		/// <summary>
		/// Downloads a file from internet
		/// </summary>
		/// <returns>The path of the downloaded file</returns>
		/// <param name="url">the requested url of the file</param>
		/// <param name="filename">the file name.</param>
		public async static Task<string> DownloadFile(string url, string localfilename)
		{
			FPLog.Instance.WriteLine ("Downloading file: {0} from: {1}", FPLog.LoggerLevel.LOG_INFORMATION, localfilename, url);

			string outputPath = Util.CacheDirectory + "/" + localfilename;

			if (File.Exists (outputPath))
				File.Delete (outputPath);

			var webClient = new WebClient();
			await Task.Run (() => {
				try{
				webClient.DownloadFile (url, outputPath);
				}
				catch (Exception ex)
				{
					FPLog.Instance.WriteLine ("Something went wrong during the download: {0}", FPLog.LoggerLevel.LOG_ERROR, ex.Message);
					outputPath = null;
				}
			});

			return outputPath;
		}



		public static void SaveServerIP(string ipaddress, bool osx)
		{
			string val = null;

			if (osx)
				val = ipaddress + ":osx";
			else 
				val = ipaddress + ":win"; 

			NSUserDefaults.StandardUserDefaults.SetString (val, "ipaddress");
		}

		public static string GetServerIP()
		{
			string val = NSUserDefaults.StandardUserDefaults.StringForKey ("ipaddress");
			if (val == null)
				return null;

			string ip = val.Remove (val.IndexOf (':'));
			string kind = val.Remove (0, val.IndexOf (':')+1);
			if (kind.Equals ("osx"))
				IsOSxServer = true;
			else
				IsOSxServer = false;
			return ip;
		}

		public static void SetLastConfigurationUsed(string name)
		{
			NSUserDefaults.StandardUserDefaults.SetString (name, "LastUsedConf");
		}

		public static string GetLastConfigurationUsed()
		{
			return NSUserDefaults.StandardUserDefaults.StringForKey ("LastUsedConf");
		}

		public static UIImage FromNSUrl (NSUrl uri)
		{
			try{
				using (var url = uri)
				using (var data = NSData.FromUrl (url))
					return UIImage.LoadFromData (data);
			}
			catch {
				return null;
			}
		}

		public static string GetColorStringRGB(UIColor orig)
		{
			string color;
			nfloat red, blue, green,alpha;
			orig.GetRGBA (out red, out green, out blue, out alpha);

			color = string.Format ("{0},{1},{2},{3}", red, green, blue,alpha);
			return color;
		}

		public static UIColor GetUicolorFromRGBString(string rgb)
		{
			try{
			
			var val = rgb.Split (',');
			UIColor color = UIColor.FromRGBA ((nfloat)float.Parse (val [0]), (nfloat)float.Parse (val [1]), (nfloat)float.Parse (val [2]), (nfloat)float.Parse (val [3]));
			return color;
			}
			catch (Exception) {
				return UIColor.Black;
			}
		}

		public static void ZipFolder(string outputfilename, string folderName) 
		{
			var zip = new ZipArchive ();
			zip.CreateZipFile (outputfilename);
			zip.AddFolder (folderName, "");
			zip.CloseZipFile ();
		}

		public static bool UnzipFile(string zipFilePath, string outputFolder)
		{
			try{
				var zip = new ZipArchive ();
				zip.UnzipOpenFile(zipFilePath);
				zip.UnzipFileTo (outputFolder, true);
				zip.UnzipCloseFile ();
			}catch (Exception ex) {
				FPLog.Instance.WriteLine ("Something went wrong when Unzipping a file: {0}", FPLog.LoggerLevel.LOG_ERROR, ex.Message);
				return false;
			}
			return true;

		}
	}

	/// <summary>
	/// Extension Class to handle Localization
	/// </summary>
	public static class LocalizationExtensions
	{
		private static readonly string[] supportedLanguages = { "it", "en", "fr", "de", "es", "ja" };

		public static string Localize(this string key)
		{
			string localizedString = NSBundle.MainBundle.LocalizedString(key, null);

			if (localizedString.Equals(key) && !supportedLanguages.Any(l => l.Equals(NSLocale.PreferredLanguages [0])))
				localizedString = FallbackBundle.LocalizedString(key, null);

			return localizedString;
		}

		private static NSBundle fallbackBundle;

		private static NSBundle FallbackBundle
		{
			get
			{
				if (fallbackBundle == null)
					fallbackBundle = NSBundle.FromPath(NSBundle.MainBundle.PathForResource("en", "lproj"));

				return fallbackBundle;
			}
		}
	}
}

