using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureTaxaGrupoBeneficio(DbModelBuilder modelBuilder)
		{

			modelBuilder.Entity<TaxaGrupoBeneficioEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);
			modelBuilder.Entity<TaxaGrupoBeneficioEntity>()
				.Property(e => e.ValorPercentualReducao)
				.HasPrecision(8, 3);
		}
	}
}