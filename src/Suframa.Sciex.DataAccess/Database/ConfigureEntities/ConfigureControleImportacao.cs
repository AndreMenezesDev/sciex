using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureControleImportacao(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ControleImportacaoEntity>()
				.Property(e => e.DescricaoSetor)
				.IsUnicode(false);

			modelBuilder.Entity<ControleImportacaoEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}