using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class UIHpShowSystem : IExecuteSystem
{
 
    private IGroup<GameEntity> m_Group;
    private Dictionary<long, Slider> m_Sliders = new Dictionary<long, Slider>();
    private CameraService _CameraService;
    private UIService _UIService;
    public UIHpShowSystem(Contexts contexts, Services services)
    {
        m_Group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Numeric)
            .NoneOf(GameMatcher.MainPlayer, GameMatcher.MirrorTag));
        _CameraService = services.CameraService;
        _UIService = services.UIService;
    }

    public void Execute()
    {
        foreach (GameEntity entity in m_Group.GetEntities())
        {
            Slider slider = null;
            if (!m_Sliders.TryGetValue(entity.unit.ID, out slider))
            {
                var sliderGo = GameObject.Instantiate(Resources.Load<GameObject>("Slider"));
                sliderGo.transform.parent = _UIService.Canvas.transform;
                sliderGo.transform.localPosition = Vector3.zero;
                sliderGo.transform.localScale = Vector3.one;
                slider = sliderGo.GetComponent<Slider>();

                m_Sliders.Add(entity.unit.ID, slider);
            }

            //udpate pos
            var worldToScreenPoint =
                TransformUtility.WorldToScreenPoint(_CameraService.Camera, entity.position.value + Vector3.up * 2.2f);
            Vector2 localPos;
            TransformUtility.ScreenPointToLocalPointInRectangle(_UIService.Canvas.transform as RectTransform, worldToScreenPoint,
                _UIService.UICamera, out localPos);
            var sliderTransform = slider.transform as RectTransform;
            sliderTransform.anchoredPosition = localPos;
            
            //update slider
            slider.value = entity.GetNumeric(NumericType.HP) / entity.GetNumeric(NumericType.MAX_HP);
        }
    }
}