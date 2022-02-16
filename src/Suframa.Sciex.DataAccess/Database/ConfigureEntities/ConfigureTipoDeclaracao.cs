using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureTipoDeclaracao(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TipoDeclaracaoEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<TipoDeclaracaoEntity>()
				.Property(e => e.Codigo);

			modelBuilder.Entity<TipoDeclaracaoEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}