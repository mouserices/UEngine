using UnityEngine;

namespace UEngine.UI.Runtime.Tools
{
    public class ResKit
    {
        public static T Load<T>(string resPath) where T : Object
        {
            return Resources.Load<T>(resPath);
        }
    }
}