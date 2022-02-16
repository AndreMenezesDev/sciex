using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContext : DbContext
	{
		private static void ConfigureDicionarioDropDown(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DicionarioDropDownEntity>()
			  .Property(e => e.Secao)
			  .IsUnicode(false);

			modelBuilder.Entity<DicionarioDropDownEntity>()
				.Property(e => e.Campo)
				.IsUnicode(false);

			modelBuilder.Entity<DicionarioDropDownEntity>()
				.Property(e => e.Valor)
				.HasPrecision(10, 0);

			modelBuilder.Entity<DicionarioDropDownEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);
		}
	}
}