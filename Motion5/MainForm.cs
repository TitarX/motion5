using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace Motion5
{
    public delegate void MotionProcessDelegate();
    public partial class MainForm : Form
    {
        private int speed = 0;
        private int colorIndex = 0;
        private Space space = null;
        private Thread newThread = null;
        private FormParameters formParameters = null;
        private bool isRun = true;
        private MotionProcessDelegate motionProcessDelegate = null;

        public MainForm()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Motion";
            this.FormClosing += new FormClosingEventHandler(FormClosingProcess);
            space = new Space();
            space.Size = new Size(panel.Width, panel.Height);
            space.Click += new EventHandler(EscapeFullScreen);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Controls.Add(space);

            speedTrackBar.Minimum = 1;
            speedTrackBar.Maximum = 100;
            speedTrackBar.Value = 90;
            speedTrackBar.TickFrequency = 1;
            ToolTip speedTrackBarToolTip = new ToolTip();
            speedTrackBarToolTip.SetToolTip(speedTrackBar, "Speed");

            speed = 101 - speedTrackBar.Value;

            newThread = new Thread(this.Run);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            ProcessOnExit();
            Application.Exit();
        }

        private void FormClosingProcess(object sender, EventArgs e)
        {
            ProcessOnExit();
        }

        private void ProcessOnExit()
        {
            if (newThread.IsAlive)
            {
                isRun = false;
                newThread.Abort();
                newThread.Join();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!newThread.IsAlive)
            {
                newThread.Start();
            }

            int wh = new Random().Next(10, 100);

            bool isPlace = true;
            IEnumerator otherMotesCloneEnumerator = ((ArrayList)space.Motes).GetEnumerator();
            while (otherMotesCloneEnumerator.MoveNext())
            {
                Mote otherMote = (Mote)otherMotesCloneEnumerator.Current;
                if ((Math.Abs(otherMote.X) < wh) || (Math.Abs(otherMote.Y) < wh))
                {
                    isPlace = false;
                }
            }
            if (isPlace)
            {
                Mote mote = new Mote();
                mote.X = 0;
                mote.Y = 0;
                mote.Wh = wh;
                colorIndex++;
                if (colorIndex > 4)
                {
                    colorIndex = 1;
                }
                switch (colorIndex)
                {
                    case 1:
                        mote.Color = Color.Red;
                        break;
                    case 2:
                        mote.Color = Color.Green;
                        break;
                    case 3:
                        mote.Color = Color.Blue;
                        break;
                    case 4:
                        mote.Color = Color.Yellow;
                        break;
                    default:
                        mote.Color = Color.Black;
                        break;
                }
                ArrayList newMotes = space.Motes;
                newMotes.Add(mote);
                space.Motes = newMotes;
            }
        }

        public void Run()
        {
            while (isRun)
            {
                motionProcessDelegate = new MotionProcessDelegate(MotionProcess);
                space.Invoke(motionProcessDelegate);
                Thread.Sleep(speed);
            }
        }

        private void MotionProcess()
        {
            space.RepaintSpace();
            IEnumerator motesCloneEnumerator = ((ArrayList)space.Motes).GetEnumerator();
            while (motesCloneEnumerator.MoveNext())
            {
                Mote mote = (Mote)motesCloneEnumerator.Current;
                int x = mote.X;
                int y = mote.Y;

                int widthLimit = space.Width - mote.Wh;
                if (Math.Abs(x) + 1 >= widthLimit)
                {
                    x = Math.Abs(x);
                    if (x >= widthLimit)
                    {
                        x = widthLimit - 1;
                    }
                }
                int heightLimit = space.Height - mote.Wh;
                if (Math.Abs(y) + 1 >= heightLimit)
                {
                    y = Math.Abs(y);
                    if (y >= heightLimit)
                    {
                        y = heightLimit - 1;
                    }
                }
                IEnumerator otherMotesEnumerator = ((ArrayList)space.Motes).GetEnumerator();
                while (otherMotesEnumerator.MoveNext())
                {
                    Mote otherMote = (Mote)otherMotesEnumerator.Current;
                    if (!mote.Equals(otherMote))
                    {
                        int mr = mote.Wh / 2;
                        int mx = Math.Abs(x) + mr;
                        int my = Math.Abs(y) + mr;

                        int omr = otherMote.Wh / 2;
                        int omx = Math.Abs(otherMote.X) + omr;
                        int omy = Math.Abs(otherMote.Y) + omr;

                        if ((Math.Abs(mx - omx) < mr + omr) && (Math.Abs(my - omy) < mr + omr))
                        {
                            if ((Math.Abs(mx - omx) < mr + omr - 2) && (Math.Abs(my - omy) < mr + omr - 2))
                            {
                                mote.Wh--;
                                if (mote.Wh < 10)
                                {
                                    ArrayList newMotes = space.Motes;
                                    newMotes.Remove(mote);
                                    space.Motes = newMotes;
                                }
                            }

                            //Угловое столкновение
                            if (((Math.Abs((mx - (mr + omr - 1)) - omx) < 3 || Math.Abs((mx + (mr + omr - 1)) - omx) < 3)
                                    && Math.Abs((my - (mr + omr - 1)) - omy) < 3 || Math.Abs((my + (mr + omr - 1)) - omy) < 3)
                                    || mx == omx || my == omy)
                            {
                                x = -x;
                                y = -y;
                            }
                            else if (Math.Abs(mx - omx) > Math.Abs(my - omy))
                            {
                                x = -x;
                            }
                            else
                            {
                                y = -y;
                            }
                        }
                    }
                }
                mote.X = --x;
                mote.Y = --y;
            }
        }

        private void speedTrackBar_Scroll(object sender, EventArgs e)
        {
            speed = 101 - speedTrackBar.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (formParameters == null)
            {
                formParameters = new FormParameters(this.FormBorderStyle, this.WindowState, panel.Location, panel.Size);
                exitButton.Visible = false;
                fullScreenButton.Visible = false;
                addButton.Visible = false;
                speedTrackBar.Visible = false;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                panel.Location = new Point(0, 0);
                panel.Size = new Size(this.Width, this.Height);
                Cursor.Hide();
                space.Focus();
            }
        }

        private void EscapeFullScreen(object sender, EventArgs e)
        {
            if (formParameters != null)
            {
                exitButton.Visible = true;
                fullScreenButton.Visible = true;
                addButton.Visible = true;
                speedTrackBar.Visible = true;
                this.FormBorderStyle = formParameters.FormBorderStyle;
                this.WindowState = formParameters.FormWindowState;
                panel.Location = formParameters.PanelLocation;
                panel.Size = formParameters.PanelSize;
                Cursor.Show();
                formParameters = null;
            }
        }
    }
}
