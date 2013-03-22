using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;

namespace Carl.Agent
{
    public sealed class StringObject : ObjectBase
    {
        private OctetString _data;
        public override event GetDataFrom GetDataHandler;

        public StringObject(string id, string name, string data)
            : base(id)
        {
            _data = new OctetString(data);
            _name = name;
        }

        public StringObject(string id, string name)
            : base(id)
        {
            _name = name;
        }

        public override ISnmpData Data
        {
            get
            {
                if (GetDataHandler != null)
                {
                    ISnmpData data = GetDataHandler();
                    if (data.TypeCode != SnmpType.OctetString)
                    {
                        throw new ArgumentException("Wrong Type");
                    }
                    return data;
                }
                if (_data != null)
                {
                    return _data;
                }
                else
                {
                    throw new ArgumentNullException("Data");
                }
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Value");
                }
                if (value.TypeCode != SnmpType.OctetString)
                {
                    throw new ArgumentException("Wrong Type");
                }
                _data = (OctetString)value;
            }
        }
    }
}
