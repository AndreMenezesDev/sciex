using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePessoaJuridicaInscricao(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PessoaJuridicaInscricaoEntity>()
              .Property(e => e.Numero)
              .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridicaInscricaoEntity>()
                .Property(e => e.TipoPrincipalSecundaria)
                .HasPrecision(1, 0);

            modelBuilder.Entity<PessoaJuridicaInscricaoEntity>()
                .Property(e => e.TipoEstadualMunicipal)
                .HasPrecision(1, 0);
        }
    }
}