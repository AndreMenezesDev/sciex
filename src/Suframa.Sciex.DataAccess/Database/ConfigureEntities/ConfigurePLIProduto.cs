using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePLIProduto(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PliProdutoEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<PliProdutoEntity>()
			.HasMany(e => e.PliMercadoria)
			.WithOptional(e => e.PliProduto)
			.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliProdutoEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}