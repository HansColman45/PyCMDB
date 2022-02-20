using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Hooks
{
    public class TestData
    {
        public static Dictionary<string, Object> Data { get; set; } = new Dictionary<string, Object>();
        public static void Add(string Key, Object t)
        {
            if (!Data.ContainsKey(Key))
                Data.Add(Key, t);
            else
                Data[Key] = t;
        }
        public static Object Get(string Key)
        {
            if (Data.ContainsKey(Key))
                return Data[Key];
            else
                return null;
        }
    }
}
