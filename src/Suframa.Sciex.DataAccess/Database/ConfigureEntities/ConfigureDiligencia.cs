using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureDiligencia(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiligenciaEntity>()
                .Property(e => e.PessoaResponsavel)
                .IsUnicode(false);

            modelBuilder.Entity<DiligenciaEntity>()
                .Property(e => e.Parecer)
                .IsUnicode(false);

            modelBuilder.Entity<DiligenciaEntity>()
                .Property(e => e.Motivo)
                .IsUnicode(false);

            modelBuilder.Entity<DiligenciaEntity>()
                .Property(e => e.AnalistaResponsavel)
                .IsUnicode(false);

            modelBuilder.Entity<DiligenciaEntity>()
                .HasMany(e => e.DiligenciaAnexo)
                .WithRequired(e => e.Diligencia)
                .WillCascadeOnDelete(false);
        }
    }
}