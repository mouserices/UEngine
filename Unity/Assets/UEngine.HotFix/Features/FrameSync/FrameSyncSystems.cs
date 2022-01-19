public class FrameSyncSystems : Feature
{
    public FrameSyncSystems()
    {
#if CLIEN
        Add(new TickAdaptationSystem());
#endif
        Add(new TickSystems());
    }
}