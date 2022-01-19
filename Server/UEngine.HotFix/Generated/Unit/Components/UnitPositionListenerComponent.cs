//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    public PositionListenerComponent positionListener { get { return (PositionListenerComponent)GetComponent(UnitComponentsLookup.PositionListener); } }
    public bool hasPositionListener { get { return HasComponent(UnitComponentsLookup.PositionListener); } }

    public void AddPositionListener(System.Collections.Generic.List<IPositionListener> newValue) {
        var index = UnitComponentsLookup.PositionListener;
        var component = (PositionListenerComponent)CreateComponent(index, typeof(PositionListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplacePositionListener(System.Collections.Generic.List<IPositionListener> newValue) {
        var index = UnitComponentsLookup.PositionListener;
        var component = (PositionListenerComponent)CreateComponent(index, typeof(PositionListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemovePositionListener() {
        RemoveComponent(UnitComponentsLookup.PositionListener);
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

    static Entitas.IMatcher<UnitEntity> _matcherPositionListener;

    public static Entitas.IMatcher<UnitEntity> PositionListener {
        get {
            if (_matcherPositionListener == null) {
                var matcher = (Entitas.Matcher<UnitEntity>)Entitas.Matcher<UnitEntity>.AllOf(UnitComponentsLookup.PositionListener);
                matcher.componentNames = UnitComponentsLookup.componentNames;
                _matcherPositionListener = matcher;
            }

            return _matcherPositionListener;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    public void AddPositionListener(IPositionListener value) {
        var listeners = hasPositionListener
            ? positionListener.value
            : new System.Collections.Generic.List<IPositionListener>();
        listeners.Add(value);
        ReplacePositionListener(listeners);
    }

    public void RemovePositionListener(IPositionListener value, bool removeComponentWhenEmpty = true) {
        var listeners = positionListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemovePositionListener();
        } else {
            ReplacePositionListener(listeners);
        }
    }
}
