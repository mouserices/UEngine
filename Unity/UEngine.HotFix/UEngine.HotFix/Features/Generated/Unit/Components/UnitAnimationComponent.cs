//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    public AnimationComponent animation { get { return (AnimationComponent)GetComponent(UnitComponentsLookup.Animation); } }
    public bool hasAnimation { get { return HasComponent(UnitComponentsLookup.Animation); } }

    public void AddAnimation(string newAnimClipName, float newSpeed, System.Action newOnEnd) {
        var index = UnitComponentsLookup.Animation;
        var component = (AnimationComponent)CreateComponent(index, typeof(AnimationComponent));
        component.AnimClipName = newAnimClipName;
        component.Speed = newSpeed;
        component.OnEnd = newOnEnd;
        AddComponent(index, component);
    }

    public void ReplaceAnimation(string newAnimClipName, float newSpeed, System.Action newOnEnd) {
        var index = UnitComponentsLookup.Animation;
        var component = (AnimationComponent)CreateComponent(index, typeof(AnimationComponent));
        component.AnimClipName = newAnimClipName;
        component.Speed = newSpeed;
        component.OnEnd = newOnEnd;
        ReplaceComponent(index, component);
    }

    public void RemoveAnimation() {
        RemoveComponent(UnitComponentsLookup.Animation);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class UnitMatcher {

    static Entitas.IMatcher<UnitEntity> _matcherAnimation;

    public static Entitas.IMatcher<UnitEntity> Animation {
        get {
            if (_matcherAnimation == null) {
                var matcher = (Entitas.Matcher<UnitEntity>)Entitas.Matcher<UnitEntity>.AllOf(UnitComponentsLookup.Animation);
                matcher.componentNames = UnitComponentsLookup.componentNames;
                _matcherAnimation = matcher;
            }

            return _matcherAnimation;
        }
    }
}