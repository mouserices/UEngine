using System.Collections.Generic;
using Entitas;

public class MirrorSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts m_Contexts;
    public MirrorSystem(Contexts contexts) : base(contexts.game)
    {
        m_Contexts = contexts;
    }
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Mirror);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isMirror;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity entity in entities)
        {
            var remoteAgentEntity = m_Contexts.game.CreateEntity();
            remoteAgentEntity.AddUnit(10000 + entity.unit.ID);
            remoteAgentEntity.isMirrorTag = true;
            //behave tree
            if (entity.hasBehaveTreeLoad)
            {
                remoteAgentEntity.AddSkillContainer(new List<Skill>());
                List<string> serverBehaveTreeNames = new List<string>();
                
                foreach (string behaveTreeName in entity.behaveTreeLoad.BehaveTreeNames)
                {
                   var serverBehaveTreeName = behaveTreeName.Replace("Client_", "Server_");
                   serverBehaveTreeNames.Add(serverBehaveTreeName);
                }
                remoteAgentEntity.AddBehaveTreeLoad(serverBehaveTreeNames);
            }
            
            //hp
            var numerics = entity.numeric.Numerics;
            var modifiers = entity.numericModifier.Modifiers;
            remoteAgentEntity.AddNumeric(new Dictionary<NumericType, float>(numerics));
            remoteAgentEntity.AddNumericModifier(new Dictionary<NumericType, List<BaseModifier>>(modifiers));
        }
    }
}