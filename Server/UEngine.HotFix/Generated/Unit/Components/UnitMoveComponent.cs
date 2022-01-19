//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    public MoveComponent move { get { return (MoveComponent)GetComponent(UnitComponentsLookup.Move); } }
    public bool hasMove { get { return HasComponent(UnitComponentsLookup.Move); } }

    public void AddMove(bool newIsMoving, UnityEngine.Vector2 newMoveDir, float newMoveSpeed) {
        var index = UnitComponentsLookup.Move;
        var component = (MoveComponent)CreateComponent(index, typeof(MoveComponent));
        component.IsMoving = newIsMoving;
        component.MoveDir = newMoveDir;
        component.MoveSpeed = newMoveSpeed;
        AddComponent(index, component);
    }

    public void ReplaceMove(bool newIsMoving, UnityEngine.Vector2 newMoveDir, float newMoveSpeed) {
        var index = UnitComponentsLookup.Move;
        var component = (MoveComponent)CreateComponent(index, typeof(MoveComponent));
        component.IsMoving = newIsMoving;
        component.MoveDir = newMoveDir;
        component.MoveSpeed = newMoveSpeed;
        ReplaceComponent(index, component);
    }

    public void RemoveMove() {
        RemoveComponent(UnitComponentsLookup.Move);
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

    static Entitas.IMatcher<UnitEntity> _matcherMove;

    public static Entitas.IMatcher<UnitEntity> Move {
        get {
            if (_matcherMove == null) {
                var matcher = (Entitas.Matcher<UnitEntity>)Entitas.Matcher<UnitEntity>.AllOf(UnitComponentsLookup.Move);
                matcher.componentNames = UnitComponentsLookup.componentNames;
                _matcherMove = matcher;
            }

            return _matcherMove;
        }
    }
}
