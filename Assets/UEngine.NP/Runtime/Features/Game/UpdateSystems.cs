using UEngine.Net;
using UEngine.NP;

public class UpdateSystems : Feature
{
    public UpdateSystems(Contexts contexts, Services services)
    {
        // Net
        Add(new NetworkSystems());
        
        // Input
        Add(new InputSystems(contexts,services));
        Add(new ProcessInputSystem(contexts));

        Add(new MovementSystems(contexts, services));
        
        // Initialize
        Add(new GameInitSystems(contexts,services));
        Add(new UnitFactorySystem(contexts));
        Add(new MirrorSystem(contexts));

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
        Add(new HitNotifySystem(contexts));
        Add(new ListenBuffSystem(contexts));
        Add(new NumericInitSystem(contexts));
        
        
        // Render
        //Add(new PlayerFollowSystem(contexts));
        Add(new UIBloodStripSystem(contexts,services));
        
       
        // Events (Generated)
        Add(new GameEventSystems(contexts));
        
        // Cleanup (Generated, only with Entitas Asset Store version)
        Add(new GameCleanupSystems(contexts));
    }
}