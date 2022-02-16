namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePessoaJuridicaInscricaoEstadual(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PessoaJuridicaInscricaoEstadualEntity>()
                .Property(e => e.Numero)
                .IsUnicode(false);
        }
    }
}