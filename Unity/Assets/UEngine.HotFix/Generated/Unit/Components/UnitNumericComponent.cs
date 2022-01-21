//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    public NumericComponent numeric { get { return (NumericComponent)GetComponent(UnitComponentsLookup.Numeric); } }
    public bool hasNumeric { get { return HasComponent(UnitComponentsLookup.Numeric); } }

    public void AddNumeric(System.Collections.Generic.Dictionary<NumericType, float> newNumerics) {
        var index = UnitComponentsLookup.Numeric;
        var component = (NumericComponent)CreateComponent(index, typeof(NumericComponent));
        component.Numerics = newNumerics;
        AddComponent(index, component);
    }

    public void ReplaceNumeric(System.Collections.Generic.Dictionary<NumericType, float> newNumerics) {
        var index = UnitComponentsLookup.Numeric;
        var component = (NumericComponent)CreateComponent(index, typeof(NumericComponent));
        component.Numerics = newNumerics;
        ReplaceComponent(index, component);
    }

    public void RemoveNumeric() {
        RemoveComponent(UnitComponentsLookup.Numeric);
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

    static Entitas.IMatcher<UnitEntity> _matcherNumeric;

    public static Entitas.IMatcher<UnitEntity> Numeric {
        get {
            if (_matcherNumeric == null) {
                var matcher = (Entitas.Matcher<UnitEntity>)Entitas.Matcher<UnitEntity>.AllOf(UnitComponentsLookup.Numeric);
                matcher.componentNames = UnitComponentsLookup.componentNames;
                _matcherNumeric = matcher;
            }

            return _matcherNumeric;
        }
    }
}