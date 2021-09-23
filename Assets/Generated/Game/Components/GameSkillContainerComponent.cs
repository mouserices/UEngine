//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public SkillContainerComponent skillContainer { get { return (SkillContainerComponent)GetComponent(GameComponentsLookup.SkillContainer); } }
    public bool hasSkillContainer { get { return HasComponent(GameComponentsLookup.SkillContainer); } }

    public void AddSkillContainer(System.Collections.Generic.List<Skill> newSkills) {
        var index = GameComponentsLookup.SkillContainer;
        var component = (SkillContainerComponent)CreateComponent(index, typeof(SkillContainerComponent));
        component.Skills = newSkills;
        AddComponent(index, component);
    }

    public void ReplaceSkillContainer(System.Collections.Generic.List<Skill> newSkills) {
        var index = GameComponentsLookup.SkillContainer;
        var component = (SkillContainerComponent)CreateComponent(index, typeof(SkillContainerComponent));
        component.Skills = newSkills;
        ReplaceComponent(index, component);
    }

    public void RemoveSkillContainer() {
        RemoveComponent(GameComponentsLookup.SkillContainer);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity : ISkillContainerEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherSkillContainer;

    public static Entitas.IMatcher<GameEntity> SkillContainer {
        get {
            if (_matcherSkillContainer == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.SkillContainer);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherSkillContainer = matcher;
            }

            return _matcherSkillContainer;
        }
    }
}