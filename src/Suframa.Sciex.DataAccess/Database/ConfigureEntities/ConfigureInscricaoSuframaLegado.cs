namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureInscricaoSuframaLegado(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InscricaoSuframaLegadoEntity>()
               .Property(e => e.NumeroCnpj)
               .IsUnicode(false);
        }
    }
}