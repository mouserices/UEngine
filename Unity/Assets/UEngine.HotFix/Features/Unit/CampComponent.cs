using Entitas;
using Entitas.CodeGeneration.Attributes;


[Unit]
public class CampComponent : IComponent
{
    [EntityIndex] public CampType Value;
}