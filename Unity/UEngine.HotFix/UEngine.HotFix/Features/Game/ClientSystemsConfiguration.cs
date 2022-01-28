using UEngine.HotFix;
using UEngine.Net;
using UEngine.NP;

public class ClientSystemsConfiguration : Feature, ISystemConfiguration
{
    public void InitializeSystems(Contexts contexts)
    {
        Add(new AutoRegisterServiceSystem());
        Add(new NetworkSystems());
        Add(new BehaveTreeFactorySystem(contexts));
        Add(new ClockSystem());
        
        Add(new GameStartSystem());
        Add(new ViewSystem(contexts));
        Add(new StateChangeSystem(contexts));
        Add(new StateRemoveSystem(contexts));
        Add(new PingSystem());
        Add(new RoomSystems());
        Add(new FrameSyncSystems());
        Add(new CameraFollowSystem());
        
        Add(new UnitEventSystems(contexts));
        return;

        // Input
        //Add(new InputSystems(contexts));

        Add(new MovementSystems(contexts));

        // Initialize
        Add(new UnitFactorySystem(contexts));
        //Add(new MirrorSystem(contexts));

        // Update
        Add(new ViewSystem(contexts));
        Add(new BehaveTreeFactorySystem(contexts));
        Add(new StateChangeSystem(contexts));
        Add(new StateRemoveSystem(contexts));
        //Add(new PatrolSystem(contexts));
        Add(new BulletScanSystem(contexts));

        Add(new TimerSystems(contexts));

        Add(new DamageSystem(contexts));
        Add(new ClearHitTargetsSystem(contexts));
        Add(new HitNotifySystem(contexts));
        Add(new ListenBuffSystem(contexts));
        Add(new NumericInitSystem(contexts));


        // Render
        //Add(new PlayerFollowSystem(contexts));
        //Add(new UIBloodStripSystem(contexts, services));


        // Events (Generated)
        //Add(new GameEventSystems(contexts));

        // Cleanup (Generated, only with Entitas Asset Store version)
        //Add(new GameCleanupSystems(contexts));
    }
}