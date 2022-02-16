using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureDiEmbalagemEntity(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DiEmbalagemEntity>().
				Property(e => e.Id);
		}
	}
}