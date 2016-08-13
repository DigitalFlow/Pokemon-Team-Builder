using System;
using Gtk;
using NLog;

namespace Pokemon.Team.Builder.UI
{
	class MainClass
	{
        private static Logger _logger = LogManager.GetCurrentClassLogger();

		public static void Main (string[] args)
		{
            _logger.Info("Starting");

            GLib.ExceptionManager.UnhandledException += HandleException;

			Application.Init ();

			using (var mainWindow = new MainWindow ()) {
				Application.Run ();
			}
		}

        private static void HandleException(GLib.UnhandledExceptionArgs args)
        {
            _logger.Error(((Exception)args.ExceptionObject).Message);
        }
    }
}
