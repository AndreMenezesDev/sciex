using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureNcmExcecao(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<NcmExcecaoEntity>()
				.Property(e => e.DescricaoNcm)
				.IsUnicode(true);

			modelBuilder.Entity<NcmExcecaoEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}