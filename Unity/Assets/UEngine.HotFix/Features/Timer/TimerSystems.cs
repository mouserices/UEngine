public class TimerSystems : Feature
{
    public TimerSystems(Contexts contexts)
    {
        Add(new DecreaseTimersSystem(contexts));
        Add(new RemoveTimersSystem(contexts));
    }
}