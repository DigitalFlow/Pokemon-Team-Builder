using System;
using Gtk;

namespace Pokemon.Team.Builder.UI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();

			using (var mainWindow = new MainWindow ()) {
				Application.Run ();
			}
		}
	}
}
