using Entitas;

public class UnitIdGenerater
{
    private static int _id;
    public static int GetID()
    {
        return ++_id;
    }
}