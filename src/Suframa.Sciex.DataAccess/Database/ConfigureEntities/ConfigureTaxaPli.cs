using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureTaxaPli(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.CNPJ)
				.IsUnicode(false);	

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorBaseFatoGerador)
				.HasPrecision(5, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorPercentualLimitadorPli)
				.HasPrecision(5, 3);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorTotalMercadoriaReais)
				.HasPrecision(19, 7);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorTotalCalculadoLimitadorPLI)
				.HasPrecision(15, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorPrevalenciaPLI)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorPercentualReducaoPLI)
				.HasPrecision(5, 3);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorReducaoPLI)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorTCIFPLI)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorReducaoBasePLI)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorTotalReducaoItens)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorTotalReducaoBaseItens)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorGeralReducaoTCIF)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorGeralTCIF)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorGeralReducaoBase)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.Property(e => e.ValorTotalTCIFItens)
				.HasPrecision(7, 2);

			modelBuilder.Entity<TaxaPliEntity>()
				.HasMany(q => q.TaxaPliDebito)
				.WithRequired(w => w.TaxaPli)
				.WillCascadeOnDelete(false);
		}
	}

}