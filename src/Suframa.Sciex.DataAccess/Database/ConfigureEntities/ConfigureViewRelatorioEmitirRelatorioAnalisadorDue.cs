using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureViewRelatorioEmitirRelatorioAnalisadorDue(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ViewEmitirRelatorioAnalisadorDueEntity>();
		}
	}
}