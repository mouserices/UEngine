public class LoginSystems : Feature
{
    public  LoginSystems()
    {
        Add(new SessionSystem());
        Add(new LoginSucceedSystem());
        /*Add(new CreateUnitOnLoginSystem());
        Add(new SyncUnitsSystem());*/
    }
}