using System;
using SQLite;
using Foundation;
using System.Collections.Generic;
using System.Linq;

namespace KeyboardCompanion
{
	public class DBManager
	{
		public static string DBpath = Util.IOS8() ? NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User) [0].Path + "/kdb.db":
			Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/kdb.db";


		private static DBManager _instance =null;
		private static SQLiteConnection _connection = null;

		public static DBManager Instance
		{
			get{
				if(_instance==null)
				{
					_instance=new DBManager();
				}
				return _instance;
			}
		}

		private DBManager ()
		{
			if (_connection != null)
				_connection.Close ();

			_connection = new SQLiteConnection (DBpath);
		}

		public void CreateTables()
		{
			_connection.CreateTable<DBKeyConfiguration> ();
		}

		public void InsertConfiguration(string name, string path)
		{
			DBKeyConfiguration val = new DBKeyConfiguration{ Name = name, Path = path };
			_connection.InsertOrReplace (val);
		}

		public List<DBKeyConfiguration> GetConfigurations()
		{
			return _connection.Table<DBKeyConfiguration> ().ToList ();
		}

		public string GetConfigurationPathFromName(string name)
		{
			var val = _connection.Table<DBKeyConfiguration> ().Where (x => x.Name == name).FirstOrDefault ();
			return val.Path;
		}

		public string GetConfigurationNameFromPath(string path)
		{
			var val = _connection.Table<DBKeyConfiguration> ().Where (x => x.Path == path).FirstOrDefault ();
			return val.Name;
		}

		public void DeleteConfiguration(string name)
		{
			var conf = new DBKeyConfiguration (){ Name = name, Path = GetConfigurationPathFromName (name) };

			_connection.Delete (conf);//<DBKeyConfiguration> ("name");
		}
	}
}

