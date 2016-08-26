using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.UI
{
    public class ShowdownWindow : Window
    {
        public ShowdownWindow(string team) : base(WindowType.Toplevel)
        {
            Initialize (team);
        }

        public void Initialize (string team)
        {
            SetSizeRequest(400, 600);

            var scrolledWindow = new ScrolledWindow();

            var boxLayout = new Box(Orientation.Vertical, 0);

            var textBuffer = new TextBuffer(new TextTagTable());

            textBuffer.Text = team;

            var text = new TextView(textBuffer)
            {
                Editable = false,
                Expand = true,
                Hexpand = true,
                Vexpand = true,
                VscrollPolicy = ScrollablePolicy.Natural
            };

            boxLayout.Add(text);
            scrolledWindow.Add(boxLayout);

            Add(scrolledWindow);

            ShowAll();
        }
    }
}
