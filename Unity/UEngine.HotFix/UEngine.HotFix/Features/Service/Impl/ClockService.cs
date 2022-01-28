using NPBehave;

[Service]
public class ClockService : IClockService
{
    private Clock _clock;
    public Clock GetClock()
    {
        if (_clock == null)
        {
            _clock = new Clock();
        }

        return _clock;
    }
}