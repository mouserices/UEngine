//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity mainPlayerEntity { get { return GetGroup(GameMatcher.MainPlayer).GetSingleEntity(); } }
    public MainPlayerComponent mainPlayer { get { return mainPlayerEntity.mainPlayer; } }
    public bool hasMainPlayer { get { return mainPlayerEntity != null; } }

    public GameEntity SetMainPlayer(long newID) {
        if (hasMainPlayer) {
            throw new Entitas.EntitasException("Could not set MainPlayer!\n" + this + " already has an entity with MainPlayerComponent!",
                "You should check if the context already has a mainPlayerEntity before setting it or use context.ReplaceMainPlayer().");
        }
        var entity = CreateEntity();
        entity.AddMainPlayer(newID);
        return entity;
    }

    public void ReplaceMainPlayer(long newID) {
        var entity = mainPlayerEntity;
        if (entity == null) {
            entity = SetMainPlayer(newID);
        } else {
            entity.ReplaceMainPlayer(newID);
        }
    }

    public void RemoveMainPlayer() {
        mainPlayerEntity.Destroy();
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

    public MainPlayerComponent mainPlayer { get { return (MainPlayerComponent)GetComponent(GameComponentsLookup.MainPlayer); } }
    public bool hasMainPlayer { get { return HasComponent(GameComponentsLookup.MainPlayer); } }

    public void AddMainPlayer(long newID) {
        var index = GameComponentsLookup.MainPlayer;
        var component = (MainPlayerComponent)CreateComponent(index, typeof(MainPlayerComponent));
        component.ID = newID;
        AddComponent(index, component);
    }

    public void ReplaceMainPlayer(long newID) {
        var index = GameComponentsLookup.MainPlayer;
        var component = (MainPlayerComponent)CreateComponent(index, typeof(MainPlayerComponent));
        component.ID = newID;
        ReplaceComponent(index, component);
    }

    public void RemoveMainPlayer() {
        RemoveComponent(GameComponentsLookup.MainPlayer);
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

    static Entitas.IMatcher<GameEntity> _matcherMainPlayer;

    public static Entitas.IMatcher<GameEntity> MainPlayer {
        get {
            if (_matcherMainPlayer == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MainPlayer);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMainPlayer = matcher;
            }

            return _matcherMainPlayer;
        }
    }
}