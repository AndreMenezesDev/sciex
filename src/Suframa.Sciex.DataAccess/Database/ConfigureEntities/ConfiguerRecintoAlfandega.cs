using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{

	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureRecintoAlfandega(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RecintoAlfandegaEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<RecintoAlfandegaEntity>()
				.Property(e => e.Codigo);

			modelBuilder.Entity<RecintoAlfandegaEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}
