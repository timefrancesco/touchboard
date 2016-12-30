using System;
using System.Configuration;
using System.Collections.Generic;


using System.IO;
using Foundation;

namespace KeyboardCompanion
{
	public class FPLog
	{
		public enum LoggerLevel
		{
			LOG_NONE = -1,
			LOG_ERROR = 0,
			LOG_WARNING = 1,
			LOG_INFORMATION = 2,
		}

		public static string LogFile = Util.IOS8() ? NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User) [0].Path + "/fplogs":
			Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "/fplogs";

		public static string LogPath = Util.IOS8() ? NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User) [0].Path :
			Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) ;

		private static FPLog _instance;
		public string m_strPath = "";
		private string _filePath = "";
		private LoggerLevel _consoleLevel = 0;
		private LoggerLevel _fileLevel = 0;
		private LoggerLevel _defaultLevel = 0;
		private object m_csWrite = new object();

		private FPLog (string strPath, string strNomeFile)
		{
			m_strPath = strPath;
			_filePath = strNomeFile;

		}

		public static FPLog Instance
		{

			get
			{
				if (_instance == null)
					_instance = new FPLog(LogPath,LogFile );

				return _instance;
			}
		}

		public void WriteLine(string text,params object[] args)
		{
			Write(string.Format(text, args), _defaultLevel);
		}

		public void WriteLine(string text,FPLog.LoggerLevel level, params object[] args )
		{
			Write(string.Format(text, args), level);
		}

		public void WriteLine(string text)
		{
			Write(text,_defaultLevel);
		}

		public void WriteLine (string strMsgLog, FPLog.LoggerLevel level)
		{
			Write(strMsgLog,level);
		}

		private void Write(string strMsgLog, FPLog.LoggerLevel level)
		{
			if (level <= _consoleLevel)
				Console.WriteLine(strMsgLog);

			if (level <= _fileLevel)
			{
				lock (m_csWrite)
				{
					//ThreadPool.QueueUserWorkItem(delegate {
					try
					{
						StreamWriter sw = new StreamWriter(_filePath, true);
						string strLogFormat = DateTime.Now.ToString("d-MM-yy HH:mm:ss:fff") + " > ";
						sw.WriteLine(strLogFormat + strMsgLog);
						sw.Flush();
						sw.Close();
					}
					catch (Exception ex)
					{
						string msg = ex.Message;
					}
					//});
				}
			}
		}

		public string GetLogDirPath()
		{
			return m_strPath;
		}

		public void ChangeLogLevel(FPLog.LoggerLevel consoleLevel, FPLog.LoggerLevel fileLevel, FPLog.LoggerLevel defaultLevel)
		{
			_consoleLevel = consoleLevel;
			_fileLevel = fileLevel;
			_defaultLevel = defaultLevel;
		}

		/// <summary>
		/// Inits the log.
		/// </summary>
		/// <param name="consoleLevel">The level of the logs written to the console</param>
		/// <param name="fileLevel">The level of the logs written to the file</param>
		/// <param name="defaultLevel">The level of the logs written when no level is specified</param>
		/// <param name="append">If set to <c>true</c> it append the logs to the file.</param>
		public void InitLog (FPLog.LoggerLevel consoleLevel, FPLog.LoggerLevel fileLevel, FPLog.LoggerLevel defaultLevel, bool append)
		{
			_consoleLevel = consoleLevel;
			_fileLevel = fileLevel;
			_defaultLevel = defaultLevel;
			if (!append) {
				if (File.Exists(_filePath))
					File.Delete(_filePath);
			}
		}

		public string ReadLogFile ()
		{
			string log = string.Empty;
			if (File.Exists (_filePath)) {
				log = File.ReadAllText(_filePath);
			}
			return log;
		}
	}

}

