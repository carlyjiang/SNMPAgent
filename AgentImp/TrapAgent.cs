using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Carl.Agent
{
    public class TrapAgent
    {
        private static IPAddress _ip;
        private static int _port;
        private static object _portlock = new object();
        private static object _iplock = new object();

        public static IPAddress IP 
        {
            get
            {
                if (_ip == null)
                {
                    throw new ArgumentNullException("_ip");
                }
                return _ip;
            }
            set
            {
                lock (_iplock)
                {
                    if(value == null)
                    {
                        throw new ArgumentNullException("value");
                    }
                    _ip = value;
                }
            }
        }

        public static int Port
        {
            get
            {
                if (_port == 0)
                {
                    throw new ArgumentException("Wrong Port Number");
                }
                return _port;
            }
            set
            {
                lock (_portlock)
                {
                    if (value != 0)
                    {
                        _port = value;
                    }
                    else
                    {
                        throw new ArgumentException("Wrong Parameter");
                    }
                }
            }
        }

        public static IPAddress SetIP(string ip)
        {
            return IP = IPAddress.Parse(ip);
        }
        public static int SetPort(int port)
        {
            if (port == 0)
            {
                throw new ArgumentException("Invalid Port Number");
            }
            return Port = port;
        }
        public TrapAgent(IPAddress ip, int port)
        {
            IP = ip;
            Port = port;
        }

        static TrapAgent()
        {
            IP = IPAddress.Parse("127.0.0.1");
            Port = 62;
        }

        public static IPAddress GetLocalIP()
        {
            return Dns.GetHostEntry(String.Empty).AddressList.Where(o => !o.IsIPv6LinkLocal).FirstOrDefault();
        }

        public static void SendTrapV1(GenericCode genericCode, string community, IList<Variable> list)
        {
            Messenger.SendTrapV1(
                new IPEndPoint(IP, Port),
                GetLocalIP(),
                new OctetString(community),
                new ObjectIdentifier("1.3.6"),
                genericCode,
                0,
                0,
                list);
        }

        public static void SendTrapV2(IList<Variable> list, string community)
        {
            Messenger.SendTrapV2(
                0,
                VersionCode.V2,
                new IPEndPoint(IP, Port),
                new OctetString(community),
                new ObjectIdentifier("1.3.6"),
                0,
                list);
        }

        public static void SendInform(VersionCode version, string community, IList<Variable> list, UserRegistry user)
        {
            if (version == VersionCode.V3)
            {
                IPrivacyProvider privacy = user.Find(new OctetString(community));
                if(privacy == null)
                {
                    throw new Exception("User not found");
                }
                SendInform(version, community, list, 2000, privacy);
            }
            SendInform(version, community, list, 2000, null);
        }

        public static void SendInform(VersionCode version, string community, IList<Variable> list, int timeout, IPrivacyProvider privacy)
        {
            IPEndPoint receiver = new IPEndPoint(IP, Port);
            if (version == VersionCode.V3)
            {
                Discovery discovery = Messenger.NextDiscovery;
                ReportMessage report = discovery.GetResponse(2000, receiver);

                Messenger.SendInform(
                   0,
                   version,
                   receiver,
                   new OctetString(community),
                   new ObjectIdentifier("1.3.6"),
                   0,
                   list,
                   timeout,
                   privacy,
                   report);
                Console.WriteLine("Done");
            }
            else
            {
                Messenger.SendInform(
                    0,
                    version,
                    receiver,
                    new OctetString(community),
                    new ObjectIdentifier("1.3.6"),
                    0,
                    list,
                    timeout,
                    null,
                    null);
            }
        }
    }
}
