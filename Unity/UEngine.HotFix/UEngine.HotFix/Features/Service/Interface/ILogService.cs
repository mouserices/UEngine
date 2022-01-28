public interface ILogService : IService
{
    void Log(string log);
    void LogError(string error);
}