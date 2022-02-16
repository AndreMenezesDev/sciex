using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigurePLIMercadoria(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PliMercadoriaEntity>()
				.Property(e => e.DescricaoInformacaoComplementar)
				.IsUnicode(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasOptional(x => x.TaxaPliMercadoria)
				.WithRequired(x => x.PliMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasMany(e => e.Lancamento)
				.WithRequired(e => e.PliMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.Property(e => e.PesoLiquido)
				.HasPrecision(15, 5);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.Property(e => e.QuantidadeUnidadeMedidaEstatistica)
				.HasPrecision(14, 5);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.Property(e => e.ValorTotalCondicaoVenda)
				.HasPrecision(15, 2);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.Property(e => e.ValorTotalCondicaoVendaDolar)
				.HasPrecision(15, 2);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.Property(e => e.ValorTotalCondicaoVendaReal)
				.HasPrecision(15, 2);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasMany(e => e.PliDetalheMercadoria)
				.WithRequired(e => e.PliMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasMany(e => e.PliProcessoAnuente)
				.WithRequired(e => e.PLIMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasOptional(e => e.Ali)
				.WithRequired(e => e.PliMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasOptional(e => e.Li)
				.WithRequired(e => e.PliMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasMany(e => e.AliHistorico)
				.WithRequired(e => e.PliMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasOptional(e => e.PliFornecedorFabricante)
				.WithRequired(e => e.PliMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.HasMany(e => e.AliEntradaArquivo)
				.WithRequired(e => e.PliMercadoria)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PliMercadoriaEntity>()
				.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

		}
	}
}