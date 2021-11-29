using UnityEngine;

public class UIService : Service
{
    private Canvas _Canvas;

    public Canvas Canvas
    {
        get => _Canvas;
        set => _Canvas = value;
    }

    private Camera _UICamera;

    public Camera UICamera
    {
        get => _UICamera;
        set => _UICamera = value;
    }

    public UIService(Contexts contexts) : base(contexts)
    {
        
    }

    public void CreateUICanvas()
    {
        _Canvas = GameObject.Instantiate(Resources.Load<GameObject>("Canvas")).GetComponent<Canvas>();
        _UICamera = _Canvas.worldCamera;
    }
}