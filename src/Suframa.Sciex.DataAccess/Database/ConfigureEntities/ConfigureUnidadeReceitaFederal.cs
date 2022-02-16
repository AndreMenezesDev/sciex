using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureUnidadeReceitaFederal(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UnidadeReceitaFederalEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<UnidadeReceitaFederalEntity>()
				.HasMany(e => e.ParametrosUnidadeEntrada)
				.WithOptional(e => e.UnidadeReceitaFederalEntrada)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UnidadeReceitaFederalEntity>()
				.HasMany(e => e.ParametrosUnidadeDespacho)
				.WithOptional(e => e.UnidadeReceitaFederalDespacho)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UnidadeReceitaFederalEntity>()
				.HasMany(e => e.PliMercadoriaUnidadeEntrada)
				.WithOptional(e => e.UnidadeReceitaFederalEntrada)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UnidadeReceitaFederalEntity>()
				.HasMany(e => e.PliMercadoriaUnidadeDespacho)
				.WithOptional(e => e.UnidadeReceitaFederalDespacho)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UnidadeReceitaFederalEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}