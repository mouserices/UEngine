//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.Roslyn.CodeGeneration.Plugins.CleanupSystemGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Entitas;

public sealed class RemoveServerListeningNetworkSystem : ICleanupSystem {

    readonly IGroup<NetworkEntity> _group;
    readonly List<NetworkEntity> _buffer = new List<NetworkEntity>();

    public RemoveServerListeningNetworkSystem(Contexts contexts) {
        _group = contexts.network.GetGroup(NetworkMatcher.ServerListening);
    }

    public void Cleanup() {
        foreach (var e in _group.GetEntities(_buffer)) {
            e.RemoveServerListening();
        }
    }
}