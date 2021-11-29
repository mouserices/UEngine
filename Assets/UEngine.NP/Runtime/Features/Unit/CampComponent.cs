using Entitas;
using Entitas.CodeGeneration.Attributes;


[Game]
public class CampComponent : IComponent
{
    [EntityIndex] public CampType Value;
}