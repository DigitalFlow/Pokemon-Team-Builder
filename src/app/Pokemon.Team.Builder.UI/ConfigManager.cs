using System;
using System.Reflection;
using System.Configuration;

namespace Pokemon.Team.Builder.UI
{
	public static class ConfigManager
	{
		public static void WriteSetting(string key, string value) 
		{
			var config = ConfigurationManager.OpenExeConfiguration (Assembly.GetExecutingAssembly ().Location);

			// Remove first if already set
			config.AppSettings.Settings.Remove (key);

			config.AppSettings.Settings.Add (key, value);
			config.Save (ConfigurationSaveMode.Minimal);
		}

		public static string GetSetting(string key) {
			var config = ConfigurationManager.OpenExeConfiguration (Assembly.GetExecutingAssembly ().Location);

			return config.AppSettings.Settings [key].Value;
		}
	}
}

