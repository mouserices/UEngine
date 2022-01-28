public class MovementSystems : Feature
{
    public MovementSystems(Contexts contexts)
    {
#if CLIENT
                Add(new MainPlayerMoveSystem(contexts));
#endif
        Add(new StandardMoveSystem(contexts));
        Add(new ChangeFsmStateOnMoveSystem(contexts));
    }
}