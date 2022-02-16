namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureArquivo(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArquivoEntity>()
              .Property(e => e.Nome)
              .IsUnicode(false);

            modelBuilder.Entity<ArquivoEntity>()
                .Property(e => e.Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<ArquivoEntity>()
               .HasMany(e => e.Recurso)
               .WithRequired(e => e.Arquivo)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<ArquivoEntity>()
               .HasMany(e => e.DiligenciaAnexos)
               .WithRequired(e => e.Arquivo)
               .WillCascadeOnDelete(false);
        }
    }
}