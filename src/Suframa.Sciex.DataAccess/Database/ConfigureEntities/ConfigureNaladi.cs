using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureNaladi(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<NaladiEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<NaladiEntity>()
				.HasMany(e => e.Parametros)
				.WithRequired(e => e.Naladi)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<NaladiEntity>()
				.HasMany(e => e.PliMercadoria)
				.WithRequired(e => e.Naladi)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<NaladiEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

		}
	}
}