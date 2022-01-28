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

public sealed class RemoveMirrorUnitSystem : ICleanupSystem {

    readonly IGroup<UnitEntity> _group;
    readonly List<UnitEntity> _buffer = new List<UnitEntity>();

    public RemoveMirrorUnitSystem(Contexts contexts) {
        _group = contexts.unit.GetGroup(UnitMatcher.Mirror);
    }

    public void Cleanup() {
        foreach (var e in _group.GetEntities(_buffer)) {
            e.isMirror = false;
        }
    }
}