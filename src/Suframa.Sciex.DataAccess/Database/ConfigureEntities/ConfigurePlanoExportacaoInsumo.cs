using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePlanoExportacaoInsumo(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PEInsumoEntity>();
				//.HasMany(q=> q.ListaPEDetalheInsumo)
				//.WithOptional(q=> q.PEInsumo)
				//.WillCascadeOnDelete(false);
		}
	}
}