using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Security;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity;

namespace Carl.Agent
{
    class Program
    {
        internal static IUnityContainer Container { get; private set; }
        [STAThread]
        static void Main(string[] args)
        {
            new ObjectRegistry().test();
            Console.ReadKey();
        }
    }
}
