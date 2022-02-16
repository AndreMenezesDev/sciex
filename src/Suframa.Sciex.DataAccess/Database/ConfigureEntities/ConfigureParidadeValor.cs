using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContextSciex : DbContext
    {
        private static void ConfigureParidadeValor(DbModelBuilder modelBuilder)
        {
			modelBuilder.Entity<ParidadeValorEntity>();

			modelBuilder.Entity<ParidadeValorEntity>()
				.HasMany(e => e.TaxaPliMercadoria)
				.WithRequired(e => e.ParidadeValor)
				.WillCascadeOnDelete(false);
		}
    }
}