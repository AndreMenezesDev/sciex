using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureCampoSistema(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CampoSistemaEntity>()
               .Property(e => e.Secao)
               .IsUnicode(false);

            modelBuilder.Entity<CampoSistemaEntity>()
                .Property(e => e.Tabela)
                .IsUnicode(false);

            modelBuilder.Entity<CampoSistemaEntity>()
                .Property(e => e.Campo)
                .IsUnicode(false);

            modelBuilder.Entity<CampoSistemaEntity>()
                .Property(e => e.DescricaoCampo)
                .IsUnicode(false);

            modelBuilder.Entity<CampoSistemaEntity>()
                .Property(e => e.CampoObjeto)
                .IsUnicode(false);

            modelBuilder.Entity<CampoSistemaEntity>()
                .Property(e => e.CampoTela)
                .IsUnicode(false);
        }
    }
}