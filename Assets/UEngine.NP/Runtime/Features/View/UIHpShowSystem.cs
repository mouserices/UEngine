using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class UIHpShowSystem : IExecuteSystem
{
 
    private IGroup<GameEntity> m_Group;
    private Dictionary<long, Slider> m_Sliders = new Dictionary<long, Slider>();

    public UIHpShowSystem(Contexts contexts)
    {
        m_Group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Numeric).NoneOf(GameMatcher.MainPlayer,GameMatcher.MirrorTag));
    }

    public void Execute()
    {
        foreach (GameEntity entity in m_Group.GetEntities())
        {
            Slider slider = null;
            if (!m_Sliders.TryGetValue(entity.unit.ID, out slider))
            {
                var sliderGo = GameObject.Instantiate(Resources.Load<GameObject>("Slider"));
                sliderGo.transform.parent = Game.Canvas.transform;
                sliderGo.transform.localPosition = Vector3.zero;
                sliderGo.transform.localScale = Vector3.one;
                slider = sliderGo.GetComponent<Slider>();

                m_Sliders.Add(entity.unit.ID, slider);
            }

            //udpate pos
            var worldToScreenPoint =
                TransformUtility.WorldToScreenPoint(Game.MainCamera, entity.position.value + Vector3.up * 2.2f);
            Vector2 localPos;
            TransformUtility.ScreenPointToLocalPointInRectangle(Game.Canvas.transform as RectTransform, worldToScreenPoint,
                Game.UICamera, out localPos);
            var sliderTransform = slider.transform as RectTransform;
            sliderTransform.anchoredPosition = localPos;
            
            //update slider
            slider.value = entity.GetNumeric(NumericType.HP) / entity.GetNumeric(NumericType.MAX_HP);
        }
    }
}