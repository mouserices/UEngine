//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月23日 13:52:22
//------------------------------------------------------------

using System;

namespace UEngine.NP
{
    public interface INP_BBValue<T>
    {
        T GetValue();
        void SetValue(INP_BBValue<T> bbValue);
    }
}