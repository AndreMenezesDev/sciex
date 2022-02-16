using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureSolicitacaoPE(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SolicitacaoPELoteEntity>();
			modelBuilder.Entity<SolicitacaoPEDetalheEntity>();
			modelBuilder.Entity<SolicitacaoPEInsumoEntity>();
			modelBuilder.Entity<SolicitacaoPEProdutoEntity>();
			modelBuilder.Entity<SolicitacaoPaisProdutoEntity>();
			modelBuilder.Entity<SolicitacaoPEArquivoEntity>();
		}
	}
}