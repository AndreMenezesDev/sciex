using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePlanoExportacaoProdutoPais(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PEProdutoPaisEntity>()
				.Property(src => src.Quantidade).HasPrecision(14, 5);
		}
	}
}