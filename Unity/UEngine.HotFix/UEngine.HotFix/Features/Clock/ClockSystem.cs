using Entitas;

public class ClockSystem : IExecuteSystem
{
    public void Execute()
    {
        var clock = MetaContext.Get<IClockService>().GetClock();
        var deltaTime = MetaContext.Get<ITimeService>().deltaTime();
        clock.Update(deltaTime);
    }
}