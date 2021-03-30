namespace AutoAttend.Interface
{
    public interface IErrorHandler
    {
        void LogErrorAndDelay(string error, int delaySecond = 3000);
        void HandleError();
        bool CheckError();
    }
}