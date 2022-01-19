//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class NetworkEntity {

    public UEngine.Net.NetworkConnectedComponent networkConnected { get { return (UEngine.Net.NetworkConnectedComponent)GetComponent(NetworkComponentsLookup.NetworkConnected); } }
    public bool hasNetworkConnected { get { return HasComponent(NetworkComponentsLookup.NetworkConnected); } }

    public void AddNetworkConnected(int newConnectedID) {
        var index = NetworkComponentsLookup.NetworkConnected;
        var component = (UEngine.Net.NetworkConnectedComponent)CreateComponent(index, typeof(UEngine.Net.NetworkConnectedComponent));
        component.ConnectedID = newConnectedID;
        AddComponent(index, component);
    }

    public void ReplaceNetworkConnected(int newConnectedID) {
        var index = NetworkComponentsLookup.NetworkConnected;
        var component = (UEngine.Net.NetworkConnectedComponent)CreateComponent(index, typeof(UEngine.Net.NetworkConnectedComponent));
        component.ConnectedID = newConnectedID;
        ReplaceComponent(index, component);
    }

    public void RemoveNetworkConnected() {
        RemoveComponent(NetworkComponentsLookup.NetworkConnected);
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

    static Entitas.IMatcher<NetworkEntity> _matcherNetworkConnected;

    public static Entitas.IMatcher<NetworkEntity> NetworkConnected {
        get {
            if (_matcherNetworkConnected == null) {
                var matcher = (Entitas.Matcher<NetworkEntity>)Entitas.Matcher<NetworkEntity>.AllOf(NetworkComponentsLookup.NetworkConnected);
                matcher.componentNames = NetworkComponentsLookup.componentNames;
                _matcherNetworkConnected = matcher;
            }

            return _matcherNetworkConnected;
        }
    }
}
