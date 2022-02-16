using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContext : DbContext
	{
		private static void ConfigureViewAtividadeEconomicaPrincipalEntity(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ViewAtividadeEconomicaPrincipalEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);


		}
	}
}