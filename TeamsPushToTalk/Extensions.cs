using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsPushToTalk
{
    static class Extensions
    {
        public static V Get<K, V>(this IDictionary<K, V> dic, K k)
        {
            if (dic.TryGetValue(k, out var v))
            {
                return v;
            }
            else
            {
                return default(V);
            }
        }
    }
}
