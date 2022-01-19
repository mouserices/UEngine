using UnityEngine;

[Service]
public class UiWindowService : IUiWindowService
{
    private Canvas _canvas;
    private Camera _UICamera;
    public Canvas GetUiCanvas()
    {
        if (_canvas != null)
        {
            return _canvas;
        }
        throw new System.NotImplementedException();
    }

    public Camera GetUiCamera()
    {
        if (_canvas == null)
        {
            GetUiCanvas();
        }

        return _canvas.worldCamera;
    }
}