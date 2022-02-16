using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePLIDetalheMercadoria(DbModelBuilder modelBuilder)
		{

			modelBuilder.Entity<PliDetalheMercadoriaEntity>()
				.HasOptional(x => x.TaxaPliDetalheMercadoria)
				.WithRequired(x => x.PliDetalheMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliDetalheMercadoriaEntity>()
				.Property(e => e.QuantidadeComercializada)
				.HasPrecision(14, 5);

			modelBuilder.Entity<PliDetalheMercadoriaEntity>()
				.Property(e => e.ValorUnitarioCondicaoVenda)
				.HasPrecision(18, 7);

			modelBuilder.Entity<PliDetalheMercadoriaEntity>()
				.Property(e => e.ValorCondicaoVenda)
				.HasPrecision(20, 12);

			modelBuilder.Entity<PliDetalheMercadoriaEntity>()
				.Property(e => e.ValorTotalCondicaoVendaReal)
				.HasPrecision(20, 12);

			modelBuilder.Entity<PliDetalheMercadoriaEntity>()
				.Property(e => e.ValorTotalCondicaoVendaDolar)
				.HasPrecision(20, 12);

			modelBuilder.Entity<PliDetalheMercadoriaEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}