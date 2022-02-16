using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureTaxaPliMercadoria(DbModelBuilder modelBuilder)
		{

			modelBuilder.Entity<TaxaPliMercadoriaEntity>();


			modelBuilder.Entity<TaxaPliMercadoriaEntity>()
				.Property(e => e.ValorMercadoriaMoedaNegociada)
				.HasPrecision(13, 2);

			modelBuilder.Entity<TaxaPliMercadoriaEntity>()
				.Property(e => e.ValorMercadoriaReais)
				.HasPrecision(19, 7);

			modelBuilder.Entity<TaxaPliMercadoriaEntity>()
				.Property(e => e.ValorPercentualReducao)
				.HasPrecision(5, 3);

		}
	}
}