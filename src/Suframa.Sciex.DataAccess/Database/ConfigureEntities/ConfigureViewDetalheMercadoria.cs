using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContextSciex : DbContext
    {
        private static void ConfigureViewDetalheMercadoria(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewDetalheMercadoriaEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);
		}
    }
}