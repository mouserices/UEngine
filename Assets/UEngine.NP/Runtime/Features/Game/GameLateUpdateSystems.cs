public class GameLateUpdateSystems : Feature
{
    public GameLateUpdateSystems(Contexts contexts)
    {
        Add(new UIHpShowSystem(contexts));
    }
}