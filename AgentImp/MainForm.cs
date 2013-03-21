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
using System.Globalization;
using System.Net;
using System.Net.Sockets;

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


        /*
        private void StartListeners()
        {
            _engine.Listener.ClearBindings();
            int port = int.Parse(tstxtPort.Text, CultureInfo.InvariantCulture);
            if (tscbIP.Text == StrAllUnassigned)
            {
                if (Socket.SupportsIPv4)
                {
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, port));
                }

                if (Socket.OSSupportsIPv6)
                {
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.IPv6Any, port));
                }

                _engine.Start();
                return;
            }

            
            IPAddress address = IPAddress.Parse(tscbIP.Text);
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                if (!Socket.SupportsIPv4)
                {
                    MessageBox.Show(Listener.ErrorIPv4NotSupported);
                    return;
                }

                _engine.Listener.AddBinding(new IPEndPoint(address, port));
                _engine.Start();
                return;
            }

            if (!Socket.OSSupportsIPv6)
            {
                MessageBox.Show(Listener.ErrorIPv6NotSupported);
                return;
            }

            _engine.Listener.AddBinding(new IPEndPoint(address, port));
            _engine.Start();
        }
        */

        private void StopListeners()
        {
            _engine.Stop();
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
