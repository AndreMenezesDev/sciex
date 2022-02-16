namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureSituacaoInscricao(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SituacaoInscricaoEntity>()
               .Property(e => e.Descricao)
               .IsUnicode(false);
        }
    }
}