namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePessoaFisica(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PessoaFisicaEntity>()
             .Property(e => e.Cpf)
             .IsUnicode(false);

            modelBuilder.Entity<PessoaFisicaEntity>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaFisicaEntity>()
                .Property(e => e.NumeroEndereco)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaFisicaEntity>()
                .Property(e => e.Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaFisicaEntity>()
                .Property(e => e.PontoReferencia)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaFisicaEntity>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}