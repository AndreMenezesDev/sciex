using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePlanoExportacaoProduto(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PEProdutoEntity>()
				.Property(src => src.Qtd).HasPrecision(14, 5);

		}
	}
}