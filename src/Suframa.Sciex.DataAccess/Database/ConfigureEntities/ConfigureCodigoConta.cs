using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureCodigoConta(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CodigoContaEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<CodigoContaEntity>()
				.Property(e => e.Codigo);

			modelBuilder.Entity<CodigoContaEntity>()
				.HasMany(e => e.PliMercadoria)
				.WithRequired(e => e.CodigoConta)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CodigoContaEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}