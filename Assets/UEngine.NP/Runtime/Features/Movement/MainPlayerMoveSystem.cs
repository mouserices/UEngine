using Entitas;

public class MainPlayerMoveSystem : IExecuteSystem
{
    private InputService _InputService;
    private Contexts _Contexts;
    public MainPlayerMoveSystem(Contexts contexts, Services services)
    {
        _Contexts = contexts;
        _InputService = services.InputService;
    }

    public void Execute()
    {
        var mainPlayerEntity = _Contexts.game.mainPlayerEntity;
        if (mainPlayerEntity == null)
        {
            return;
        }
        
        //mainPlayerEntity.ReplaceMove(_InputService.IsMoving,_InputService.Move,2f);
    }
}