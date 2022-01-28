using Entitas;using UnityEngine.PlayerLoop;

public class TickAdaptationSystem : IInitializeSystem, IExecuteSystem
{
    private IGroup<RoomEntity> _group;
    private RoomContext _roomContext;
    public void Initialize()
    {
        _roomContext = Contexts.sharedInstance.room;
        _group = _roomContext.GetGroup(RoomMatcher.Tick);
    }
    
    public void Execute()
    {
        if (_group.count <= 0)
        {
            return;
        }
        
        
    }
}