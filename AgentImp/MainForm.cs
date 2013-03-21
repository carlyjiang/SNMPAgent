using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using Microsoft.Practices.Unity;

namespace Carl.Agent
{
    public partial class MainForm : Form
    {
        private readonly SnmpEngine _engine;
        public MainForm()
        {
            InitializeComponent();
            _engine = Program.Container.Resolve<SnmpEngine>();
            _engine.ExceptionRaised += (sender, e) => MessageBox.Show(e.Exception.ToString());


            
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            using (ConfigForm config = new ConfigForm())
            {
                if (config.ShowDialog() != DialogResult.OK)
                    return;
            }
        }
    }
}
