//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity mainPlayerDataEntity { get { return GetGroup(GameMatcher.MainPlayerData).GetSingleEntity(); } }
    public MainPlayerDataComponent mainPlayerData { get { return mainPlayerDataEntity.mainPlayerData; } }
    public bool hasMainPlayerData { get { return mainPlayerDataEntity != null; } }

    public GameEntity SetMainPlayerData(long newPlayerID, int newRoomID) {
        if (hasMainPlayerData) {
            throw new Entitas.EntitasException("Could not set MainPlayerData!\n" + this + " already has an entity with MainPlayerDataComponent!",
                "You should check if the context already has a mainPlayerDataEntity before setting it or use context.ReplaceMainPlayerData().");
        }
        var entity = CreateEntity();
        entity.AddMainPlayerData(newPlayerID, newRoomID);
        return entity;
    }

    public void ReplaceMainPlayerData(long newPlayerID, int newRoomID) {
        var entity = mainPlayerDataEntity;
        if (entity == null) {
            entity = SetMainPlayerData(newPlayerID, newRoomID);
        } else {
            entity.ReplaceMainPlayerData(newPlayerID, newRoomID);
        }
    }

    public void RemoveMainPlayerData() {
        mainPlayerDataEntity.Destroy();
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

    public MainPlayerDataComponent mainPlayerData { get { return (MainPlayerDataComponent)GetComponent(GameComponentsLookup.MainPlayerData); } }
    public bool hasMainPlayerData { get { return HasComponent(GameComponentsLookup.MainPlayerData); } }

    public void AddMainPlayerData(long newPlayerID, int newRoomID) {
        var index = GameComponentsLookup.MainPlayerData;
        var component = (MainPlayerDataComponent)CreateComponent(index, typeof(MainPlayerDataComponent));
        component.PlayerID = newPlayerID;
        component.RoomID = newRoomID;
        AddComponent(index, component);
    }

    public void ReplaceMainPlayerData(long newPlayerID, int newRoomID) {
        var index = GameComponentsLookup.MainPlayerData;
        var component = (MainPlayerDataComponent)CreateComponent(index, typeof(MainPlayerDataComponent));
        component.PlayerID = newPlayerID;
        component.RoomID = newRoomID;
        ReplaceComponent(index, component);
    }

    public void RemoveMainPlayerData() {
        RemoveComponent(GameComponentsLookup.MainPlayerData);
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

    static Entitas.IMatcher<GameEntity> _matcherMainPlayerData;

    public static Entitas.IMatcher<GameEntity> MainPlayerData {
        get {
            if (_matcherMainPlayerData == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MainPlayerData);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMainPlayerData = matcher;
            }

            return _matcherMainPlayerData;
        }
    }
}
