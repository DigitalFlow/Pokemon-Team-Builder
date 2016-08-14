using System;
using System.Linq;
using Gtk;
using System.Collections;
using System.Collections.Generic;

namespace Pokemon.Team.Builder.UI
{
	public static class UiExtensions
	{
		public static Image SetPicture(this Image image, Pokemon pokemon, int width = 96, int height = 96)
		{
			if (pokemon == null) {
				return image;
			}

			var pixBuf = new Gdk.Pixbuf(Convert.FromBase64String(pokemon.Image));
			image.Pixbuf = pixBuf.ScaleSimple (width, height, Gdk.InterpType.Nearest);

			return image;
		}

		public static void AddItems<T>(this Grid grid, List<T> enumerable, List<Func<T, Widget>> selectorFuncs)
		{
			for (var i = 0; i < enumerable.Count(); i++) {
				var item = enumerable [i];

				var j = 0;

				foreach (var selector in selectorFuncs) {
					var widget = selector (item);

					var label = widget as Label;

					// Fix columns without name that somehow appear
					if (label != null && string.IsNullOrEmpty (label.Text)) {
						break;
					}

					grid.Attach(widget, ++j, i, 1, 1);	
				}
			}
		}
	}
}

