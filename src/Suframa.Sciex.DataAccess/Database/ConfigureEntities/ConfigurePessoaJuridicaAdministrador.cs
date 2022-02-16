namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePessoaJuridicaAdministrador(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PessoaJuridicaAdministradorEntity>()
                .Property(e => e.Cpf)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaAdministradorEntity>()
                .Property(e => e.Nome)
                .IsUnicode(false);
        }
    }
}