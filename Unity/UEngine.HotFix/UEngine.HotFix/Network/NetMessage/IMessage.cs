namespace UEngine.Net
{
    public interface IMessage
    {
        int RpcID { get; set; }
    }

    public interface ISimpleMessage : IMessage
    {
    }

    public interface IRequest : IMessage
    {
    }

    public interface IResponse : IMessage
    {
    }
}