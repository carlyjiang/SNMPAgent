using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib;
using System;

public sealed class IntegerObject : ScalarObject
{
    private Integer32 _data;
    public IntegerObject(Int32 data, string id)
        : base(id)
    {
        _data = new Integer32(data);
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

            if (value.TypeCode != SnmpType.Integer32)
            {
                throw new ArgumentException("Wrong Type");
            }

            _data = (Integer32)value;
        }
    }
}