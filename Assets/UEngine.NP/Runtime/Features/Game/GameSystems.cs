using UEngine.NP;

public class GameSystems : Feature
{
    public GameSystems(Contexts contexts)
    {
        // Input
        Add(new ProcessInputSystem(contexts));
        
        // Initialize
        Add(new UnitFactorySystem(contexts));
        
        // Update
        Add(new AddViewSystem(contexts));
        Add(new BehaveTreeFactorySystem(contexts));
        Add(new StateChangeSystem(contexts));
        Add(new StateRemoveSystem(contexts));
        
        // Render
        Add(new PlayerFollowSystem(contexts));
       
        // Events (Generated)
        Add(new GameEventSystems(contexts));
        
        // Cleanup (Generated, only with Entitas Asset Store version)
        Add(new GameCleanupSystems(contexts));
    }
}