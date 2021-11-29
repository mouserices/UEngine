using Entitas;
using UnityEngine;

public class UpdateInputSystem : IExecuteSystem
{
    private InputService _InputService;
    public UpdateInputSystem(Contexts contexts, Services services)
    {
        _InputService = services.InputService;
    }

    public void Execute()
    {
        _InputService.IsMoving = _InputService.StarterAssetsInputs.move != Vector2.zero;
        _InputService.Move = _InputService.StarterAssetsInputs.move;
    }
}