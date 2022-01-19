using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game,Cleanup(CleanupMode.DestroyEntity)]
public class LoginMessageComponent : IComponent
{
    public int ConnectedID;
    public long PlayerID;
    public string UserName;
    public string Password;
}