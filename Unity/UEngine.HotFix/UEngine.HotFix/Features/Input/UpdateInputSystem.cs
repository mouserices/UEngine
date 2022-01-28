using Entitas;
using UnityEngine;

public class UpdateInputSystem : IExecuteSystem
{
    private UnityInputService _InputService;
    public UpdateInputSystem(Contexts contexts)
    {
        //_InputService = services.InputService;
    }

    public void Execute()
    {
        // _InputService.IsMoving = _InputService.StarterAssetsInputs.move != Vector2.zero;
        // _InputService.Move = _InputService.StarterAssetsInputs.move;
    }
}