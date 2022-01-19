using Entitas;
using NPBehave;
using Pathfinding;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class PatrolSystem : IInitializeSystem, IExecuteSystem
{
    private Contexts m_Contexts;
    private IGroup<UnitEntity> m_GroupReadyToPatrol;
    private IGroup<UnitEntity> m_GroupPatroling;

    public PatrolSystem(Contexts contexts)
    {
        m_Contexts = contexts;
    }

    public void Initialize()
    {
        m_GroupReadyToPatrol =
            m_Contexts.unit.GetGroup(UnitMatcher.AllOf(UnitMatcher.Patrol).NoneOf(UnitMatcher.PatrolPaths));
        m_GroupPatroling = m_Contexts.unit.GetGroup(UnitMatcher.AllOf(UnitMatcher.Patrol, UnitMatcher.PatrolPaths));
    }

    public void Execute()
    {
        foreach (UnitEntity entity in m_GroupReadyToPatrol.GetEntities())
        {
            InitPatrolData(entity);
        }
        
        foreach (UnitEntity entity in m_GroupPatroling.GetEntities())
        {
            DoPatrol(entity);
        }
    }

    private async void DoPatrol(UnitEntity entity)
    {
        Path path = entity.patrolPaths.Path;
        int curWayPoint = entity.patrolPaths.CurWayPoint;
        
        if (path == null)
        {
            return;
        }

        var distance = Vector3.Distance(entity.position.value, path.vectorPath[curWayPoint]);
        if (distance < 1)
        {
            if (curWayPoint + 1 < path.vectorPath.Count)
            {
                entity.patrolPaths.CurWayPoint++;
            }
            else
            {
                entity.patrolPaths.ReachedEndPoint = true;
            }
        }

        float speedFactor = entity.patrolPaths.ReachedEndPoint ? Mathf.Sqrt(distance / 1) : 1f;

        Vector3 moveDir = path.vectorPath[curWayPoint] - entity.position.value;
        moveDir = moveDir.normalized;

        var lookRotation = Quaternion.LookRotation(moveDir);
        entity.ReplaceRotation(lookRotation.eulerAngles);

        entity.ReplacePosition(entity.position.value + moveDir * entity.patrol.Speed  * speedFactor);
        if (entity.patrolPaths.ReachedEndPoint)
        {
            entity.RemovePatrolPaths();
        }
    }

    private void InitPatrolData(UnitEntity entity)
    {
        // Vector3 patrolCenter = entity.patrol.Center;
        // float patrolDistance = entity.patrol.Distance;
        //
        // var circlePoint = GetCirclePoint(patrolDistance);
        // var targetPoint = new Vector3(patrolCenter.x + circlePoint.x, patrolCenter.y, patrolCenter.z + circlePoint.y);
        //
        // var viewValue = entity.view.value as View;
        // var component = viewValue.GetComponent<Seeker>();
        // if (component == null)
        // {
        //     component = viewValue.gameObject.AddComponent<Seeker>();
        // }
        //
        // var simpleSmoothModifier = viewValue.GetComponent<SimpleSmoothModifier>();
        // if (simpleSmoothModifier == null)
        // {
        //     simpleSmoothModifier = viewValue.gameObject.AddComponent<SimpleSmoothModifier>();
        // }
        //
        // entity.isRequestPath = true;
        // component.StartPath(entity.position.value,targetPoint, (path) =>
        // {
        //     if (!path.error)
        //     {
        //         entity.isRequestPath = false;
        //         entity.AddPatrolPaths(path,0,false);
        //     }
        // });
    }

    System.Random random = new System.Random(1000);

    //半径随机 ，弧度随机
    public Vector2 GetCirclePoint(float m_Radius)
    {
        //随机获取弧度
        m_Radius = (float) GetRandomValue(0, m_Radius);
        float radin = (float) GetRandomValue(0, 2 * Mathf.PI);
        float x = m_Radius * Mathf.Cos(radin);
        float z = m_Radius * Mathf.Sin(radin);
        Vector2 endPoint = new Vector2(x, z);
        return endPoint;
    }

    public double GetRandomValue(double min, double max)
    {
        double v = random.NextDouble() * (max - min) + min;
        return v;
    }
}