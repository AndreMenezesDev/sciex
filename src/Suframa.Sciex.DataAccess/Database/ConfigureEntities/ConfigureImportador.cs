using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureImportador(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ImportadorEntity>()
				.Property(e => e.CNPJ)
				.IsUnicode(false);
		}
	}
}