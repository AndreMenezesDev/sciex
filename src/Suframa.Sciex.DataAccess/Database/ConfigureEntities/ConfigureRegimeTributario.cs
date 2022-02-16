namespace Suframa.Sciex.DataAccess.Database
{
	using Suframa.Sciex.DataAccess.Database.Entities;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity;

	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureRegimeTributario(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RegimeTributarioEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<RegimeTributarioEntity>()
				.HasMany(e => e.Parametros)
				.WithOptional(e => e.RegimeTributario)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<RegimeTributarioEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}