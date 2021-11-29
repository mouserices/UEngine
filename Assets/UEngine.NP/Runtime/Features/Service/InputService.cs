using StarterAssets;
using UnityEngine;

public class InputService : Service
{
    private StarterAssetsInputs _StarterAssetsInputs;

    public StarterAssetsInputs StarterAssetsInputs
    {
        get => _StarterAssetsInputs;
        set => _StarterAssetsInputs = value;
    }

    private bool _IsMoving;

    public bool IsMoving
    {
        get => _IsMoving;
        set => _IsMoving = value;
    }

    public Vector2 Move
    {
        get => _Move;
        set => _Move = value;
    }

    private Vector2 _Move;

    public InputService(Contexts contexts) : base(contexts)
    {
    }
    
    
}