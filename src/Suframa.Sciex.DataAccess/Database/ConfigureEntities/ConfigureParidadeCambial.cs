using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContextSciex : DbContext
    {
        private static void ConfigureParidadeCambial(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParidadeCambialEntity>()
                .Property(e => e.NumeroUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<ParidadeCambialEntity>()
                .Property(e => e.NomeUsuario)
                .IsUnicode(false);

			modelBuilder.Entity<ParidadeCambialEntity>()
				.HasMany(e => e.ParidadeValor)
				.WithRequired(e => e.ParidadeCambial)
				.WillCascadeOnDelete(false);
		}
    }
}