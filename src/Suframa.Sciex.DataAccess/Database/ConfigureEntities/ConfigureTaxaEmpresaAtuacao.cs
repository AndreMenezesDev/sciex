using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureTaxaEmpresaAtuacao(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TaxaEmpresaAtuacaoEntity>()
				.Property(e => e.CNPJ)
				.IsUnicode(false);
		}
	}
}