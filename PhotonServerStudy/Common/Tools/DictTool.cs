
using System.Collections.Generic;

namespace Common.Tools
{
    public class DictTool
    {
        public static R GetValue<T, R>(Dictionary<T, R> dict, T key)
        {
            R value;
            bool isSuccess = dict.TryGetValue(key, out value);
            if (isSuccess)
            {
                return value;
            }
            else
            {
                return default(R);
            }
        }
    }
}
