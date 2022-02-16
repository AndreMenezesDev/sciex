using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureNcm(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<NcmEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(true);
		}
	}
}