namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePaiz(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaizEntity>()
               .Property(e => e.Descricao)
               .IsUnicode(false);
        }
    }
}