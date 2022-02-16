using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureRegimeTributarioMercadoria(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RegimeTributarioMercadoriaEntity>()
				.Property(e => e.DescricaoMunicipio)
				.IsUnicode(false);

			modelBuilder.Entity<RegimeTributarioMercadoriaEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}