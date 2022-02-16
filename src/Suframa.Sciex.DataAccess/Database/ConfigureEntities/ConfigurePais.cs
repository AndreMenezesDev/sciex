namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePais(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaisEntity>()
               .Property(e => e.Descricao)
               .IsUnicode(false);
        }
    }
}