using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database.ConfigureEntities
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureMotivoSituacaoInscricao(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MotivoSituacaoInscricaoEntity>()
                    .Property(e => e.Descricao)
                    .IsUnicode(false);

            modelBuilder.Entity<MotivoSituacaoInscricaoEntity>()
                    .Property(e => e.Explicacao)
                    .IsUnicode(false);

            modelBuilder.Entity<MotivoSituacaoInscricaoEntity>()
                    .Property(e => e.Orientacao)
                    .IsUnicode(false);
        }
    }
}