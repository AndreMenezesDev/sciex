namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureRequerimentoDocumento(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequerimentoDocumentoEntity>()
             .Property(e => e.NumeroCertidao)
             .IsUnicode(false);

            modelBuilder.Entity<RequerimentoDocumentoEntity>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<RequerimentoDocumentoEntity>()
                .Property(e => e.TipoOrigem);
        }
    }
}