namespace Suframa.Sciex.DataAccess.Database
{
	using Suframa.Sciex.DataAccess.Database.Entities;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity;

	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureFornecedor(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<FornecedorEntity>()
				.Property(e => e.RazaoSocial)
				.IsUnicode(false);

			modelBuilder.Entity<FornecedorEntity>()
				.HasMany(e => e.Parametros)
				.WithOptional(e => e.Fornecedor)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<FornecedorEntity>()
			.Property(e => e.RowVersion)
			.IsRowVersion()
			.IsConcurrencyToken()
			.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}