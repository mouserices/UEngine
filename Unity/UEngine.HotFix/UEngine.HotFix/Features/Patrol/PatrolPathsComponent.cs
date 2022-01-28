using Entitas;
using Entitas.VisualDebugging.Unity;

[Unit,DontDrawComponent]
public class PatrolPathsComponent : IComponent
{
    // public Pathfinding.Path Path;
    public int CurWayPoint;
    public bool ReachedEndPoint;
}