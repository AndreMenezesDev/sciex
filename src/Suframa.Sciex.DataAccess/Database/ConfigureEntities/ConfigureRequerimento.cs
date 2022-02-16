namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureRequerimento(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequerimentoEntity>()
              .Property(e => e.Codigo)
              .IsUnicode(false);

            modelBuilder.Entity<RequerimentoEntity>()
                .HasMany(e => e.Protocolo)
                .WithRequired(e => e.Requerimento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequerimentoEntity>()
                .HasMany(e => e.RequerimentoDocumento)
                .WithRequired(e => e.Requerimento)
                .WillCascadeOnDelete(false);
        }
    }
}