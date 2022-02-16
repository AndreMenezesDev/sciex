using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureCodigoUtilizacao(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CodigoUtilizacaoEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);


			modelBuilder.Entity<CodigoUtilizacaoEntity>()
				.HasMany(e => e.PliMercadoria)
				.WithRequired(e => e.CodigoUtilizacao)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CodigoUtilizacaoEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}