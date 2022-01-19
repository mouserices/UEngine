using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;
using System.Linq;
using System.Reflection;

[Title("Buff效果", TitleAlignment = TitleAlignments.Centered)]
public class NP_BuffStatusAction : NP_BaseAction
{
    [LabelText("状态ID"), DelayedProperty] public string ID = "Status1";
    [LabelText("状态名称"), DelayedProperty] public string Name = "状态1";

    public StatusType StatusType;
    [HideInInspector] public uint Duration;

    [LabelText("是否在状态栏显示"), UnityEngine.Serialization.FormerlySerializedAs("ShowInStatusIconList")]
    public bool ShowInStatusSlots;

    [LabelText("能否叠加")] public bool CanStack;

    [LabelText("最高叠加层数"), ShowIf("CanStack"), Range(0, 99)]
    public int MaxStack = 0;

    // [LabelText("子状态效果")] public bool EnableChildrenStatuses;
    //
    // [OnInspectorGUI("DrawSpace", append: true)]
    // [HideReferenceObjectPicker]
    // [LabelText("子状态效果列表"), ShowIf("EnableChildrenStatuses"),
    //  ListDrawerSettings(DraggableItems = false, ShowItemCount = false, CustomAddFunction = "AddChildStatus")]
    // public List<ChildStatus> ChildrenStatuses = new List<ChildStatus>();
    //
    // private void AddChildStatus()
    // {
    //     ChildrenStatuses.Add(new ChildStatus());
    // }

    [ToggleGroup("EnabledStateModify", "行为禁制")]
    public bool EnabledStateModify;

    [ToggleGroup("EnabledStateModify")] public ActionControlType ActionControlType;

    [ToggleGroup("EnabledAttributeModify", "属性修饰")]
    public bool EnabledAttributeModify;

    [ToggleGroup("EnabledAttributeModify")]
    public AttributeType AttributeType;

    [ToggleGroup("EnabledAttributeModify"), LabelText("数值参数")]
    public string NumericValue;

    public string NumericValueProperty { get; set; }

    [ToggleGroup("EnabledAttributeModify")]
    public ModifyType ModifyType;
    //[ToggleGroup("EnabledAttributeModify"), LabelText("属性修饰")]
    //[DictionaryDrawerSettings(KeyLabel =)]
    //public Dictionary<NumericType, string> AttributeChanges = new Dictionary<NumericType, string>();

    [ToggleGroup("EnabledLogicTrigger", "逻辑触发")]
    public bool EnabledLogicTrigger;

    [ToggleGroup("EnabledLogicTrigger")]
    [LabelText("效果列表") /*, Space(30)*/]
    [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowItemCount = false, HideAddButton = true)]
    [HideReferenceObjectPicker]
    public List<Effect> Effects = new List<Effect>();

    [HorizontalGroup("EnabledLogicTrigger/Hor2", PaddingLeft = 40, PaddingRight = 40)]
    [HideLabel]
    [OnValueChanged("AddEffect")]
    [ValueDropdown("EffectTypeSelect")]
    public string EffectTypeName = "(添加效果)";
    
    public IEnumerable<string> EffectTypeSelect()
    {
        var types = typeof(Effect).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => typeof(Effect).IsAssignableFrom(x))
            //.Where(x => x != typeof(AttributeNumericModifyEffect))
            .Where(x => x.GetCustomAttribute<EffectAttribute>() != null)
            .OrderBy(x => x.GetCustomAttribute<EffectAttribute>().Order)
            .Select(x => x.GetCustomAttribute<EffectAttribute>().EffectType);

        //var status = AssetDatabase.FindAssets("t:StatusConfigObject", new string[] { "Assets" })
        //    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
        //    .Select(path => AssetDatabase.LoadAssetAtPath<StatusConfigObject>(path).Name)
        //    .Select(name => $"施加状态效果 [{name}]");

        var results = types.ToList();
        //results.AddRange(status);
        results.Insert(0, "(添加效果)");
        return results;
    }
    
    private void AddEffect()
    {
        if (EffectTypeName != "(添加效果)")
        {
            //if (EffectTypeName.Contains("施加状态效果 ["))
            //{
            //    var effect = Activator.CreateInstance<AddStatusEffect>() as Effect;
            //    effect.Enabled = true;
            //    if (effect is AddStatusEffect addStatusEffect)
            //    {
            //        var status = AssetDatabase.FindAssets("t:StatusConfigObject", new string[] { "Assets" })
            //            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            //            .Select(path => AssetDatabase.LoadAssetAtPath<StatusConfigObject>(path).Name)
            //            .Select(name => $"施加状态效果 [{name}]")
            //            .Where(name => name == $"施加状态效果 [{name}]");
            //        //addStatusEffect.AddStatus = AssetDatabase.load
            //    }
            //    Effects.Add(effect);
            //}
            //else
            {
                var effectType = typeof(Effect).Assembly.GetTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => typeof(Effect).IsAssignableFrom(x))
                    .Where(x => x.GetCustomAttribute<EffectAttribute>() != null)
                    .Where(x => x.GetCustomAttribute<EffectAttribute>().EffectType == EffectTypeName)
                    .First();
                var effect = Activator.CreateInstance(effectType) as Effect;
                effect.Enabled = true;
                Effects.Add(effect);
            }

            EffectTypeName = "(添加效果)";
        }
        //SkillHelper.AddEffect(Effects, EffectType);
    }
}

// public class ChildStatus
// {
//     [LabelText("状态效果")] public StatusConfigObject StatusConfigObject;
//
//     [LabelText("参数列表"), HideReferenceObjectPicker]
//     public Dictionary<string, string> Params = new Dictionary<string, string>();
// }

public enum StatusType
{
    [LabelText("Buff(增益)")] Buff,
    [LabelText("Debuff(减益)")] Debuff,
    [LabelText("其他")] Other,
}

public enum EffectTriggerType
{
    [LabelText("立即触发")] Instant = 0,
    [LabelText("条件触发")] Condition = 1,
    [LabelText("行动点触发")] Action = 2,
    [LabelText("间隔触发")] Interval = 3,
    [LabelText("在行动点且满足条件")] ActionCondition = 4,
}
