namespace SexyFishHorse.WauwBot.View.UiElements.MessageListBox
{
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using ListBox;

    public sealed class MessageListBox : ResizableListBox
    {
        private const int MainTextOffset = 30;
        private readonly Font headingFont;
        private ImageList iconList;
        private System.ComponentModel.IContainer components;

        public MessageListBox()
        {
            InitializeComponent();
            headingFont = new Font(Font, FontStyle.Bold);
            MeasureItem += MeasureItemHandler;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            headingFont.Dispose();

            base.Dispose(disposing);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            var bounds = e.Bounds;
            var textColor = SystemColors.ControlText;

            var item = Items[e.Index];

            // draw selected item background
            if (e.State == DrawItemState.Selected)
            {
                using (Brush b = new SolidBrush(SystemColors.Highlight))
                {
                    // Fill background;
                    e.Graphics.FillRectangle(b, e.Bounds);
                }

                textColor = SystemColors.HighlightText;
            }

            // draw image
            if (item.MessageType != ParseMessageType.None)
            {
                iconList.Draw(
                    e.Graphics,
                    bounds.Left + 1,
                    bounds.Top + 2,
                    (int)item.MessageType);
            }

            using (var textBrush = new SolidBrush(textColor))
            {
                // draw Headline
                e.Graphics.DrawString(
                    item.LineHeader,
                    headingFont,
                    textBrush,
                    bounds.Left + iconList.ImageSize.Width + 5,
                    bounds.Top + iconList.ImageSize.Height - headingFont.Height);

                // Draw layout, 2 times the offset (left & right)
                var oneLine = new Size(Width - (MainTextOffset * 2), Font.Height);

                var textToDraw = new StringBuilder(item.MessageText);
                const int Index1 = 0;
                var top = bounds.Top + iconList.ImageSize.Height + 2;

                while (textToDraw.Length > 0)
                {
                    // Break string into more lines when an end-of-line character is found
                    string strLineToDraw;
                    int index2New;
                    int index2;
                    if ((index2 = textToDraw.ToString().IndexOf('\n')) > 0)
                    {
                        strLineToDraw = textToDraw.ToString(Index1, index2 - Index1);
                        index2New = index2 + 1;
                    }
                    else
                    {
                        index2 = textToDraw.Length;
                        index2New = index2;
                        strLineToDraw = textToDraw.ToString();
                    }

                    int charsFitted;
                    int linesFilled;
                    e.Graphics.MeasureString(
                        strLineToDraw,
                        Font,
                        oneLine,
                        StringFormat.GenericDefault,
                        out charsFitted,
                        out linesFilled);

                    // There's no knowledge about words, so just don't split words up if possible
                    if (charsFitted < index2)
                    {
                        var index = strLineToDraw.LastIndexOf(' ', charsFitted - 1, charsFitted);

                        if (index != -1)
                        {
                            index2New = index + 1;
                        }
                        else
                        {
                            index2New = charsFitted;
                        }

                        strLineToDraw = textToDraw.ToString(Index1, index2New - Index1);
                    }

                    // Draw the text
                    e.Graphics.DrawString(
                        strLineToDraw,
                        Font,
                        textBrush,
                        bounds.Left + MainTextOffset,
                        top);

                    // Adjust top
                    top += Font.Height;

                    // Next line
                    textToDraw = textToDraw.Remove(Index1, index2New);
                }
            }
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.Resources.ResourceManager(typeof(MessageListBox));
            iconList = new ImageList(components)
            {
                ColorDepth = ColorDepth.Depth8Bit,
                ImageSize = new Size(16, 16),
                ImageStream = (ImageListStreamer)resources.GetObject("iconList.ImageStream"),
                TransparentColor = Color.Transparent
            };
        }

        private void MeasureItemHandler(object sender, MeasureItemEventArgs e)
        {
            var item = Items[e.Index];

            // Draw layout, 2 times the offset (left & right)
            var sz = new Size(Width - (MainTextOffset * 2), Font.Height);

            var textToDraw = new StringBuilder(item.MessageText);
            const int Index1 = 0;
            var lines = 0;

            while (textToDraw.Length > 0)
            {
                // Break string into more lines when an end-of-line character is found
                string strLineToDraw;
                int index2New;
                int index2;
                if ((index2 = textToDraw.ToString().IndexOf('\n')) > 0)
                {
                    strLineToDraw = textToDraw.ToString(Index1, index2 - Index1);
                    index2New = index2 + 1;
                }
                else
                {
                    index2 = textToDraw.Length;
                    index2New = index2;
                    strLineToDraw = textToDraw.ToString();
                }

                int charsFitted;
                int linesFilled;
                e.Graphics.MeasureString(
                    strLineToDraw,
                    Font,
                    sz,
                    StringFormat.GenericDefault,
                    out charsFitted,
                    out linesFilled);

                // There's no knowledge about words, so just don't split words up if possible
                if (charsFitted < index2)
                {
                    var index = strLineToDraw.LastIndexOf(' ', charsFitted - 1, charsFitted);

                    if (index != -1)
                    {
                        index2New = index + 1;
                    }
                    else
                    {
                        index2New = charsFitted;
                    }
                }

                lines += linesFilled;
                textToDraw = textToDraw.Remove(Index1, index2New);
            }

            var mainTextHeight = lines * Font.Height;

            e.ItemHeight = iconList.ImageSize.Height + mainTextHeight + 4;
            e.ItemWidth = sz.Width;
        }
    }
}
