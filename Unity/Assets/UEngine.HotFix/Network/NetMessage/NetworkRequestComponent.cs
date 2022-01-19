using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [Network,Unique]
    public class NetworkRequestComponent : IComponent
    {
        public Dictionary<int, NetworkRequest> Requests;
    }
}