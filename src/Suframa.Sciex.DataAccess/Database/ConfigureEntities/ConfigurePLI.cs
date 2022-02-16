using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePLI(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PliEntity>()
				.Property(e => e.Cnpj)
				.IsUnicode(false);

			modelBuilder.Entity<PliEntity>()
				.HasOptional(x => x.TaxaPli)
				.WithRequired(x => x.Pli)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliEntity>()
				.HasMany(e => e.PLIHistorico)
				.WithRequired(e => e.Pli)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliEntity>()
				.HasMany(e => e.PLIMercadoria)
				.WithRequired(e => e.Pli)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliEntity>()
				.HasMany(e => e.PLIProduto)
				.WithRequired(e => e.Pli)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliEntity>()
				.HasMany(e => e.Lancamento)
				.WithRequired(e => e.Pli)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliEntity>()
				.HasMany(e => e.ErroProcessamento)
				.WithOptional(e => e.Pli)
				.WillCascadeOnDelete(false);


			modelBuilder.Entity<PliEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
		}
	}
}