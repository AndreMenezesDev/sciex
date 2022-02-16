using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureEstruturaPropriaPLIArquivo(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EstruturaPropriaPliArquivoEntity>();
		}
	}
}
