using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContextSciex : DbContext
    {
        private static void ConfigureViewImportador(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewImportadorEntity>()
                .Property(e => e.DescricaoNaturezaGrupo)
                .IsUnicode(false);
		}
    }
}