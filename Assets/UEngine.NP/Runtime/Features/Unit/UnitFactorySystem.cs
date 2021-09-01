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
        //gameEntity.AddPosition2(2,1,1);
        gameEntity.AddMainPlayer(1);
        gameEntity.AddUnit(1);
        gameEntity.AddAsset("Avatar17");
        gameEntity.AddPosition(new Vector3(44,3,64));
        gameEntity.AddRotation(new Vector3(0,0,0));

        gameEntity.AddState(new LinkedList<FsmStateBase>());
        gameEntity.AddStateEnter(new IdleStateParam(){AnimClipName = "Avatar17_Idle"});
        
        gameEntity.AddBehaveTree(new List<Root>());
        gameEntity.AddBehaveTreeLoad(new List<string>(){"Client_Skill01"});
        
        
        var monsterEntity = m_Contexts.game.CreateEntity();
        monsterEntity.AddUnit(2);
        monsterEntity.AddAsset("Avatar93");
        monsterEntity.AddPosition(new Vector3(39,3,68));
        monsterEntity.AddRotation(new Vector3(0,137f,0));
        monsterEntity.AddState(new LinkedList<FsmStateBase>());
        monsterEntity.AddStateEnter(new IdleStateParam(){AnimClipName = "Avatar93_idle"});
    }
}