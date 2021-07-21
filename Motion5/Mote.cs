using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Motion5
{
    class Mote
    {
        private int x = 0;
        private int y = 0;
        private int wh = 10;
        private Color color = Color.Black;

        public Mote() { }

        public Mote(Color color, int x, int y, int wh)
        {
            this.x = x;
            this.y = y;
            this.wh = wh;
            this.color = color;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Wh
        {
            get { return wh; }
            set { wh = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
    }
}
