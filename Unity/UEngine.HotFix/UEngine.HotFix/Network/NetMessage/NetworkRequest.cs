using System;
using System.Runtime.CompilerServices;

namespace UEngine.Net
{
    public class NetworkRequest
    {
        private IResponse result;
        public Action OnComplete;
        public bool IsDone;
        public int RpcId { get; set; }

        public NetworkRequest(int rpcId)
        {
            RpcId = rpcId;
        }

        public IResponse GetResult()
        {
            return result;
        }

        public void SetResult(IResponse t)
        {
            result = t;
            IsDone = true;
            if (OnComplete != null)
            {
                OnComplete();
                OnComplete = null;
            }
        }

        public NetworkRequestAwaiter GetAwaiter()
        {
            return new NetworkRequestAwaiter(this);
        }
    }

    public struct NetworkRequestAwaiter : INotifyCompletion
    {
        private NetworkRequest _networkRequest;
        private Action _onComplete;

        public NetworkRequestAwaiter(NetworkRequest networkRequest)
        {
            _networkRequest = networkRequest;
            _onComplete = null;
        }
        public bool IsCompleted => _networkRequest.IsDone;

        public void OnCompleted(Action continuation)
        {
            _onComplete = () =>
            {
                if (continuation != null)
                {
                    continuation();
                }
            };
            _networkRequest.OnComplete = _onComplete;
        }

        public IResponse GetResult()
        {
            return _networkRequest.GetResult();
        }
    }
}