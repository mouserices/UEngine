using Entitas;

public class UIInitSystem : IInitializeSystem
{
    private UIService _UIService;
    public UIInitSystem(Contexts contexts, Services services)
    {
        _UIService = services.UIService;
    }

    public void Initialize()
    {
        _UIService.CreateUICanvas();
    }
}