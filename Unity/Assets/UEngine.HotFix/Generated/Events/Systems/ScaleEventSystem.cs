//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventSystemGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class ScaleEventSystem : Entitas.ReactiveSystem<UnitEntity> {

    readonly System.Collections.Generic.List<IScaleListener> _listenerBuffer;

    public ScaleEventSystem(Contexts contexts) : base(contexts.unit) {
        _listenerBuffer = new System.Collections.Generic.List<IScaleListener>();
    }

    protected override Entitas.ICollector<UnitEntity> GetTrigger(Entitas.IContext<UnitEntity> context) {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(UnitMatcher.Scale)
        );
    }

    protected override bool Filter(UnitEntity entity) {
        return entity.hasScale && entity.hasScaleListener;
    }

    protected override void Execute(System.Collections.Generic.List<UnitEntity> entities) {
        foreach (var e in entities) {
            var component = e.scale;
            _listenerBuffer.Clear();
            _listenerBuffer.AddRange(e.scaleListener.value);
            foreach (var listener in _listenerBuffer) {
                listener.OnScale(e, component.Value);
            }
        }
    }
}
