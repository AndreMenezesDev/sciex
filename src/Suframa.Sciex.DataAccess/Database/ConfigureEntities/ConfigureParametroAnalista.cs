namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureParametroAnalista(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParametroAnalistaEntity>()
                .HasMany(e => e.ParametroAnalistaServico)
                .WithRequired(e => e.ParametroAnalista)
                .WillCascadeOnDelete(false);
        }
    }
}