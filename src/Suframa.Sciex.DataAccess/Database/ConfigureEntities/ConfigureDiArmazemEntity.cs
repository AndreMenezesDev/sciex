using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureDiArmazemEntity(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DiArmazemEntity>().
				Property(e => e.Id);
		}
	}
}