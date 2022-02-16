namespace Suframa.Sciex.DataAccess.Database
{
	using Suframa.Sciex.DataAccess.Database.Entities;
	using System.Data.Entity;

	public partial class DatabaseContext : DbContext
	{
		private static void ConfigureRegimeTributarioTeste(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RegimeTributarioTesteEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);
		}
	}
}