using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{


    public partial class DatabaseContextSciex : DbContext
    {
        private static void ConfigureViewNcm(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewNcmEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);
		}
    }
}