
using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureViaTransporteEntity(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ViaTransporteEntity>()
				.Property( e => e.IdViaTransporte);

			modelBuilder.Entity<ViaTransporteEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<ViaTransporteEntity>()
				.Property(e => e.Codigo);

			modelBuilder.Entity<ViaTransporteEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}
