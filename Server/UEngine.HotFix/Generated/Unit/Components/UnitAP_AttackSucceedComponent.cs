//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    static readonly AP_AttackSucceedComponent aP_AttackSucceedComponent = new AP_AttackSucceedComponent();

    public bool isAP_AttackSucceed {
        get { return HasComponent(UnitComponentsLookup.AP_AttackSucceed); }
        set {
            if (value != isAP_AttackSucceed) {
                var index = UnitComponentsLookup.AP_AttackSucceed;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : aP_AttackSucceedComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
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

    static Entitas.IMatcher<UnitEntity> _matcherAP_AttackSucceed;

    public static Entitas.IMatcher<UnitEntity> AP_AttackSucceed {
        get {
            if (_matcherAP_AttackSucceed == null) {
                var matcher = (Entitas.Matcher<UnitEntity>)Entitas.Matcher<UnitEntity>.AllOf(UnitComponentsLookup.AP_AttackSucceed);
                matcher.componentNames = UnitComponentsLookup.componentNames;
                _matcherAP_AttackSucceed = matcher;
            }

            return _matcherAP_AttackSucceed;
        }
    }
}
