using Entitas;
using UnityEngine;

public class GameInitSystem : IInitializeSystem
{
    public GameInitSystem(Contexts contexts)
    {
    }

    public void Initialize()
    {
        Game.Canvas = GameObject.Instantiate(Resources.Load<GameObject>("Canvas")).GetComponent<Canvas>();
        Game.UICamera = Game.Canvas.worldCamera;
        Game.MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
}