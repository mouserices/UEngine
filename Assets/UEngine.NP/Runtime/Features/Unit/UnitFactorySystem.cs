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
        var gameEntity = m_Contexts.game.CreateEntity();
  
        gameEntity.AddMainPlayer(1);
        gameEntity.AddUnit(1);
        gameEntity.AddAsset("eliteKnight");
        gameEntity.AddPosition(new Vector3(0,0,-15f));
        gameEntity.AddRotation(new Vector3(0,0,0));

        gameEntity.AddState(new LinkedList<FsmStateBase>());
        gameEntity.AddStateEnter(new IdleStateParam(){AnimClipName = "Idle",StateType = StateType.IDLE});
        
        gameEntity.AddSkillContainer(new List<Skill>());
        gameEntity.AddBehaveTreeLoad(new List<string>(){"Client_Skill01","Client_Skill02"});
        
        gameEntity.AddNumeric(new Dictionary<NumericType, float>());
        gameEntity.AddNumericModifier(new Dictionary<NumericType, List<BaseModifier>>());
        
        gameEntity.AddInputKey(new Dictionary<KeyCode, int>());
        gameEntity.isMirror = true;
        
        
        var monsterEntity = m_Contexts.game.CreateEntity();
        monsterEntity.AddUnit(2);
        monsterEntity.AddAsset("Aavatar101");
        monsterEntity.AddPosition(new Vector3(0,0,0));
        monsterEntity.AddRotation(new Vector3(0,180,0));
        monsterEntity.AddScale(new Vector3(1,1,1));
        monsterEntity.AddState(new LinkedList<FsmStateBase>());
        
        monsterEntity.AddNumeric(new Dictionary<NumericType, float>());
        monsterEntity.AddNumericModifier(new Dictionary<NumericType, List<BaseModifier>>());
        
        monsterEntity.AddStateEnter(new IdleStateParam(){AnimClipName = "Standing Idle",StateType = StateType.IDLE});
        monsterEntity.isMirror = true;
    }
}