using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib;
using System;

public sealed class SampleObject : ScalarObject
{
    private OctetString _data;
    public SampleObject(string data, string id) : base(id)
    {
        _data = new OctetString(data);
    }
    public override ISnmpData Data
    {
        get
        {
            if (_data != null)
                return _data;
            else
                throw new ArgumentNullException("Data");
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (value.TypeCode != SnmpType.OctetString)
            {
                throw new ArgumentException("Wrong Type");
            }

            _data = (OctetString)value;
        }
    }
}