using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureOrgaoAnuente(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrgaoAnuenteEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<OrgaoAnuenteEntity>()
				.HasMany(e => e.PliProcessoAnuenteEntity)
				.WithRequired(e => e.OrgaoAnuente)
				.WillCascadeOnDelete(false);
		}
	}
}