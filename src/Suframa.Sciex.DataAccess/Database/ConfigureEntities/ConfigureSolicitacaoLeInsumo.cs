using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureSolicitacaoLeInsumo(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SolicitacaoLeInsumoEntity>()
				.Property(e => e.DescricaoInsumo)
				.IsUnicode(false);
		}
	}
}