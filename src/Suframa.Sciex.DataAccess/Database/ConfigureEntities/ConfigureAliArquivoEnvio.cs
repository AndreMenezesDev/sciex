using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureAliArquivoEnvio(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AliArquivoEnvioEntity>();

			modelBuilder.Entity<AliArquivoEnvioEntity>()
				.HasMany(e => e.Ali)
				.WithRequired(e => e.AliArquivoEnvio)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<AliArquivoEnvioEntity>()
				.HasMany(e => e.AliEntradaArquivo)
				.WithRequired(e => e.AliArquivoEnvio)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<AliArquivoEnvioEntity>()
			  .HasOptional(s => s.AliArquivo)
			  .WithRequired(ad => ad.AliArquivoEnvio);

		}
	}
}