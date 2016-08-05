using System;
using Gtk;

namespace Pokemon.Team.Builder.UI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();

			// This prevents crashes when waiting for operations and controls are pressed
			var init = new Gtk.Init ();

			var window = new MainWindow ();

			Application.Run ();
		}
	}
}
