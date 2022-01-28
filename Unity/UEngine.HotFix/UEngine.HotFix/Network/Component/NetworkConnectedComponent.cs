using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [Network,Cleanup(CleanupMode.DestroyEntity)]
    public class NetworkConnectedComponent : IComponent
    {
        public int ConnectedID;
    }
}