using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureEstruturaPropriaPLI(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EstruturaPropriaPliEntity>();

			modelBuilder.Entity<EstruturaPropriaPliEntity>()
			  .HasOptional(s => s.EstruturaPropriaPliArquivo)
			  .WithRequired(ad => ad.EstruturaPropriaPli);

			modelBuilder.Entity<EstruturaPropriaPliEntity>()
			  .HasMany(s => s.PliEntity)
			  .WithRequired(ad => ad.EstruturaPropriaPli);

			modelBuilder.Entity<EstruturaPropriaPliEntity>()
				.HasMany(e => e.SolicitacaoPli)
				.WithOptional(e => e.EstruturaPropriaPli)
				.WillCascadeOnDelete(false);
		}
	}
}
