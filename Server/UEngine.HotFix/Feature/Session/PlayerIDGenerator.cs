public class PlayerIDGenerator
{
    private static int _id;

    public static int GetID()
    {
        return ++_id;
    }
}