using System;
using System.Collections.Generic;
using Entitas;

public class FixedTickSystem : Systems
{
    private readonly List<ITickSystem> _tickSystems = new List<ITickSystem>();

    private IGroup<RoomEntity> _group;
    private TimeSpan _targetTicksPerFrame;
    private TickContext _tickContext;
    private RoomContext _roomContext;

    public override void Initialize()
    {
        base.Initialize();
        _tickContext = Contexts.sharedInstance.tick;
        _roomContext = Contexts.sharedInstance.room;
        _group = _roomContext.GetGroup(RoomMatcher.Tick);
        _targetTicksPerFrame = new TimeSpan(TimeSpan.TicksPerSecond / 20);
    }

    public override Systems Add(ISystem system)
    {
        if (system is ITickSystem tickSystem)
        {
            _tickSystems.Add(tickSystem);
        }

        return this;
    }

    public override void Execute()
    {
        if (_group.count <= 0)
        {
            return;
        }

        foreach (var roomEntity in _group.GetEntities())
        {
            var timerTick = roomEntity.tick.TimerTick;
            timerTick.Tick();

            var targetTickCount = (int)(timerTick.TotalTimeWithPause.Ticks / _targetTicksPerFrame.Ticks);

//             if (roomEntity.tick.TickCount < targetTickCount)
//             {
//                 roomEntity.tick.TickCount++;
// #if CLIENT
//                 roomEntity.tick.ServerTickCount++;
// #endif
//                 Tick();
//             }

            while (roomEntity.tick.TickCount < targetTickCount)
            {
                roomEntity.tick.TickCount++;
#if CLIENT
                roomEntity.tick.ServerTickCount++;
#endif
                Tick();
            }
        }


        // var elapsedAdjustedTime = _timerTick.ElapsedTimeWithPause;
        //
        // if (Math.Abs(elapsedAdjustedTime.Ticks - _targetElapsedTime.Ticks) < (_targetElapsedTime.Ticks >> 6))
        // {
        //     elapsedAdjustedTime = _targetElapsedTime;
        // }
        //
        // _accumulatedElapsedGameTime += elapsedAdjustedTime;
        //
        // var updateCount = (int) (_accumulatedElapsedGameTime.Ticks / _targetElapsedTime.Ticks);
        // if (updateCount == 0)
        // {
        //     return;
        // }
        // //Debug.Log($"updateCount: {updateCount} _accumulatedElapsedGameTime: {_accumulatedElapsedGameTime.Ticks} {_accumulatedElapsedGameTime.TotalMilliseconds}");
        // _accumulatedElapsedGameTime = new TimeSpan(_accumulatedElapsedGameTime.Ticks - (updateCount * _targetElapsedTime.Ticks));
        // for (int i = 0; i < updateCount; i++)
        // {
        //     _tickContext.tick.TickCount++;
        //     Tick();
        // }
    }

    public void Tick()
    {
        for (int index = 0; index < this._tickSystems.Count; ++index)
            this._tickSystems[index].Tick();
    }
}