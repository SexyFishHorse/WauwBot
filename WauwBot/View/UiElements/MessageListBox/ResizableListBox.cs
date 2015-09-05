namespace SexyFishHorse.WauwBot.View.UiElements.MessageListBox
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public class ResizableListBox : Panel
    {
        // our data containers - exposed via properties
        private readonly ListBoxList items = new ListBoxList();

        private readonly ArrayList selectedItems = new ArrayList();

        private readonly ArrayList selectedItemIndices = new ArrayList();

        private bool ctrlPressed;

        public ResizableListBox()
        {
            // We are going to do all of the painting so better 
            // setup the control to use double buffering
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque
                | ControlStyles.UserPaint | ControlStyles.DoubleBuffer,
                true);

            // set some defaults
            BackColor = Color.White;
            AutoScroll = true;
            HScroll = false;

            items.ItemInserted += ItemInserted;
        }

        public event EventHandler SelectedIndexChanged;

        public event MeasureItemEventHandler MeasureItem;

        public event DrawItemEventHandler DrawItem;

        public ListBoxList Items
        {
            get { return items; }
        }

        public object SelectedItem
        {
            get
            {
                return selectedItems.Count > 0 ? selectedItems[0] : null;
            }

            set
            {
                var pos = items.IndexOf(value);
                if (pos < 0)
                {
                    return;
                }

                // clear list
                selectedItemIndices.Clear();
                selectedItems.Clear();

                // add item
                AddSelectedItem(pos);
            }
        }

        public ArrayList SelectedItems
        {
            get
            {
                return selectedItems;
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (selectedItemIndices.Count > 0)
                {
                    return (int)selectedItemIndices[0];
                }

                return -1;
            }

            set
            {
                if ((value < items.Count) && (value >= -1))
                {
                    AddSelectedItem(value);
                }
            }
        }

        public ArrayList SelectedItemIndices
        {
            get
            {
                return selectedItemIndices;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            ctrlPressed = e.Control;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            ctrlPressed = e.Control;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // call base implementation
            base.OnMouseDown(e);

            // make sure we receive key events
            Focus();

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            // determine which item was clicked
            var index = ItemHitTest(e.Y);

            if (index < 0)
            {
                return;
            }

            if (ctrlPressed)
            {
                if (selectedItemIndices.Contains(index))
                {
                    RemoveSelectedItem(index);
                }
                else
                {
                    AddSelectedItem(index);
                }
            }
            else
            {
                if (selectedItemIndices.Contains(index) && (selectedItemIndices.Count == 1))
                {
                    return;
                }

                selectedItemIndices.Clear();
                selectedItems.Clear();

                AddSelectedItem(index);
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var g = pe.Graphics;
            var bounds = new Rectangle();
            var posScrollY = AutoScrollPosition.Y;
            var height = 0;

            // clear background
            using (Brush b = new SolidBrush(BackColor))
            {
                // Fill background;
                g.FillRectangle(b, ClientRectangle);
            }

            // draw our items
            var i = 0;
            while (i < items.Count)
            {
                // measure only when neccesary
                if (!items.Info(i).HeightValid)
                {
                    var miea = new MeasureItemEventArgs(g, i);
                    OnMeasureItem(miea);
                }

                var itemHeight = items.Info(i).Height;

                if ((posScrollY + itemHeight >= 0) && posScrollY < ClientRectangle.Height)
                {
                    bounds.Location = new Point(AutoScrollPosition.X, posScrollY);
                    bounds.Size = new Size(ClientRectangle.Right, itemHeight);

                    var state = selectedItemIndices.Contains(i) ? DrawItemState.Selected : DrawItemState.Default;
                    var diea = new DrawItemEventArgs(
                        g,
                        Font,
                        bounds,
                        i,
                        state,
                        ForeColor,
                        BackColor);
                    OnDrawItem(diea);
                }

                posScrollY += itemHeight;
                height += itemHeight;
                i++;
            }

            AutoScrollMinSize = new Size(Width - 30, height);
        }

        protected override void OnResize(EventArgs e)
        {
            for (var i = 0; i < items.Count; i++)
            {
                items.Info(i).HeightValid = false;
            }

            base.OnResize(e);
        }

        protected virtual void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            var bounds = e.Bounds;
            var textColor = SystemColors.ControlText;

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

            using (var textBrush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(items[e.Index].ToString(), Font, textBrush, bounds.Left, bounds.Top);
            }

            if (DrawItem != null)
            {
                DrawItem(this, e);
            }
        }

        private void ItemInserted(int index)
        {
            for (var i = 0; i < selectedItemIndices.Count; i++)
            {
                var selIndex = (int)selectedItemIndices[i];
                if (selIndex >= index)
                {
                    selectedItemIndices[i] = selIndex + 1;
                }
            }
        }

        private int ItemHitTest(int y)
        {
            var posY = AutoScrollPosition.Y;
            for (var i = 0; i < items.Count; i++)
            {
                posY += items.Info(i).Height;

                if (y < posY)
                {
                    return i;
                }
            }

            return -1;
        }

        private void AddSelectedItem(int index)
        {
            if (index == -1)
            {
                selectedItemIndices.Clear();
                selectedItems.Clear();
            }
            else
            {
                selectedItemIndices.Add(index);
                selectedItems.Add(items[index]);
            }

            OnSelectedIndexChanged(new EventArgs());
        }

        private void RemoveSelectedItem(int index)
        {
            selectedItemIndices.Remove(index);
            selectedItems.Remove(items[index]);
            OnSelectedIndexChanged(new EventArgs());
        }

        private void OnMeasureItem(MeasureItemEventArgs e)
        {
            e.ItemHeight = Font.Height;

            if (MeasureItem != null)
            {
                MeasureItem(this, e);
            }

            items.Info(e.Index).Height = e.ItemHeight;
        }

        private void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, e);
            }
        }
    }
}
