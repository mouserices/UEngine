using System;
using UnityEngine.Rendering.VirtualTexturing;

namespace UEngine.Net
{
    public interface IMessageHandler
    {
        void Handle(int connectionId,IMessage request);
        Type GetRequestType();
    }
}