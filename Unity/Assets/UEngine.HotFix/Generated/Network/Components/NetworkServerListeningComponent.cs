//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class NetworkContext {

    public NetworkEntity serverListeningEntity { get { return GetGroup(NetworkMatcher.ServerListening).GetSingleEntity(); } }
    public UEngine.Net.ServerListeningComponent serverListening { get { return serverListeningEntity.serverListening; } }
    public bool hasServerListening { get { return serverListeningEntity != null; } }

    public NetworkEntity SetServerListening(int newPort) {
        if (hasServerListening) {
            throw new Entitas.EntitasException("Could not set ServerListening!\n" + this + " already has an entity with UEngine.Net.ServerListeningComponent!",
                "You should check if the context already has a serverListeningEntity before setting it or use context.ReplaceServerListening().");
        }
        var entity = CreateEntity();
        entity.AddServerListening(newPort);
        return entity;
    }

    public void ReplaceServerListening(int newPort) {
        var entity = serverListeningEntity;
        if (entity == null) {
            entity = SetServerListening(newPort);
        } else {
            entity.ReplaceServerListening(newPort);
        }
    }

    public void RemoveServerListening() {
        serverListeningEntity.Destroy();
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

    public UEngine.Net.ServerListeningComponent serverListening { get { return (UEngine.Net.ServerListeningComponent)GetComponent(NetworkComponentsLookup.ServerListening); } }
    public bool hasServerListening { get { return HasComponent(NetworkComponentsLookup.ServerListening); } }

    public void AddServerListening(int newPort) {
        var index = NetworkComponentsLookup.ServerListening;
        var component = (UEngine.Net.ServerListeningComponent)CreateComponent(index, typeof(UEngine.Net.ServerListeningComponent));
        component.Port = newPort;
        AddComponent(index, component);
    }

    public void ReplaceServerListening(int newPort) {
        var index = NetworkComponentsLookup.ServerListening;
        var component = (UEngine.Net.ServerListeningComponent)CreateComponent(index, typeof(UEngine.Net.ServerListeningComponent));
        component.Port = newPort;
        ReplaceComponent(index, component);
    }

    public void RemoveServerListening() {
        RemoveComponent(NetworkComponentsLookup.ServerListening);
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

    static Entitas.IMatcher<NetworkEntity> _matcherServerListening;

    public static Entitas.IMatcher<NetworkEntity> ServerListening {
        get {
            if (_matcherServerListening == null) {
                var matcher = (Entitas.Matcher<NetworkEntity>)Entitas.Matcher<NetworkEntity>.AllOf(NetworkComponentsLookup.ServerListening);
                matcher.componentNames = NetworkComponentsLookup.componentNames;
                _matcherServerListening = matcher;
            }

            return _matcherServerListening;
        }
    }
}
