using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureLiArquivoRetorno(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<LiArquivoRetornoEntity>();

			modelBuilder.Entity<LiArquivoRetornoEntity>()
			  .HasOptional(s => s.LiArquivo)
			  .WithRequired(ad => ad.LiArquivoRetorno);
		}
	}
}