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
            //new ObjectRegistry().test();
            //Console.ReadKey();

            Container = new UnityContainer();
            Container.LoadConfiguration("agent");

            var store = Container.Resolve<ObjectStore>();
            store.Add(new SysDescr());
            store.Add(new SysObjectId());
            store.Add(new SysUpTime());
            store.Add(new SysContact());
            store.Add(new SysName());
            store.Add(new SysLocation());
            store.Add(new SysServices());
            store.Add(new SysORLastChange());
            store.Add(new SysORTable());


            SecurityUserRegistry user = new SecurityUserRegistry("user2", 
                AuthenticationMethod.MD5AuthenticationProvider, 
                PrivacyMethod.AESPrivacyProvider, 
                "abcdefghijk", "abcdefghijk");
            var users = Container.Resolve<UserRegistry>();
            users.Add(user.UserName, user.PrivacyProvider);
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));


            ObjectRegistry or = new ObjectRegistry();
            or.Load(or.DefaultFileName);
            var allObjects = or.GetAllObject();

            foreach (var v in allObjects)
            {
                store.Add(v);
            }

            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
