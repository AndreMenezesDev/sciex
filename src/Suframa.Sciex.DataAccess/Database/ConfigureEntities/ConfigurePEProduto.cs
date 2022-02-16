using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database.ConfigureEntities
{

	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePEProduto(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PEProdutoEntity>();
		}
	}
}
