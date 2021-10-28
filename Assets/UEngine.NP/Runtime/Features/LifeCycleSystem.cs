using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class LifeCycleSystem : IExecuteSystem
{
    private IGroup<GameEntity> m_Group;
    public LifeCycleSystem(Contexts contexts)
    {
        m_Group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Life).NoneOf(GameMatcher.Destroyed));
    }

    public void Execute()
    {
        if (m_Group.count > 0 )
        {
            foreach (GameEntity entity in m_Group.GetEntities())
            {
                if (entity.life.PastTime >= entity.life.TotalTime)
                {
                    entity.isDestroyed = true;
                    Debug.Log($"isDestroyed = true {entity.name.Value} frameCount: {Time.frameCount}");
                    continue;
                }

                entity.life.PastTime += Time.deltaTime;
            }
        }
    }
}