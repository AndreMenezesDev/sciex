using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureAuditoria(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AuditoriaEntity>()
				.Property(e => e.DescricaoAcao)
				.IsUnicode(true);

			modelBuilder.Entity<AuditoriaEntity>()
				.Property(e => e.Justificativa)
				.IsUnicode(true);
		}
	}
}