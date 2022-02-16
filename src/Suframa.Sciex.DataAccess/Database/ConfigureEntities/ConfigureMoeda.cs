using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureMoeda(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MoedaEntity>()
				.Property(e => e.Descricao)
				.IsUnicode(false);

			modelBuilder.Entity<MoedaEntity>()
				.HasMany(e => e.ParidadeValor)
				.WithRequired(e => e.Moeda)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<MoedaEntity>()
				.HasMany(e => e.PliMercadoria)
				.WithRequired(e => e.Moeda)
				.WillCascadeOnDelete(false);

		}
	}
}
