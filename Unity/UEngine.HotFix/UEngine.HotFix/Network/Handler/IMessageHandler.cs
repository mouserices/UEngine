using System;

namespace UEngine.Net
{
    public interface IMessageHandler
    {
        void Handle(int connectionId,IMessage request);
        Type GetRequestType();
    }
}