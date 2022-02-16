namespace Suframa.Sciex.BusinessLogic
{
    public interface ICaptchaBll
    {
        bool IsValid(string token);
    }
}