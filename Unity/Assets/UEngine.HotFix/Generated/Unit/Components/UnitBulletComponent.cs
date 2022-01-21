//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UnitEntity {

    public BulletComponent bullet { get { return (BulletComponent)GetComponent(UnitComponentsLookup.Bullet); } }
    public bool hasBullet { get { return HasComponent(UnitComponentsLookup.Bullet); } }

    public void AddBullet(long newSourceUnitID, long newSourceSkillID) {
        var index = UnitComponentsLookup.Bullet;
        var component = (BulletComponent)CreateComponent(index, typeof(BulletComponent));
        component.SourceUnitID = newSourceUnitID;
        component.SourceSkillID = newSourceSkillID;
        AddComponent(index, component);
    }

    public void ReplaceBullet(long newSourceUnitID, long newSourceSkillID) {
        var index = UnitComponentsLookup.Bullet;
        var component = (BulletComponent)CreateComponent(index, typeof(BulletComponent));
        component.SourceUnitID = newSourceUnitID;
        component.SourceSkillID = newSourceSkillID;
        ReplaceComponent(index, component);
    }

    public void RemoveBullet() {
        RemoveComponent(UnitComponentsLookup.Bullet);
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
public sealed partial class UnitMatcher {

    static Entitas.IMatcher<UnitEntity> _matcherBullet;

    public static Entitas.IMatcher<UnitEntity> Bullet {
        get {
            if (_matcherBullet == null) {
                var matcher = (Entitas.Matcher<UnitEntity>)Entitas.Matcher<UnitEntity>.AllOf(UnitComponentsLookup.Bullet);
                matcher.componentNames = UnitComponentsLookup.componentNames;
                _matcherBullet = matcher;
            }

            return _matcherBullet;
        }
    }
}