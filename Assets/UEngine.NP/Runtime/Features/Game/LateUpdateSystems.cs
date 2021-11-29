public class LateUpdateSystems : Feature
{
    public LateUpdateSystems(Contexts contexts, Services services)
    {
        Add(new UIHpShowSystem(contexts,services));
    }
}