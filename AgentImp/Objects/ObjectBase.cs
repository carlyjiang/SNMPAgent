using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib;

namespace Carl.Agent
{
    public delegate ISnmpData GetDataFrom();
    abstract public class ObjectBase : ScalarObject
    {
        protected string _name;
        protected ObjectIdentifier _id;        
        abstract public event GetDataFrom GetDataHandler;

        public ObjectBase(string id)
            : base(id)
        {
            _id = new ObjectIdentifier(id);
        }

        public string Name
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _name = value;
            }
            get
            {
                return _name;
            }
        }

        public ObjectIdentifier GetId
        {
            get
            {
                return _id;
            }
        }

        public override ISnmpData Data
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public String Description {set;get;}
    }
}
