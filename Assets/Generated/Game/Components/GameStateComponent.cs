//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public StateComponent state { get { return (StateComponent)GetComponent(GameComponentsLookup.State); } }
    public bool hasState { get { return HasComponent(GameComponentsLookup.State); } }

    public void AddState(System.Collections.Generic.LinkedList<UEngine.NP.FsmState.FsmStateBase> newFsmStateBases) {
        var index = GameComponentsLookup.State;
        var component = (StateComponent)CreateComponent(index, typeof(StateComponent));
        component.FsmStateBases = newFsmStateBases;
        AddComponent(index, component);
    }

    public void ReplaceState(System.Collections.Generic.LinkedList<UEngine.NP.FsmState.FsmStateBase> newFsmStateBases) {
        var index = GameComponentsLookup.State;
        var component = (StateComponent)CreateComponent(index, typeof(StateComponent));
        component.FsmStateBases = newFsmStateBases;
        ReplaceComponent(index, component);
    }

    public void RemoveState() {
        RemoveComponent(GameComponentsLookup.State);
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

    static Entitas.IMatcher<GameEntity> _matcherState;

    public static Entitas.IMatcher<GameEntity> State {
        get {
            if (_matcherState == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.State);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherState = matcher;
            }

            return _matcherState;
        }
    }
}
