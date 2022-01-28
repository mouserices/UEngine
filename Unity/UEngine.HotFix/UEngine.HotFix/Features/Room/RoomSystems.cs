public class RoomSystems : Feature
{
    public RoomSystems()
    {
        Add(new RoomPlayerSystem());
        Add(new RoomCleanupSystems(Contexts.sharedInstance));
    }
}