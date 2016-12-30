using System;
using Foundation;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace KeyboardCompanion
{
	/*
	Configurations are a simple json file with all the settings and a subfolder where the images will be loaded from
	*/
	public class ConfigsEngine
	{

		private const string CONFIG_FILE_NAME = "keyconfig.ninja";
		private const int MAX_CONFIG_KEYS = 20;

		public static string ConfigFolder = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path + "/";

		public ConfigsEngine ()
		{
		}

		/// <summary>
		/// Create a new directory where the json and the images will be stored
		/// </summary>
		/// <returns>The configuration path, empty if it already exists</returns>
		/// <param name="name">the name of the configuration.</param>
		/// <param name="confToCopy">the configuration from where to copy the keys</param>
		public static KeyConfiguration CreateNewConfiguration(string name, KeyConfiguration confToCopy)
		{
			FPLog.Instance.WriteLine ("Creating New Configuration = {0}", FPLog.LoggerLevel.LOG_INFORMATION, name);

			string path = ConfigFolder + name;
			if (Directory.Exists (path))
				return null;
			else {
				Directory.CreateDirectory (path);
				Directory.CreateDirectory (path + "/images"); 
			}

			KeyConfiguration newConfig = new KeyConfiguration (name, Util.IsOSxServer);

			if (confToCopy == null) {
				for (int i = 0; i < MAX_CONFIG_KEYS; i++)
					newConfig.Keys.Add (new FPKey ());
			} else {
				foreach (var key in confToCopy.Keys)
					newConfig.Keys.Add (key);
			}

			if (SaveConfiguration (newConfig)) {
				DBManager.Instance.InsertConfiguration (name, path);
				return newConfig;
			} else
				return null;
		}

		public static void DeleteConfiguration(string name)
		{
			string path = ConfigFolder + name;
			if (Directory.Exists (path))
				Directory.Delete (path,true);

			DBManager.Instance.DeleteConfiguration (name);
		}

		public static bool SaveConfiguration(KeyConfiguration configuration)
		{
			ClearImages (configuration);

			string path = ConfigFolder + configuration.Name;

			if (!Directory.Exists (path)) {
				Directory.CreateDirectory (path);
				Directory.CreateDirectory (path + "/images"); 
			} 

			string configFilePath = path + "/" + CONFIG_FILE_NAME;
			var jsonFile = new StringBuilder ();

			jsonFile.Append (JsonConvert.SerializeObject (configuration));
			File.WriteAllText (configFilePath, jsonFile.ToString ());

			FPLog.Instance.WriteLine ("Saved Configuration {0}", FPLog.LoggerLevel.LOG_INFORMATION, configFilePath);

			return true;
		}


		/// <summary>
		/// Given a configurations, it checks what images are used and deletes the unused ones
		/// </summary>
		/// <param name="configuration">Configuration.</param>
		private static void ClearImages(KeyConfiguration configuration)
		{
			FPLog.Instance.WriteLine ("Clearing Images", FPLog.LoggerLevel.LOG_INFORMATION);
			string path = ConfigFolder + configuration.Name + "/images/";

			if (!Directory.Exists (path))
				return;

			var images = Directory.GetFiles (path);

			foreach (var image in images) {
				bool used = false;
				foreach (var key in configuration.Keys) {
					if (key.DefaultIcon == Path.GetFileName(image) || key.SelectedIcon == Path.GetFileName(image)) {
						used = true;
						break;
					}
				}
				if (!used) {
					FPLog.Instance.WriteLine ("Deleting image file: {0}", FPLog.LoggerLevel.LOG_INFORMATION, image);
					File.Delete (image);
				}
			}

		}

		public static KeyConfiguration LoadConfiguration(string name)
		{
			string path = ConfigFolder + name + "/" + CONFIG_FILE_NAME;

			FPLog.Instance.WriteLine ("Loading Configuration: {0} from: {1}", FPLog.LoggerLevel.LOG_INFORMATION, name, path);
		//	var files = Directory.GetFiles (ConfigFolder + name);
			if (File.Exists (path)) {
				var config = JsonConvert.DeserializeObject<KeyConfiguration> (File.ReadAllText (path));
				foreach (var key in config.Keys)
					key.CurrentlySelected = false;
				return config;
			} else
				return null;
		}


		public static async Task<KeyConfiguration> LoadDefaultConfiguration(bool osx)
		{
			if (osx)
			{
				await ExtractFile("Configurations/Xcode.zip");
				var config = LoadConfiguration("Xcode");
				return config;
			}
			else {
				var def = CreateNewConfiguration("Default", null);
				def.OSx = osx;

				string action = osx ? VirtualKeyCodeOSX.KEY_A.ToString("D") : VirtualKeyCodeWIN.KEY_A.ToString("D");
				action = "[" + action + "]";

				FPKey keyA = new FPKey
				{
					Action = action,
					MainTitle = "A",
					SubTitle = "SEND A",
					DefaultIcon = "",
					BackgroundColor = Util.GetColorStringRGB(Util.CellBackgroundColor),
					SelectedBackgroundColor = Util.GetColorStringRGB(UIColor.FromRGB(244, 233, 207))
				};
				def.Keys[0] = (keyA);

				action = osx ? VirtualKeyCodeOSX.KEY_B.ToString("D") : VirtualKeyCodeWIN.KEY_B.ToString("D");
				action = "[" + action + "]";

				FPKey keyB = new FPKey
				{
					Action = action,
					MainTitle = "B",
					SubTitle = "SEND B",
					DefaultIcon = "",
					BackgroundColor = Util.GetColorStringRGB(Util.CellBackgroundColor),
					SelectedBackgroundColor = Util.GetColorStringRGB(UIColor.FromRGB(244, 233, 207))
				};
				def.Keys[1] = (keyB);

				action = osx ? VirtualKeyCodeOSX.RETURN.ToString("D") : VirtualKeyCodeWIN.RETURN.ToString("D");
				action = "[" + action + "]";

				FPKey keyENTER = new FPKey
				{
					Action = action,
					MainTitle = "ENT",
					SubTitle = "ENTER",
					DefaultIcon = "",
					BackgroundColor = Util.GetColorStringRGB(Util.CellBackgroundColor),
					SelectedBackgroundColor = Util.GetColorStringRGB(UIColor.FromRGB(244, 233, 207))
				};
				def.Keys[2] = (keyENTER);

				action = osx ? VirtualKeyCodeOSX.KEY_1.ToString("D") + VirtualKeyCodeOSX.KEY_2.ToString("D") : VirtualKeyCodeWIN.KEY_1.ToString("D") + VirtualKeyCodeWIN.KEY_2.ToString("D");
				action = "[" + action + "]";

				FPKey keyE = new FPKey
				{
					Action = "action",
					MainTitle = "12",
					SubTitle = "MULTIPLE",
					DefaultIcon = "",
					BackgroundColor = Util.GetColorStringRGB(Util.CellBackgroundColor),
					SelectedBackgroundColor = Util.GetColorStringRGB(UIColor.FromRGB(244, 233, 207))
				};
				def.Keys[3] = (keyE);

				if (def.Keys.Count < MAX_CONFIG_KEYS)
				{
					for (int i = def.Keys.Count; i < MAX_CONFIG_KEYS; i++)
						def.Keys.Add(new FPKey());
				}

				var x = SaveConfiguration(def);

				return def;
			}
		}

		/*public static KeyConfiguration CreateNewConfiguration(string configurationName)
		{
			

			var  conf = NewConfiguration (configurationName);
			for (int i = 0; i < MAX_CONFIG_KEYS; i++)
				conf.Keys.Add (new FPKey ());
			if (SaveConfiguration (conf))
				return conf;
			else
				return null;
		}*/

		public async static Task<List<string>> GetListOfConfigurations()
		{

			var confs = DBManager.Instance.GetConfigurations ();
			var strc = confs.Select (cf => cf.Name).ToList ();

			/*var configs = Directory.GetDirectories (ConfigFolder);
			List<string> configsNames = new List<string> ();
			foreach (var config in configs)
				configsNames.Add( config.Replace (ConfigFolder, ""));*/
				
			await Task.Delay(50); //temp trick
			return strc;
		}

		/// <summary>
		/// Given an image, save it in the current configuration folder
		/// </summary>
		/// <param name="imageName">Image name.</param>
		/// <param name="img">Image object.</param>
		public static void SaveImageForCurrentConfiguration(string imageName, UIImage img)
		{
			if (!Directory.Exists(ConfigFolder + Util.GetLastConfigurationUsed() + "/images"))
			{
				Directory.CreateDirectory(ConfigFolder + Util.GetLastConfigurationUsed() + "/images");
			}

			string path = ConfigFolder + Util.GetLastConfigurationUsed () + "/images/" + imageName;

			var pngImg = img.AsPNG ();
			var saved = pngImg.Save (path, true);

			FPLog.Instance.WriteLine ("Saved image: {0}", FPLog.LoggerLevel.LOG_INFORMATION, path);
		}

		/// <summary>
		/// Given the name, returns the path of the image for the loaded configuration currently in use
		/// </summary>
		/// <returns>The full path of the image</returns>
		/// <param name="imageName">Image name.</param>
		public static string GetImageForCurrentConfiguration(string imageName)
		{
			string path = ConfigFolder + Util.GetLastConfigurationUsed () + "/images/" + imageName;
			return path;
		}

		/// <summary>
		/// Create a zip file ready to be exported
		/// </summary>
		/// <returns>The path of the file</returns>
		/// <param name="config">the configuration to be exported </param>
		public async static Task<string> CreateFileForExport(KeyConfiguration config)
		{
			string outputPath = Util.CacheDirectory + "/" + config.Name + ".zip";
			FPLog.Instance.WriteLine ("Creating Zip File: {0}", FPLog.LoggerLevel.LOG_INFORMATION, outputPath);

			await Task.Run(() => {
				try{
					var inputPath = ConfigFolder + config.Name ;		

					if (File.Exists (outputPath))
						File.Delete (outputPath);

					Util.ZipFolder (outputPath, inputPath);
				}
				catch (Exception ex)
				{
					FPLog.Instance.WriteLine ("Something went wrong in CreateFileForExport: {0}", FPLog.LoggerLevel.LOG_ERROR, ex.Message);
					outputPath = string.Empty;
				}

			});

			return outputPath;
		}

		/// <summary>
		/// Given a url of a zip file, it downloads it, unzip it and move it to the configurations folder and add it to the DB
		/// </summary>
		/// <returns>The number of configurations imported.</returns>
		/// <param name="filename">File path of the zip file downloaded </param>
		public async static Task<string> ImportFile(string url, string filename)
		{
			string filePath = await Util.DownloadFile(url, filename);
			if (filePath == null)
				return "Error Downloading File".Localize();

			var output = await ExtractFile(filePath);
			return output;
		}

		public async static Task<string> ExtractFile(string filePath)
		{
			string output = string.Empty;

			if (File.Exists(filePath))
			{
				Console.WriteLine("SAD");
			}

			await Task.Run(() => {

				FPLog.Instance.WriteLine ("Importing Zip File: {0}", FPLog.LoggerLevel.LOG_INFORMATION, filePath);
				string unzipPath = Util.CacheDirectory + "/" + Path.GetFileNameWithoutExtension(filePath);
				if (Util.UnzipFile(filePath,unzipPath))
				{
					var dirs = Directory.GetDirectories(unzipPath);
					if (dirs.Length != 0)
					{
						List<string> directoriesWithWorkingConfiguration = new List<string>();

						foreach (var dir in dirs)
						{
							var files = Directory.GetFiles(dir,CONFIG_FILE_NAME);
							if (files.Length > 0)
							{
								try{
									var config = JsonConvert.DeserializeObject<KeyConfiguration> (File.ReadAllText (files[0]));
									if (config != null){
									string newdir = ConfigFolder + config.Name;
									if (!Directory.Exists(newdir))
										Directory.Move(dir,newdir);
										DBManager.Instance.InsertConfiguration(config.Name,ConfigFolder + Path.GetDirectoryName(dir));
										directoriesWithWorkingConfiguration.Add(dir);
									}
								}
								catch (Exception ex) //domething wrong with the configuration probably
								{
									FPLog.Instance.WriteLine ("Error in importing conf: {0}", FPLog.LoggerLevel.LOG_ERROR, ex.Message);
									continue;
								}
							}
						}
						if (directoriesWithWorkingConfiguration.Count == 0)
							output = "No Configurations Found In Zip File".Localize();
						else
							output = "Successfully Imported ".Localize() + directoriesWithWorkingConfiguration.Count + " configurations".Localize();


					}
					else
						output = "Unable to read the file".Localize();
				}
				else
					output = "Error unzipping the file".Localize();
			});

			return output;
		}

		const string REPOSITORY_ADDRESS = "http://www.timealbs.io/touchboard/repo/";
		const string REPOSITORY_PKG_LIST = REPOSITORY_ADDRESS + "nkconfigs.ninja";
		public static async Task<List<RepoConfig>> GetServerConfigurations()
		{
			List<RepoConfig> repo = null;

			var url = new System.Uri(REPOSITORY_PKG_LIST);
			FPWebClient webclient = new FPWebClient ();
			webclient.Encoding =  System.Text.Encoding.UTF8;
		
			await Task.Run (() => {
				try{
					var configslist = webclient.DownloadString (url);
					repo = JsonConvert.DeserializeObject<List<RepoConfig>> (configslist);
				}
				catch (Exception ex)
				{
					FPLog.Instance.WriteLine ("Error while downloading repo configurations: {0}", FPLog.LoggerLevel.LOG_ERROR, ex.Message);
				}

			});

			return repo;
		}

	}

	class FPWebClient : WebClient
	{
		protected override WebRequest GetWebRequest(System.Uri uri)
		{
			WebRequest w = base.GetWebRequest(uri);
			w.Timeout = 15 * 1000; 
			return w;
		}
	}
}

