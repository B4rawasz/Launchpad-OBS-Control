using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace launchpadControler
{
    public partial class LaunchpadControl : UserControl
    {

        private int _ID;

        public event EventHandler LPPressed;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        protected virtual void OnLPPPressed(EventArgs e)
        {
            LPPressed?.Invoke(this, e);
        }

        public LaunchpadControl()
        {
            InitializeComponent();
        }

        private void launchpadButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
