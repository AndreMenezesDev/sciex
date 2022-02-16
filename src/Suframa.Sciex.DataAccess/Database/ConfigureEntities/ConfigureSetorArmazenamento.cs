using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureSetorArmazenamento(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SetorArmazenamentoEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<SetorArmazenamentoEntity>()
				.Property(e => e.Codigo);

			modelBuilder.Entity<SetorArmazenamentoEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

			//modelBuilder.Entity<SetorArmazenamentoEntity>()
			//	.Property(e => e.IdRecintoAlfandega);

		}
	}
}
