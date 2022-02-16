namespace Suframa.Sciex.DataAccess.LdapService
{
    public interface IAutenticacaoLdap
    {
        bool Autenticar(string usuario, string senha);
    }
}