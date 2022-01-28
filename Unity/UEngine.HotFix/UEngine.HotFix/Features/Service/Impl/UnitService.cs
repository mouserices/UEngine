using System.Collections.Generic;
using UEngine.NP.FsmState;
using UnityEngine;

[Service]
public class UnitService : IUnitService
{
    public UnitEntity CreateUnit(int id, long playerID, int connectionId, string asset, Vector3 pos, Vector3 rot,
        StateParam stateParam,
        List<int> skills, CampType campType)
    {
        var gameEntity = Contexts.sharedInstance.unit.CreateEntity();

        gameEntity.AddUnit(id);
        gameEntity.AddPlayerID(playerID);
#if CLIENT
        if (playerID == Contexts.sharedInstance.game.mainPlayerData.PlayerID)
        {
            gameEntity.isMainPlayer = true;
        }
        gameEntity.AddAsset(asset);
#elif SERVER
        gameEntity.AddConnectionID(connectionId);
#endif
        gameEntity.AddPosition(pos);
        gameEntity.AddRotation(rot);

        gameEntity.AddState(new LinkedList<FsmStateBase>());
        gameEntity.AddStateEnter(stateParam);

        gameEntity.AddSkillContainer(new List<Skill>());
        gameEntity.AddBehaveTreeLoad(skills);

        gameEntity.AddNumeric(new Dictionary<NumericType, float>());
        gameEntity.AddNumericModifier(new Dictionary<NumericType, List<BaseModifier>>());

        gameEntity.AddInputKey(new Dictionary<KeyCode, int>());
        gameEntity.AddCamp(campType);
        gameEntity.AddMove(false, Vector2.zero, 0);
        return gameEntity;
    }
}