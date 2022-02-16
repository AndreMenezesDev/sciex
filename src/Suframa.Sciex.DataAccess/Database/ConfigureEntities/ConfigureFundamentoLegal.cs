namespace Suframa.Sciex.DataAccess.Database
{
	using Suframa.Sciex.DataAccess.Database.Entities;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity;

	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureFundamentoLegal(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<FundamentoLegalEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<FundamentoLegalEntity>()
				.HasMany(e => e.Parametros)
				.WithOptional(e => e.FundamentoLegal)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<FundamentoLegalEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}