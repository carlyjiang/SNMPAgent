using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib;

namespace Carl.Agent
{
    class DataGetMethodFactory
    {
        private static object _lock = new object();
        private static DataGetMethodFactory _instance;
        private DataGetMethodFactory()
        {
        }
        //get the single instance
        public static DataGetMethodFactory GetDataGetMethodFactory()
        {
            if (_instance == null)
            {
                lock (_lock)
                if (_instance == null)
                {
                    _instance = new DataGetMethodFactory();
                }
            }
            return _instance;
        }

        public ISnmpData GetMethodInteger32()
        {
            return new Integer32(123);
        }
    }
}
