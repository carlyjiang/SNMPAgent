using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Security;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Carl.Agent
{
    class Program
    {
        internal static IUnityContainer Container { get; private set; }
        [STAThread]
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            //Console.ReadKey();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());


            Container = new UnityContainer();
            Container.LoadConfiguration("agent");
        }
    }
}


