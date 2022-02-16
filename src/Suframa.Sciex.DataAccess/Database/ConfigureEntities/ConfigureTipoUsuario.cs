using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureTipoUsuario(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoUsuarioEntity>()
               .Property(e => e.Descricao)
               .IsUnicode(false);
        }
    }
}