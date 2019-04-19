using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Common.Share
{
    public class GlobalStatic
    {
        /// <summary>
        /// 静态policies，用于保存同一函数的Policy，同一个函数有一个Policy就可以了
        /// </summary>
        public static ConcurrentDictionary<object, AsyncPolicy> dict_policies = new ConcurrentDictionary<object, AsyncPolicy>();
    }
}
