using System.Collections.Generic;
using Entitas;
using TMPro;
using UnityEngine;

public class UIBloodStripSystem : ReactiveSystem<UnitEntity>
{
    private Contexts _Contexts;
    private UnityCameraService _CameraService;
    /*private UIService _UIService;*/
    public UIBloodStripSystem(Contexts contexts) : base(contexts.unit)
    {
        _Contexts = contexts;
        /*_CameraService = services.CameraService;
        _UIService = services.UIService;*/
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
    {
        return context.CreateCollector(UnitMatcher.Damage);
    }

    protected override bool Filter(UnitEntity entity)
    {
        return entity.hasDamage;
    }

    protected override void Execute(List<UnitEntity> entities)
    {
        foreach (UnitEntity damageEntity in entities)
        {
            /*var uInstantiate = GameObject.Instantiate(Resources.Load<GameObject>("UIDamage"));
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
            textMeshProUGUI.text = damageEntity.damage.Damage.ToString();*/
        }
    }
}