using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContextSciex : DbContext
    {
        private static void ConfigureViewUnidadeMedida(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewUnidadeMedidaEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);
		}
    }
}