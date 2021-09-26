//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public NumericModifierComponent numericModifier { get { return (NumericModifierComponent)GetComponent(GameComponentsLookup.NumericModifier); } }
    public bool hasNumericModifier { get { return HasComponent(GameComponentsLookup.NumericModifier); } }

    public void AddNumericModifier(System.Collections.Generic.Dictionary<NumericType, System.Collections.Generic.List<BaseModifier>> newModifiers) {
        var index = GameComponentsLookup.NumericModifier;
        var component = (NumericModifierComponent)CreateComponent(index, typeof(NumericModifierComponent));
        component.Modifiers = newModifiers;
        AddComponent(index, component);
    }

    public void ReplaceNumericModifier(System.Collections.Generic.Dictionary<NumericType, System.Collections.Generic.List<BaseModifier>> newModifiers) {
        var index = GameComponentsLookup.NumericModifier;
        var component = (NumericModifierComponent)CreateComponent(index, typeof(NumericModifierComponent));
        component.Modifiers = newModifiers;
        ReplaceComponent(index, component);
    }

    public void RemoveNumericModifier() {
        RemoveComponent(GameComponentsLookup.NumericModifier);
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
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherNumericModifier;

    public static Entitas.IMatcher<GameEntity> NumericModifier {
        get {
            if (_matcherNumericModifier == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.NumericModifier);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherNumericModifier = matcher;
            }

            return _matcherNumericModifier;
        }
    }
}
