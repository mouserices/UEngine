using Entitas;
//using StarterAssets;
using UnityEngine;

public class InitInputSystem : IInitializeSystem
{
    private UnityInputService _InputService;
    public InitInputSystem(Contexts contexts)
    {
        //_InputService = services.InputService;
    }

    public void Initialize()
    {
        //var i = GameObject.Instantiate(Resources.Load<GameObject>("Input"));
        //_InputService.StarterAssetsInputs = i.GetComponent<StarterAssetsInputs>();
    }
}