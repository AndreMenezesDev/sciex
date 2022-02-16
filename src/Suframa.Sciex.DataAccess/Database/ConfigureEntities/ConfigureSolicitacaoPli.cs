using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureSolicitacaoPli(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SolicitacaoPliEntity>();

			modelBuilder.Entity<SolicitacaoPliEntity>()
				.HasMany(e => e.ErroProcessamento)
				.WithOptional(e => e.SolicitacaoPli)
				.WillCascadeOnDelete(false);
		}
	}
}