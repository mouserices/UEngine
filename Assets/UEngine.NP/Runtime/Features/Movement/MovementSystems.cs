public class MovementSystems : Feature
{
    public MovementSystems(Contexts contexts, Services services)
    {
        Add(new MainPlayerMoveSystem(contexts, services));
        Add(new StandardMoveSystem(contexts,services));
        Add(new ChangeFsmStateOnMoveSystem(contexts,services));
    }
}