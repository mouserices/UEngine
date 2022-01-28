//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    public NumericModifierComponent numericModifier { get { return (NumericModifierComponent)GetComponent(UnitComponentsLookup.NumericModifier); } }
    public bool hasNumericModifier { get { return HasComponent(UnitComponentsLookup.NumericModifier); } }

    public void AddNumericModifier(System.Collections.Generic.Dictionary<NumericType, System.Collections.Generic.List<BaseModifier>> newModifiers) {
        var index = UnitComponentsLookup.NumericModifier;
        var component = (NumericModifierComponent)CreateComponent(index, typeof(NumericModifierComponent));
        component.Modifiers = newModifiers;
        AddComponent(index, component);
    }

    public void ReplaceNumericModifier(System.Collections.Generic.Dictionary<NumericType, System.Collections.Generic.List<BaseModifier>> newModifiers) {
        var index = UnitComponentsLookup.NumericModifier;
        var component = (NumericModifierComponent)CreateComponent(index, typeof(NumericModifierComponent));
        component.Modifiers = newModifiers;
        ReplaceComponent(index, component);
    }

    public void RemoveNumericModifier() {
        RemoveComponent(UnitComponentsLookup.NumericModifier);
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

    static Entitas.IMatcher<UnitEntity> _matcherNumericModifier;

    public static Entitas.IMatcher<UnitEntity> NumericModifier {
        get {
            if (_matcherNumericModifier == null) {
                var matcher = (Entitas.Matcher<UnitEntity>)Entitas.Matcher<UnitEntity>.AllOf(UnitComponentsLookup.NumericModifier);
                matcher.componentNames = UnitComponentsLookup.componentNames;
                _matcherNumericModifier = matcher;
            }

            return _matcherNumericModifier;
        }
    }
}