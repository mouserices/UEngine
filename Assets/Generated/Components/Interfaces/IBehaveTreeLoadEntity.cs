//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial interface IBehaveTreeLoadEntity {

    BehaveTreeLoadComponent behaveTreeLoad { get; }
    bool hasBehaveTreeLoad { get; }

    void AddBehaveTreeLoad(System.Collections.Generic.List<string> newBehaveTreeNames);
    void ReplaceBehaveTreeLoad(System.Collections.Generic.List<string> newBehaveTreeNames);
    void RemoveBehaveTreeLoad();
}