using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;

namespace Carl.Agent
{
    public enum AuthenticationMethod
    {
        DefaultAuthenticationProvider,
        MD5AuthenticationProvider,
        SHA1AuthenticationProvider
    }

    public enum PrivacyMethod
    {
        DefaultPrivacyProvider,
        DESPrivacyProvider,
        AESPrivacyProvider
    }

    class SecurityUserRegistry
    {
        private OctetString _userName;
        private readonly IAuthenticationProvider _auth;
        private readonly IPrivacyProvider _privacy;
        internal string AuthenticationPassphrase { get; private set; }
        internal string PrivacyPassphrase { get; private set; }
        internal AuthenticationMethod AuthMethod { get; private set; }
        internal PrivacyMethod PrivMethod { get; private set; }
        internal IPrivacyProvider PrivacyProvider 
        {
            get
            {
                if (_privacy != null)
                {
                    return _privacy;
                }
                else
                {
                    throw new ArgumentNullException("_privacy");
                }
            }
            private set {}
        }
        
        public SecurityUserRegistry(string username) 
            : this(username, 
                    AuthenticationMethod.DefaultAuthenticationProvider, 
                    PrivacyMethod.DefaultPrivacyProvider,
                    String.Empty, 
                    String.Empty)
        {
        }

        public SecurityUserRegistry(string username,
            AuthenticationMethod authenticationMethod, 
            PrivacyMethod privacyMethod,
            string authenticationPassphrase, 
            string privacyPassphrase)
        {

            if (authenticationMethod == AuthenticationMethod.DefaultAuthenticationProvider
                && privacyMethod != PrivacyMethod.DefaultPrivacyProvider)
            {
                throw new ArgumentException("Wrong Parameter: AuthenticationMethod and PrivacyMethod are not matched");
            }

            if (authenticationPassphrase.Length < 8)
            {
                throw new ArgumentException("authenticationPassphrase length is too short");
            }
            if (privacyPassphrase.Length < 8)
            {
                throw new ArgumentException("privacyPassphrase length is too short");
            }

            AuthenticationPassphrase = authenticationPassphrase;
            PrivacyPassphrase = privacyPassphrase;
            AuthMethod = authenticationMethod;
            PrivMethod = privacyMethod;
            UserName = new OctetString(username);

            switch (authenticationMethod)
            {
                case AuthenticationMethod.DefaultAuthenticationProvider:
                    _auth = DefaultAuthenticationProvider.Instance;
                    break;
                case AuthenticationMethod.MD5AuthenticationProvider:
                    _auth = new MD5AuthenticationProvider(new OctetString(AuthenticationPassphrase));
                    break;
                case AuthenticationMethod.SHA1AuthenticationProvider:
                    _auth = new SHA1AuthenticationProvider(new OctetString(AuthenticationPassphrase));
                    break;
            }
            switch (privacyMethod)
            {
                case PrivacyMethod.DefaultPrivacyProvider:
                    _privacy = new DefaultPrivacyProvider(_auth);
                    break;
                case PrivacyMethod.DESPrivacyProvider:
                    _privacy = new DESPrivacyProvider(new OctetString(PrivacyPassphrase), _auth);
                    break;
                case PrivacyMethod.AESPrivacyProvider:
                    _privacy = new AESPrivacyProvider(new OctetString(PrivacyPassphrase), _auth);
                    break;
            }
        }

        public OctetString UserName
        {
            set
            {
                if(value != null && value is OctetString)
                {
                    _userName = value;
                }
                else
                {
                    throw new ArgumentException("Value");
                }
            }
            get
            {
                if(_userName != null)
                {
                    return _userName;
                }
                else
                {
                    throw new ArgumentNullException("UserName");
                }
            }
        }
    }
}
