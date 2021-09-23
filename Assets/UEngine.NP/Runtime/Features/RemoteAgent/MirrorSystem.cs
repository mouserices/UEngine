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
            var remoteAgentEntity = m_Contexts.remoteAgent.CreateEntity();
            remoteAgentEntity.AddUnit(entity.unit.ID);
            
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
            remoteAgentEntity.AddHP(entity.hP.MaxHP,entity.hP.CurHP);
        }
    }
}