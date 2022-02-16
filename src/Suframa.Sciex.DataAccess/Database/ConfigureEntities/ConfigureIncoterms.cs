using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureIncoterms(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IncotermsEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);
		}
	}
}