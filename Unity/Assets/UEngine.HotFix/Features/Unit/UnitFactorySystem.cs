using System.Collections.Generic;
using Entitas;
using NPBehave;
using UEngine.NP.Features.FsmState;
using UEngine.NP.FsmState;
using UnityEngine;


public class UnitFactorySystem : IInitializeSystem
{
    private Contexts m_Contexts;
    public UnitFactorySystem(Contexts contexts)
    {
        m_Contexts = contexts;
    }

    public void Initialize()
    {
        var gameEntity = m_Contexts.unit.CreateEntity();
  
        gameEntity.AddUnit(1);
#if CLIENT
        gameEntity.AddAsset("eliteKnight");
#endif
        gameEntity.AddPosition(new Vector3(0,0,-15f));
        gameEntity.AddRotation(new Vector3(0,0,0));

        gameEntity.AddState(new LinkedList<FsmStateBase>());
        gameEntity.AddStateEnter(new IdleStateParam(){AnimClipName = "Idle",StateType = StateType.IDLE});
        
        gameEntity.AddSkillContainer(new List<Skill>());
        //gameEntity.AddBehaveTreeLoad(new List<int>(){"Client_Skill01","Client_Skill02"});
        
        gameEntity.AddNumeric(new Dictionary<NumericType, float>());
        gameEntity.AddNumericModifier(new Dictionary<NumericType, List<BaseModifier>>());
        
        gameEntity.AddInputKey(new Dictionary<KeyCode, int>());
        gameEntity.AddCamp(CampType.Own);
        gameEntity.AddMove(false,Vector2.zero,0);
        
        
        var monsterEntity = m_Contexts.unit.CreateEntity();
        monsterEntity.AddUnit(2);
#if CLIENT
        monsterEntity.AddAsset("Aavatar101");
#endif
        
        monsterEntity.AddPosition(new Vector3(0,0,0));
        monsterEntity.AddRotation(new Vector3(0,180,0));
        monsterEntity.AddScale(new Vector3(1,1,1));
        monsterEntity.AddState(new LinkedList<FsmStateBase>());
        
        monsterEntity.AddNumeric(new Dictionary<NumericType, float>());
        monsterEntity.AddNumericModifier(new Dictionary<NumericType, List<BaseModifier>>());
        
        monsterEntity.AddStateEnter(new IdleStateParam(){AnimClipName = "Standing Idle",StateType = StateType.IDLE});
        monsterEntity.AddCamp(CampType.Enemy);

        monsterEntity.AddMove(false,Vector2.zero,0);
    }
}