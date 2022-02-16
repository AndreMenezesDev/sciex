using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureDiligenciaAtividades(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiligenciaAtividadesEntity>()
                .Property(e => e.DescricaoSubclasse)
                .IsUnicode(false);

            modelBuilder.Entity<DiligenciaAtividadesEntity>()
                .HasMany(e => e.DiligenciaAtividadeSetor)
                .WithRequired(e => e.DiligenciaAtividade)
                .WillCascadeOnDelete(false);
        }
    }
}