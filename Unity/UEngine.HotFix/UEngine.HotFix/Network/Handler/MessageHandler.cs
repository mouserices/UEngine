using System;

namespace UEngine.Net
{
    public abstract class MessageHandler<T, K> : IMessageHandler where T : IMessage where K : IResponse
    {
        public abstract void OnMessageHandle(int connectionId,T request, Action<K> response);

        public void Handle(int connectionId,IMessage request)
        {
            Action<K> reply = response =>
            {
                response.RpcID = request.RpcID;
                MetaContext.Get<INetworkService>().SendResponse(connectionId, response);
            };
            this.OnMessageHandle(connectionId,(T)request, reply);
        }

        public Type GetRequestType()
        {
            return typeof(T);
        }
    }

    public abstract class MessageHandler<T> : IMessageHandler where T : ISimpleMessage
    {
        public abstract void OnMessageHandle(int connectionId,T request);

        public void Handle(int connectionId,IMessage request)
        {
            OnMessageHandle(connectionId, (T)request);
        }

        public Type GetRequestType()
        {
            return typeof(T);
        }
    }
}