using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{

	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureTipoEmbalagem(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TipoEmbalagemEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<TipoEmbalagemEntity>()
				.Property(e => e.Codigo);

			modelBuilder.Entity<TipoEmbalagemEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}
