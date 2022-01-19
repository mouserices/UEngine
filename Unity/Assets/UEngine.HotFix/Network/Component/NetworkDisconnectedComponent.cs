using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [Network,Cleanup(CleanupMode.DestroyEntity)]
    public class NetworkDisconnectedComponent : IComponent
    {
        public int ConnectedID;
    }
}