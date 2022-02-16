namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureVWPessoaJuridicaAprovado(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.Cnpj)
                .IsUnicode(false);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.RazaoSocial)
                .IsUnicode(false);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.NomeFantasia)
                .IsUnicode(false);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.NumeroRegistroConstituicao)
                .IsUnicode(false);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.NumeroEndereco)
                .IsUnicode(false);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
               .Property(e => e.Telefone)
               .HasPrecision(11, 0);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.PontoReferencia)
                .IsUnicode(false);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<VWPessoaJuridicaAprovadoEntity>()
                .Property(e => e.NumeroInscricaoMunicipal)
                .IsUnicode(false);
        }
    }
}