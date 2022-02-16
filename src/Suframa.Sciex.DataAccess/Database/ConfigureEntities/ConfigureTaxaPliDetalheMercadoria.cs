using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureTaxaPliDetalheMercadoria(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>();

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorBaseFatoGeradorItem)
				.HasPrecision(5, 2);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorPercentualLimitadorItem)
				.HasPrecision(5, 3);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorPercentualReducaoItem)
				.HasPrecision(5, 3);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.QtdUnidadeComercializada)
				.HasPrecision(15, 5);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorUnidadeCondicaoVenda)
				.HasPrecision(19, 7);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorUnidadeReais)
				.HasPrecision(19, 7);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorCalculadoLimitadorItem)
				.HasPrecision(19, 7);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorPrevalenciaItem)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorReducaoItem)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorTCIFItem)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliDetalheMercadoriaEntity>()
				.Property(e => e.ValorReducaoBaseItem)
				.HasPrecision(7, 2);

		}
	}
}