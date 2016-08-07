using System;
using Gtk;

namespace Pokemon.Team.Builder.UI
{
	public static class UiExtensions
	{
		public static void SetPicture(this Image image, Pokemon pokemon, int width = 96, int height = 96)
		{
			var pixBuf = new Gdk.Pixbuf(Convert.FromBase64String(pokemon.Image));
			image.Pixbuf = pixBuf.ScaleSimple (width, height, Gdk.InterpType.Nearest);
		}
	}
}

