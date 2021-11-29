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

public sealed class DestroyMessageDispacherNetMessageSystem : ICleanupSystem {

    readonly IGroup<NetMessageEntity> _group;
    readonly List<NetMessageEntity> _buffer = new List<NetMessageEntity>();

    public DestroyMessageDispacherNetMessageSystem(Contexts contexts) {
        _group = contexts.netMessage.GetGroup(NetMessageMatcher.MessageDispacher);
    }

    public void Cleanup() {
        foreach (var e in _group.GetEntities(_buffer)) {
            e.Destroy();
        }
    }
}
