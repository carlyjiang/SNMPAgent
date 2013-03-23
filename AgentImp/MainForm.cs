using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

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
        private readonly int _listenPort = 161;
        private readonly int _sendPort = 162;
        private readonly string _listenAddress = "0.0.0.0";
        private readonly string _sendAddress = "127.0.0.1";

        public MainForm()
        {
            InitializeComponent();
            _engine = Program.Container.Resolve<SnmpEngine>();
            _engine.ExceptionRaised += (sender, e) => MessageBox.Show(e.Exception.ToString());

            buttonListener.Text = "Start Listener";
        }

        private bool StartListeners()
        {
            _engine.Listener.ClearBindings();
            int port = _listenPort;
            if (_listenAddress == "0.0.0.0")
            {
                if (Socket.OSSupportsIPv4)
                {
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, port));
                }

                if (Socket.OSSupportsIPv6)
                {
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.IPv6Any, port));
                }

                try
                {
                    _engine.Start();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
                return true;
            }

            IPAddress address = IPAddress.Parse(_listenAddress);
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                if (!Socket.OSSupportsIPv4)
                {
                    MessageBox.Show(Listener.ErrorIPv4NotSupported);
                    return false;
                }

                _engine.Listener.AddBinding(new IPEndPoint(address, port));
                _engine.Start();
                return true;
            }

            if (!Socket.OSSupportsIPv6)
            {
                MessageBox.Show(Listener.ErrorIPv6NotSupported);
                return false;
            }

            _engine.Listener.AddBinding(new IPEndPoint(address, port));
            _engine.Start();
            return true;
        }
        

        private void StopListeners()
        {
            _engine.Stop();
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            using (ConfigForm config = new ConfigForm())
            {
                if (config.ShowDialog() != DialogResult.OK)
                {
                    return;
                }


            }
        }

        private void setListenerButton()
        {
            if (_engine.Listener.Active)
            {
                StopListeners();
                buttonListener.Text = "Start Listener";
            }
            else if (!_engine.Listener.Active)
            {
                if(StartListeners())
                    buttonListener.Text = "Stop Listener";
            }
        }

        private void buttonListener_Click(object sender, EventArgs e)
        {
            setListenerButton();
        }
    }
}
