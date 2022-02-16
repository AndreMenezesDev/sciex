using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePedidoAlteracao(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoCorrecaoEntity>()
               .Property(e => e.CampoDe)
               .IsUnicode(false);

            modelBuilder.Entity<PedidoCorrecaoEntity>()
                .Property(e => e.CampoPara)
                .IsUnicode(false);

            modelBuilder.Entity<PedidoCorrecaoEntity>()
                .Property(e => e.Justificativa)
                .IsUnicode(false);
        }
    }
}