using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{


    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureViewSetorEmpresa(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewSetorEmpresaEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);
		}
    }
}