using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{


    public partial class DatabaseContext : DbContext
    {
        public static void ConfigureViewMunicipio(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewMunicipioEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);
		}
    }
}