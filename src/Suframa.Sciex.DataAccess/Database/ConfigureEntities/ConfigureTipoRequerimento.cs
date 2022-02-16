namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureTipoRequerimento(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoRequerimentoEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TipoRequerimentoEntity>()
                .Property(e => e.StatusTipoCobranca);

            modelBuilder.Entity<TipoRequerimentoEntity>()
                .Property(e => e.StatusExigeAnalise);

            modelBuilder.Entity<TipoRequerimentoEntity>()
                .HasMany(e => e.ListaDocumento)
                .WithRequired(e => e.TipoRequerimento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoRequerimentoEntity>()
                .HasMany(e => e.Requerimento)
                .WithRequired(e => e.TipoRequerimento)
                .WillCascadeOnDelete(false);
        }
    }
}