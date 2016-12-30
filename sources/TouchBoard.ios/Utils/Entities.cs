using System;
using SQLite;
using System.Collections.Generic;

namespace KeyboardCompanion
{
	public class FPKey
	{
		public string MainTitle { get; set; }
		public string SubTitle { get; set; }
		public string DefaultIcon { get; set; } //the location
		public string SelectedIcon { get; set; } //the location
		public string Action{ get; set; } //the string to send to the server
		public string MainTitleColor { get; set; } 
		public string SelectedMainTitleColor { get; set; } 
		public string SubTitleColor { get; set; } 
		public string SelectedSubTitleColor { get; set; } 
		public string BackgroundColor { get; set; } 
		public string SelectedBackgroundColor { get; set; } 
		public bool PersistSelected{ get; set; } //if true, the key act like a ON-OFF switch
		public bool CurrentlySelected{ get; set; } //used locally to select/deselect a key

		public FPKey()
		{
			MainTitle = "";
			SubTitle = "";
			DefaultIcon = "";
		}
	}

	public class KeyConfiguration
	{
		public string Name { get; set; }
		public List<FPKey> Keys { get; set; }
		public bool OSx{ get; set; }

		public KeyConfiguration(string name, bool osx)
		{
			Name = name;
			Keys = new List<FPKey> ();
			OSx = osx;
		}
	}

	//this is only used to query the db to get a list of all the configurations and their path without having to 
	// go thru the file system and check all the folders
	public class DBKeyConfiguration
	{
		[PrimaryKey]
		public string Name { get; set; }
		public string Path { get; set; }

	}

	public class NinjaServer
	{
		public string IpAddress { get; set;}
		public bool OsX { get; set; }
	}

	//the object retrieved from the server which contains a list of this configs
	public class RepoConfig
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public string PreviewImg { get; set; }

	}
}

