using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePLIAplicacao(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PliAplicacaoEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<PliAplicacaoEntity>()
				.HasMany(e => e.ControleImportacao)
				.WithRequired(e => e.PliAplicacao)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliAplicacaoEntity>()
				.HasMany(e => e.PliEntity)
				.WithRequired(e => e.PliAplicacao)
				.WillCascadeOnDelete(false);

		}
	}
}