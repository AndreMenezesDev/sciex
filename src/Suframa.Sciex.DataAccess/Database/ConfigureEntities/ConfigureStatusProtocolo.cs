namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureStatusProtocolo(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatusProtocoloEntity>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<StatusProtocoloEntity>()
                .HasMany(e => e.Protocolo)
                .WithRequired(e => e.StatusProtocolo)
                .WillCascadeOnDelete(false);
        }
    }
}