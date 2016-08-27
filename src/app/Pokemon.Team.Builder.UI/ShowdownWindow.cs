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
        private TextBuffer _buffer;
        private TextView _view;

        public ShowdownWindow(string team) : base(WindowType.Toplevel)
        {
            Initialize (team);
        }

        public void Initialize (string team)
        {
            SetSizeRequest(400, 600);

            var scrolledWindow = new ScrolledWindow();

            var boxLayout = new Box(Orientation.Vertical, 0);

            _buffer = new TextBuffer(new TextTagTable());

            _buffer.Text = team;

            _view = new TextView(_buffer)
            {
                Editable = false,
                Expand = true,
                Hexpand = true,
                Vexpand = true,
                VscrollPolicy = ScrollablePolicy.Natural
            };

            scrolledWindow.Add(_view);
            boxLayout.Add(scrolledWindow);

            var copyButton = new Button("Copy");

            copyButton.Clicked += CopyButton_Activated;

            boxLayout.Add(copyButton);

            Add(boxLayout);

            ShowAll();
        }

        private void CopyButton_Activated(object sender, EventArgs e)
        {
            var clipBoard = _view.GetClipboard(Gdk.Selection.Clipboard);

            var text = _buffer.Text;
            
            clipBoard.Text = text;
        }
    }
}
