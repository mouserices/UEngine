using Entitas;
using Entitas.CodeGeneration.Attributes;

[Room]
public class TickComponent : IComponent
{
    public int TickCount;
    public TimerTick TimerTick;
    public int ServerTickCount;
}