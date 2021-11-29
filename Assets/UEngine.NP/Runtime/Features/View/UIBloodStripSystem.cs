using System.Collections.Generic;
using Entitas;
using TMPro;
using UnityEngine;

public class UIBloodStripSystem : ReactiveSystem<GameEntity>
{
    private Contexts _Contexts;
    private CameraService _CameraService;
    private UIService _UIService;
    public UIBloodStripSystem(Contexts contexts, Services services) : base(contexts.game)
    {
        _Contexts = contexts;
        _CameraService = services.CameraService;
        _UIService = services.UIService;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Damage);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDamage;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity damageEntity in entities)
        {
            var uInstantiate = GameObject.Instantiate(Resources.Load<GameObject>("UIDamage"));
            uInstantiate.transform.parent = _UIService.Canvas.transform;
            uInstantiate.transform.localPosition = Vector3.zero;
            uInstantiate.transform.localScale = Vector3.one;
            
            //udpate pos

            var attackedEntity = _Contexts.game.GetEntityWithUnit(damageEntity.damage.TargetUnitID);

            var worldToScreenPoint =
                TransformUtility.WorldToScreenPoint(_CameraService.Camera, attackedEntity.position.value + Vector3.up * 2.2f);
            Vector2 localPos;
            TransformUtility.ScreenPointToLocalPointInRectangle(_UIService.Canvas.transform as RectTransform, worldToScreenPoint,
                _UIService.UICamera, out localPos);
            var sliderTransform = uInstantiate.transform as RectTransform;
            sliderTransform.anchoredPosition = localPos + Vector2.right * 150f;

            uInstantiate.AddComponent<UIBlood>();

            var textMeshProUGUI = uInstantiate.GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = damageEntity.damage.Damage.ToString();
        }
    }
}