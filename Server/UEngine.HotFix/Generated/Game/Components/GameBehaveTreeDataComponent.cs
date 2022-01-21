//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity behaveTreeDataEntity { get { return GetGroup(GameMatcher.BehaveTreeData).GetSingleEntity(); } }
    public BehaveTreeDataComponent behaveTreeData { get { return behaveTreeDataEntity.behaveTreeData; } }
    public bool hasBehaveTreeData { get { return behaveTreeDataEntity != null; } }

    public GameEntity SetBehaveTreeData(System.Collections.Generic.Dictionary<int, UEngine.NP.NP_DataSupportorBase> newNameToBehaveTreeDatas) {
        if (hasBehaveTreeData) {
            throw new Entitas.EntitasException("Could not set BehaveTreeData!\n" + this + " already has an entity with BehaveTreeDataComponent!",
                "You should check if the context already has a behaveTreeDataEntity before setting it or use context.ReplaceBehaveTreeData().");
        }
        var entity = CreateEntity();
        entity.AddBehaveTreeData(newNameToBehaveTreeDatas);
        return entity;
    }

    public void ReplaceBehaveTreeData(System.Collections.Generic.Dictionary<int, UEngine.NP.NP_DataSupportorBase> newNameToBehaveTreeDatas) {
        var entity = behaveTreeDataEntity;
        if (entity == null) {
            entity = SetBehaveTreeData(newNameToBehaveTreeDatas);
        } else {
            entity.ReplaceBehaveTreeData(newNameToBehaveTreeDatas);
        }
    }

    public void RemoveBehaveTreeData() {
        behaveTreeDataEntity.Destroy();
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
public partial class GameEntity {

    public BehaveTreeDataComponent behaveTreeData { get { return (BehaveTreeDataComponent)GetComponent(GameComponentsLookup.BehaveTreeData); } }
    public bool hasBehaveTreeData { get { return HasComponent(GameComponentsLookup.BehaveTreeData); } }

    public void AddBehaveTreeData(System.Collections.Generic.Dictionary<int, UEngine.NP.NP_DataSupportorBase> newNameToBehaveTreeDatas) {
        var index = GameComponentsLookup.BehaveTreeData;
        var component = (BehaveTreeDataComponent)CreateComponent(index, typeof(BehaveTreeDataComponent));
        component.NameToBehaveTreeDatas = newNameToBehaveTreeDatas;
        AddComponent(index, component);
    }

    public void ReplaceBehaveTreeData(System.Collections.Generic.Dictionary<int, UEngine.NP.NP_DataSupportorBase> newNameToBehaveTreeDatas) {
        var index = GameComponentsLookup.BehaveTreeData;
        var component = (BehaveTreeDataComponent)CreateComponent(index, typeof(BehaveTreeDataComponent));
        component.NameToBehaveTreeDatas = newNameToBehaveTreeDatas;
        ReplaceComponent(index, component);
    }

    public void RemoveBehaveTreeData() {
        RemoveComponent(GameComponentsLookup.BehaveTreeData);
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
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherBehaveTreeData;

    public static Entitas.IMatcher<GameEntity> BehaveTreeData {
        get {
            if (_matcherBehaveTreeData == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.BehaveTreeData);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherBehaveTreeData = matcher;
            }

            return _matcherBehaveTreeData;
        }
    }
}