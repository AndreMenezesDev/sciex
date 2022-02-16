using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureParametros(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ParametrosEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);


			modelBuilder.Entity<ParametrosEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

		}
	}
}