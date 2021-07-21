using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace Motion5
{
    class Space : Panel
    {
        private ArrayList motes = new ArrayList();

        public Space()
        {
            this.BackColor = Color.Black;
            this.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        public ArrayList Motes
        {
            get
            {
                ArrayList newMotes = new ArrayList(motes);
                return newMotes;
            }
            set
            {
                motes = value;
            }
        }

        public void RepaintSpace()
        {
            int width = this.Width;
            int height = this.Height;
            if (width > 0 && height > 0)
            {

                Image image = new Bitmap(this.Width, this.Height);
                Graphics buffer = Graphics.FromImage(image);
                buffer.Clear(Color.Black);
                IEnumerator motesEnumerator = motes.GetEnumerator();
                while (motesEnumerator.MoveNext())
                {
                    Mote mote = (Mote)motesEnumerator.Current;
                    buffer.FillRectangle(new SolidBrush(mote.Color), Math.Abs(mote.X), Math.Abs(mote.Y), Math.Abs(mote.Wh), Math.Abs(mote.Wh));
                }
                Graphics graphics = this.CreateGraphics();
                graphics.DrawImage(image, 0, 0);

                buffer.Dispose();
                graphics.Dispose();
                image.Dispose();
            }
        }
    }
}
