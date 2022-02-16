using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContextSciex : DbContext
    {
        private static void ConfigureListaServico(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListaServicoEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

         
        }
    }
}