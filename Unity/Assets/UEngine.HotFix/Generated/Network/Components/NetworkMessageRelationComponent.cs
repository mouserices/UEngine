//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class NetworkContext {

    public NetworkEntity messageRelationEntity { get { return GetGroup(NetworkMatcher.MessageRelation).GetSingleEntity(); } }
    public UEngine.Net.MessageRelationComponent messageRelation { get { return messageRelationEntity.messageRelation; } }
    public bool hasMessageRelation { get { return messageRelationEntity != null; } }

    public NetworkEntity SetMessageRelation(System.Collections.Generic.Dictionary<int, System.Type> newOp2Types, System.Collections.Generic.Dictionary<System.Type, int> newType2Ops, System.Collections.Generic.Dictionary<System.Type, System.Type> newRequest2Response, System.Collections.Generic.Dictionary<System.Type, UEngine.Net.IMessageHandler> newRequest2Handlers) {
        if (hasMessageRelation) {
            throw new Entitas.EntitasException("Could not set MessageRelation!\n" + this + " already has an entity with UEngine.Net.MessageRelationComponent!",
                "You should check if the context already has a messageRelationEntity before setting it or use context.ReplaceMessageRelation().");
        }
        var entity = CreateEntity();
        entity.AddMessageRelation(newOp2Types, newType2Ops, newRequest2Response, newRequest2Handlers);
        return entity;
    }

    public void ReplaceMessageRelation(System.Collections.Generic.Dictionary<int, System.Type> newOp2Types, System.Collections.Generic.Dictionary<System.Type, int> newType2Ops, System.Collections.Generic.Dictionary<System.Type, System.Type> newRequest2Response, System.Collections.Generic.Dictionary<System.Type, UEngine.Net.IMessageHandler> newRequest2Handlers) {
        var entity = messageRelationEntity;
        if (entity == null) {
            entity = SetMessageRelation(newOp2Types, newType2Ops, newRequest2Response, newRequest2Handlers);
        } else {
            entity.ReplaceMessageRelation(newOp2Types, newType2Ops, newRequest2Response, newRequest2Handlers);
        }
    }

    public void RemoveMessageRelation() {
        messageRelationEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class NetworkEntity {

    public UEngine.Net.MessageRelationComponent messageRelation { get { return (UEngine.Net.MessageRelationComponent)GetComponent(NetworkComponentsLookup.MessageRelation); } }
    public bool hasMessageRelation { get { return HasComponent(NetworkComponentsLookup.MessageRelation); } }

    public void AddMessageRelation(System.Collections.Generic.Dictionary<int, System.Type> newOp2Types, System.Collections.Generic.Dictionary<System.Type, int> newType2Ops, System.Collections.Generic.Dictionary<System.Type, System.Type> newRequest2Response, System.Collections.Generic.Dictionary<System.Type, UEngine.Net.IMessageHandler> newRequest2Handlers) {
        var index = NetworkComponentsLookup.MessageRelation;
        var component = (UEngine.Net.MessageRelationComponent)CreateComponent(index, typeof(UEngine.Net.MessageRelationComponent));
        component.Op2Types = newOp2Types;
        component.Type2Ops = newType2Ops;
        component.Request2Response = newRequest2Response;
        component.Request2Handlers = newRequest2Handlers;
        AddComponent(index, component);
    }

    public void ReplaceMessageRelation(System.Collections.Generic.Dictionary<int, System.Type> newOp2Types, System.Collections.Generic.Dictionary<System.Type, int> newType2Ops, System.Collections.Generic.Dictionary<System.Type, System.Type> newRequest2Response, System.Collections.Generic.Dictionary<System.Type, UEngine.Net.IMessageHandler> newRequest2Handlers) {
        var index = NetworkComponentsLookup.MessageRelation;
        var component = (UEngine.Net.MessageRelationComponent)CreateComponent(index, typeof(UEngine.Net.MessageRelationComponent));
        component.Op2Types = newOp2Types;
        component.Type2Ops = newType2Ops;
        component.Request2Response = newRequest2Response;
        component.Request2Handlers = newRequest2Handlers;
        ReplaceComponent(index, component);
    }

    public void RemoveMessageRelation() {
        RemoveComponent(NetworkComponentsLookup.MessageRelation);
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
public sealed partial class NetworkMatcher {

    static Entitas.IMatcher<NetworkEntity> _matcherMessageRelation;

    public static Entitas.IMatcher<NetworkEntity> MessageRelation {
        get {
            if (_matcherMessageRelation == null) {
                var matcher = (Entitas.Matcher<NetworkEntity>)Entitas.Matcher<NetworkEntity>.AllOf(NetworkComponentsLookup.MessageRelation);
                matcher.componentNames = NetworkComponentsLookup.componentNames;
                _matcherMessageRelation = matcher;
            }

            return _matcherMessageRelation;
        }
    }
}