//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventSystemGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class BoxColliderEventSystem : Entitas.ReactiveSystem<UnitEntity> {

    readonly System.Collections.Generic.List<IBoxColliderListener> _listenerBuffer;

    public BoxColliderEventSystem(Contexts contexts) : base(contexts.unit) {
        _listenerBuffer = new System.Collections.Generic.List<IBoxColliderListener>();
    }

    protected override Entitas.ICollector<UnitEntity> GetTrigger(Entitas.IContext<UnitEntity> context) {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(UnitMatcher.BoxCollider)
        );
    }

    protected override bool Filter(UnitEntity entity) {
        return entity.isBoxCollider && entity.hasBoxColliderListener;
    }

    protected override void Execute(System.Collections.Generic.List<UnitEntity> entities) {
        foreach (var e in entities) {
            
            _listenerBuffer.Clear();
            _listenerBuffer.AddRange(e.boxColliderListener.value);
            foreach (var listener in _listenerBuffer) {
                listener.OnBoxCollider(e);
            }
        }
    }
}