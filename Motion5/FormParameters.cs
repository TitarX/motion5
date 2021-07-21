using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Motion5
{
    class FormParameters
    {
        private FormBorderStyle formBorderStyle;
        private FormWindowState formWindowState;
        private Point panelLocation;
        private Size panelSize;

        public FormParameters(FormBorderStyle formBorderStyle, FormWindowState formWindowState
            , Point panelLocation, Size panelSize)
        {
            this.formBorderStyle = formBorderStyle;
            this.formWindowState = formWindowState;
            this.panelLocation = panelLocation;
            this.panelSize = panelSize;
        }

        public FormBorderStyle FormBorderStyle
        {
            get
            {
                return formBorderStyle;
            }
        }

        public FormWindowState FormWindowState
        {
            get
            {
                return formWindowState;
            }
        }

        public Point PanelLocation
        {
            get
            {
                return panelLocation;
            }
        }

        public Size PanelSize
        {
            get
            {
                return panelSize;
            }
        }
    }
}
