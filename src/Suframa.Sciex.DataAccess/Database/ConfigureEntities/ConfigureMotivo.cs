using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureMotivo(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MotivoEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);
		}
	}
}