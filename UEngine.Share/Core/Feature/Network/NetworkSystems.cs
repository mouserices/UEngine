using System.Collections;
using System.Collections.Generic;
using Entitas;

namespace UEngine.Net
{
    public class NetworkSystems : Feature
    {
        public NetworkSystems()
        {
            Add(new NetworkStartSystem());
            Add(new NetworkTickSystem());

            Add(new MessageSendSystem());

            Add(new MessageDispacherSystem());
            Add(new NetworkDestroySystem());
        }
    }
}
