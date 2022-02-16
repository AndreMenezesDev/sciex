namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureProtocolo(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProtocoloEntity>()
                .HasMany(e => e.TaxaServico)
                .WithRequired(e => e.Protocolo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProtocoloEntity>()
               .HasMany(e => e.WorkflowProtocolo)
               .WithRequired(e => e.Protocolo)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProtocoloEntity>()
                .HasMany(e => e.Recurso)
                .WithRequired(e => e.Protocolo)
                .WillCascadeOnDelete(false);
        }
    }
}