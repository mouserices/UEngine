public class TickSystems : FixedTickSystem
{
    public TickSystems()
    {
        Add(new PlayerInputTickSystem());
#if CLIENT
        Add(new MainPlayerMoveSystem(Contexts.sharedInstance));
        Add(new StandardMoveSystem(Contexts.sharedInstance));
        Add(new ChangeFsmStateOnMoveSystem(Contexts.sharedInstance));
#endif
       
    }
}