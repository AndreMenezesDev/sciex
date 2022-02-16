namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePessoaJuridicaSocio(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PessoaJuridicaSocioEntity>()
               .Property(e => e.TipoPessoa)
               .HasPrecision(1, 0);

            modelBuilder.Entity<PessoaJuridicaSocioEntity>()
                .Property(e => e.CnpjCpf)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaSocioEntity>()
                .Property(e => e.Nome)
                .IsUnicode(false);
        }
    }
}