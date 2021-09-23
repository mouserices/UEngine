using Entitas;
using Entitas.VisualDebugging.Unity;

[Game,DontDrawComponent]
public class PatrolPathsComponent : IComponent
{
    public Pathfinding.Path Path;
    public int CurWayPoint;
    public bool ReachedEndPoint;
}