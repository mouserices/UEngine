using System;
using UEngine.NP;

[BsonDeserializerRegister]
public abstract class NP_BaseService
{
    public Skill Skill { get; set; }
    public virtual float GetInterval()
    {
        return 0.125f;
    }

    public abstract Action GetServiceAction();
}