using System.Collections;
using System.Collections.Generic;
using Entitas;
using UEngine.Net.System;

namespace UEngine.Net
{
    public class NetworkSystems : Feature
    {
        public NetworkSystems()
        {
            Add(new NetworkInitSystem());
            Add(new NetworkStartSystem());
            Add(new NetworkTickSystem());

            Add(new MessageSendSystem());

            Add(new MessageDispacherSystem());
            Add(new NetworkDestroySystem());
            
            //clear
            Add(new NetworkCleanupSystems(Contexts.sharedInstance));
        }
    }
}
