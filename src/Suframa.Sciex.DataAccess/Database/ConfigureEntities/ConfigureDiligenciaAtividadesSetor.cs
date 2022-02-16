using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureDiligenciaAtividadesSetor(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiligenciaAtividadesSetorEntity>()
              .Property(e => e.Setor)
              .IsUnicode(false);
        }
    }
}