//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.Roslyn.CodeGeneration.Plugins.CleanupSystemsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class GameCleanupSystems : Feature {

    public GameCleanupSystems(Contexts contexts) {
        Add(new DestroyDestroyedGameSystem(contexts));
        Add(new DestroyLoginMessageGameSystem(contexts));
    }
}
