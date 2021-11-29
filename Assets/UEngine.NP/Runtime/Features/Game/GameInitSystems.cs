public class GameInitSystems : Feature
{
    public GameInitSystems(Contexts contexts, Services services)
    {
        Add(new UIInitSystem(contexts,services));
        Add(new CameraInitSystem(contexts, services));
    }
}