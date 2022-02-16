namespace Suframa.Sciex.DataAccess.RestService
{
    public interface IAutenticacaoApi
    {
        bool Autenticar(string login, string lowerMD5);
    }
}