using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Suframa.Sciex.DataAccess.Database
{
    public interface IDatabaseContext
    {
        System.Data.Entity.Database Database { get; }

        void DetachEntries();

        void DiscartChanges();

        DbEntityEntry Entry(object entity);

        IList<ProtocoloProcedure> ListarProtocolos(ProtocoloProcedure protocolo = null);

        int SaveChanges();

        int SelecionarNumeroInscricaoUnico(int tipoPessoa, int unidadeCadastradora);

        DbSet<T> Set<T>() where T : class;
    }
}