using Entitas;
using Entitas.CodeGeneration.Attributes;
using Telepathy;

namespace UEngine.Net
{
    [Network]
    [Unique]
    public class NetworkClientComponent : IComponent
    {
        public Client Client;
    }
}