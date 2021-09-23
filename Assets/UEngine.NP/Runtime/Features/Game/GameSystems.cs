using UEngine.NP;

public class GameSystems : Feature
{
    public GameSystems(Contexts contexts)
    {
        // Input
        Add(new ProcessInputSystem(contexts));
        
        // Initialize
        Add(new GameInitSystem(contexts));
        Add(new UnitFactorySystem(contexts));
        
        // Update
        Add(new AddViewSystem(contexts));
        Add(new BehaveTreeFactorySystem(contexts));
        Add(new StateChangeSystem(contexts));
        Add(new StateRemoveSystem(contexts));
        Add(new PatrolSystem(contexts));
        Add(new BulletScanSystem(contexts));
        Add(new LifeCycleSystem(contexts));

        Add(new DecreaseTimersSystem(contexts));
        Add(new RemoveTimersSystem(contexts));
        
        Add(new DamageSystem(contexts));
        Add(new ClearHitTargetsSystem(contexts));
        Add(new MirrorSystem(contexts));
        Add(new HitNotifySystem(contexts));
        Add(new ListenBuffSystem(contexts));
        
        // Render
        Add(new PlayerFollowSystem(contexts));
        Add(new UIBloodStripSystem(contexts));
        
       
        // Events (Generated)
        Add(new GameEventSystems(contexts));
        
        // Cleanup (Generated, only with Entitas Asset Store version)
        Add(new GameCleanupSystems(contexts));
    }
}