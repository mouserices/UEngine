namespace UEngine.Net
{
    public static class RpcIdGenerater
    {
        private static int _rpcId;

        public static int GetRpcID()
        {
            return ++_rpcId;
        }
    }
}