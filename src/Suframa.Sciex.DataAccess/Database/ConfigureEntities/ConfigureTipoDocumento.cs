namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureTipoDocumento(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoDocumentoEntity>()
                .Property(e => e.Codigo)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDocumentoEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDocumentoEntity>()
                .Property(e => e.TipoOrigem)
                .HasPrecision(1, 0);

            modelBuilder.Entity<TipoDocumentoEntity>()
                .Property(e => e.Link)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDocumentoEntity>()
                .Property(e => e.DescricaoLegal)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDocumentoEntity>()
                .Property(e => e.StatusInformacaoComplementar);

            modelBuilder.Entity<TipoDocumentoEntity>()
                .HasMany(e => e.ListaDocumento)
                .WithRequired(e => e.TipoDocumento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoDocumentoEntity>()
                .HasMany(e => e.RequerimentoDocumento)
                .WithRequired(e => e.TipoDocumento)
                .WillCascadeOnDelete(false);
        }
    }
}