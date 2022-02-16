namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureQuadroPessoal(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuadroPessoalEntity>()
                .Property(e => e.Cpf)
                .IsUnicode(false);

            modelBuilder.Entity<QuadroPessoalEntity>()
                .Property(e => e.Nome)
                .IsUnicode(false);
        }
    }
}