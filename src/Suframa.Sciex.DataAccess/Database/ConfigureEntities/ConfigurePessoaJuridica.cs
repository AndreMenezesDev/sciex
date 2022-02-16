namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePessoaJuridica(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.Cnpj)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.RazaoSocial)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.NomeFantasia)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.NumeroRegistroConstituicao)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.NumeroEndereco)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
               .Property(e => e.Telefone)
               .HasPrecision(11, 0);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.PontoReferencia)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .HasMany(e => e.PessoaJuridicaAdministrador)
                .WithRequired(e => e.PessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .HasMany(e => e.PessoaJuridicaAtividade)
                .WithRequired(e => e.PessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .HasMany(e => e.PessoaJuridicaInscricaoEstadual)
                .WithRequired(e => e.PessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .HasMany(e => e.PessoaJuridicaSocio)
                .WithRequired(e => e.PessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .HasMany(e => e.QuadroPessoal)
                .WithRequired(e => e.PessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridicaEntity>()
                .Property(e => e.NumeroInscricaoMunicipal)
                .IsUnicode(false);
        }
    }
}