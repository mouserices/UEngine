using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [Network]
    [Unique]
    public class ClientConnectingComponent : IComponent
    {
        public string IP;
        public int Port;
    }
}