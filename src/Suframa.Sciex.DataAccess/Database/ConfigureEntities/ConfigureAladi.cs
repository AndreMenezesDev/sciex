using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureAladi(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AladiEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<AladiEntity>()
				.HasMany(e => e.Parametros)
				.WithRequired(e => e.Aladi)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<AladiEntity>()
				.HasMany(e => e.PliMercadoria)
				.WithRequired(e => e.Aladi)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<AladiEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}