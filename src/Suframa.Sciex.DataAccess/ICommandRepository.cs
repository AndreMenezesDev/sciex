using Suframa.Sciex.DataAccess.Database;

namespace Suframa.Sciex.DataAccess
{
    public interface ICommandRepository<T> where T : BaseEntity
    {
        void Apagar(int id);

        void ApagarAssinc(int id);

        void Salvar(T entity);
    }
}