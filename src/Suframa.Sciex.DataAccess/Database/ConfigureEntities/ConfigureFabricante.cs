namespace Suframa.Sciex.DataAccess.Database
{
	using Suframa.Sciex.DataAccess.Database.Entities;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity;

	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureFabricante(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<FabricanteEntity>()
				.Property(e => e.RazaoSocial)
				.IsUnicode(false);

			modelBuilder.Entity<FabricanteEntity>()
				.HasMany(e => e.Parametros)
				.WithOptional(e => e.Fabricante)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<FundamentoLegalEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

		}

	
	}
}