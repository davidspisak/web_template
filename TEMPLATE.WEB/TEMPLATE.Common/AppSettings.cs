using System;

namespace TEMPLATE.Common
{
	public class AppSettings
	{
		public static AppSettings Instance;
		public string ApplicationVersion { get; set; }
		public AppLogger AppLogger { get; set; }
		public DbOptions DbOptions { get; set; }

		public AppSettings()
		{
			DbOptions = new DbOptions();
			AppLogger = new AppLogger();
		}
	}

	public class AppLogger
	{
		public string LogPath { get; set; }
		public bool EnableLogToFile { get; set; } = true;
		public bool EnableLogToDb { get; set; } = true;
		public bool PostgreSqlProvider { get; set; } = false;
		public bool LogRequest { get; set; } = false;
	}

	public class DbOptions
	{
		public string ConnectionString { get; set; } = "Host=localhost;Database=template_web;Username=templateuser;Password=templatepwd";
		public string Key { get; set; } = "PGPASSWORD";
	}
}
