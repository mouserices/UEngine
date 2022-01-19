//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    public HPComponent hP { get { return (HPComponent)GetComponent(UnitComponentsLookup.HP); } }
    public bool hasHP { get { return HasComponent(UnitComponentsLookup.HP); } }

    public void AddHP(float newMaxHP, float newCurHP) {
        var index = UnitComponentsLookup.HP;
        var component = (HPComponent)CreateComponent(index, typeof(HPComponent));
        component.MaxHP = newMaxHP;
        component.CurHP = newCurHP;
        AddComponent(index, component);
    }

    public void ReplaceHP(float newMaxHP, float newCurHP) {
        var index = UnitComponentsLookup.HP;
        var component = (HPComponent)CreateComponent(index, typeof(HPComponent));
        component.MaxHP = newMaxHP;
        component.CurHP = newCurHP;
        ReplaceComponent(index, component);
    }

    public void RemoveHP() {
        RemoveComponent(UnitComponentsLookup.HP);
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

    static Entitas.IMatcher<UnitEntity> _matcherHP;

    public static Entitas.IMatcher<UnitEntity> HP {
        get {
            if (_matcherHP == null) {
                var matcher = (Entitas.Matcher<UnitEntity>)Entitas.Matcher<UnitEntity>.AllOf(UnitComponentsLookup.HP);
                matcher.componentNames = UnitComponentsLookup.componentNames;
                _matcherHP = matcher;
            }

            return _matcherHP;
        }
    }
}
