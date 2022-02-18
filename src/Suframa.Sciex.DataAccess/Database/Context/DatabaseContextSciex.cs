using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.Resources;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext, IDatabaseContextSciex
	{
		public virtual DbSet<PRCDueEntity> PRCDue { get; set; }
		public virtual DbSet<RegimeTributarioEntity> RegimeTributario { get; set; }
		public virtual DbSet<AladiEntity> Aladi { get; set; }
		public virtual DbSet<NaladiEntity> Naladi { get; set; }
		public virtual DbSet<UnidadeReceitaFederalEntity> UnidadeReceitaFederal { get; set; }
		public virtual DbSet<FundamentoLegalEntity> FundamentoLegal { get; set; }
		public virtual DbSet<FornecedorEntity> Fornecedor { get; set; }
		public virtual DbSet<FabricanteEntity> Fabricante { get; set; }
		public virtual DbSet<AnalistaEntity> Analista { get; set; }
		public virtual DbSet<ParidadeCambialEntity> ParidadeCambial { get; set; }
		public virtual DbSet<ParidadeValorEntity> ParidadeValor { get; set; }		
		public virtual DbSet<MoedaEntity> Moeda { get; set; }
		public virtual DbSet<ImportadorEntity> Importador { get; set; }
		public virtual DbSet<ListaServicoEntity> ListaServico { get; set; }
		public virtual DbSet<ControleExecucaoServicoEntity> ControleExecucaoServico { get; set; }
		public virtual DbSet<ParametroConfiguracaoEntity> ParametroConfiguracao { get; set; }
		public virtual DbSet<ParametrosEntity> Parametros { get; set; }
		public virtual DbSet<InstituicaoFinanceiraEntity> InstituicaoFinanceira { get; set; }
		public virtual DbSet<MotivoEntity> Motivo { get; set; }
		public virtual DbSet<IncotermsEntity> Incoterms { get; set; }
		public virtual DbSet<ModalidadePagamentoEntity> ModalidadePagamento { get; set; }
		public virtual DbSet<CodigoContaEntity> CodigoConta { get; set; }
		public virtual DbSet<TipoDeclaracaoEntity> TipoDeclaracao { get; set; }
		public virtual DbSet<CodigoUtilizacaoEntity> CodigoUtilizacao { get; set; }
		public virtual DbSet<RegimeTributarioMercadoriaEntity> RegimeTributarioMercadoria { get; set; }
		public virtual DbSet<ControleImportacaoEntity> ControleImportacao { get; set; }
		public virtual DbSet<NcmEntity> Ncm { get; set; }
		public virtual DbSet<ViewNcmEntity> ViewNcm { get; set; }
		public virtual DbSet<ViewProdutoEmpresaEntity> ViewProdutoEmpresa { get; set; }
		public virtual DbSet<ViewMercadoriaEntity> ViewMercadoria { get; set; }
		public virtual DbSet<ViewDetalheMercadoriaEntity> ViewDetalheMercadoria { get; set; }
		public virtual DbSet<ViewUnidadeMedidaEntity> ViewUnidadeMedida { get; set; }

		public virtual DbSet<PliAplicacaoEntity> PliAplicacao { get; set; }
		public virtual DbSet<PliDetalheMercadoriaEntity> PliDetalheMercadoria { get; set; }
		public virtual DbSet<PliEntity> Pli { get; set; }
		public virtual DbSet<PliAnaliseVisualEntity> PliAnaliseVisual { get; set; }
		public virtual DbSet<PliAnaliseVisualAnexoEntity> PliAnaliseVisualAnexo { get; set; }
		public virtual DbSet<PliHistoricoEntity> PliHistoricoMercadoria { get; set; }
		public virtual DbSet<LiHistoricoErroEntity> LiHistoricoErroEntity { get; set; }
		public virtual DbSet<PliMercadoriaEntity> PliMercadoria { get; set; }
		public virtual DbSet<PliProcessoAnuenteEntity> PliProcessoAnuente { get; set; }				
		public virtual DbSet<NcmExcecaoEntity> NcmExcecao { get; set; }
		public virtual DbSet<PliProdutoEntity> PliProduto { get; set; }
		public virtual DbSet<OrgaoAnuenteEntity> OrgaoAnuente { get; set; }
		public virtual DbSet<ViewImportadorEntity> ViewImportador { get; set; }

		public virtual DbSet<TaxaEmpresaAtuacaoEntity> TaxaEmpresaAtuacao { get; set; }
		public virtual DbSet<TaxaFatoGeradorEntity> TaxaFatoGerador { get; set; }
		public virtual DbSet<TaxaGrupoBeneficioEntity> TaxaGrupoBeneficio { get; set; }
		public virtual DbSet<TaxaNCMBeneficioEntity> TaxaNCMBeneficio { get; set; }
		public virtual DbSet<TaxaPliDetalheMercadoriaEntity> TaxaPliDetalheMercadoria { get; set; }
		public virtual DbSet<TaxaPliEntity> TaxaPli { get; set; }
		public virtual DbSet<TaxaPliHistoricoEntity> TaxaPliHistorico { get; set; }
		public virtual DbSet<TaxaPliMercadoriaEntity> TaxaPliMercadoria { get; set; }

		public virtual DbSet<AliEntity> AliEntity { get; set; }
		public virtual DbSet<AliArquivoEntity> AliArquivoEntity { get; set; }
		public virtual DbSet<AliArquivoEnvioEntity> AliArquivoEnvioEntity { get; set; }
		public virtual DbSet<LiEntity> LiEntity { get; set; }
		public virtual DbSet<LiSubstituidaEntity> LiSubstituidaEntity { get; set; }
		public virtual DbSet<LiArquivoRetornoEntity> LiArquivoRetornoEntity { get; set; }
		public virtual DbSet<DiEntity> DiEntity { get; set; }

		public virtual DbSet<CodigoLancamentoEntity> CodigoLancamento { get; set; }
		public virtual DbSet<LancamentoEntity> Lancamento { get; set; }
		public virtual DbSet<ErroMensagemEntity> ErroMensagem { get; set; }
		public virtual DbSet<ErroProcessamentoEntity> ErroProcessamento { get; set; }
		public virtual DbSet<ViewRelatorioAnaliseProcessamentoPliEntity> ViewRelatorioAnaliseProcessamentoPli { get; set; }

		public virtual DbSet<LiArquivoEntity> LiArquivo { get; set; }
		public virtual DbSet<AliEntradaArquivoEntity> AliEntradaArquivo { get; set; }
		public virtual DbSet<EstruturaPropriaPliEntity> EstruturaPropriaPLI { get; set; }
		public virtual DbSet<EstruturaPropriaPliArquivoEntity> EstruturaPropriaPLIArquivo { get; set; }

		public virtual DbSet<SolicitacaoFornecedorFabricanteEntity> SolicitacaoFornecedorFabricante { get; set; }
		public virtual DbSet<SolicitacaoPliProcessoAnuenteEntity> SolicitacaoPliProcessoAnuente { get; set; }

		public virtual DbSet<AliHistoricoEntity> AliHistorico { get; set; }

		public virtual DbSet<PliFornecedorFabricanteEntity> PliFornecedorFabricante { get; set; }

		public virtual DbSet<TaxaPliDebitoEntity> TaxaPliDebito { get; set; }
		public virtual DbSet<AuditoriaEntity> Auditoria { get; set; }
		public virtual DbSet<AuditoriaAplicacaoEntity> AuditoriaAplicacao { get; set; }

		public virtual DbSet<SequencialEntity> Sequencial { get; set; }
		public virtual DbSet<DiLiEntity> DiLi { get; set; }
		public virtual DbSet<DiEmbalagemEntradaEntity> DiEmbalagemEntrada { get; set; }
		public virtual DbSet<DiEmbalagemEntity> DiEmbalagem { get; set; }
		public virtual DbSet<DiAdicaoEntradaEntity> DiAdicaoEntrada { get; set; }
		public virtual DbSet<DiArmazemEntity> DiArmazem { get; set; }
		public virtual DbSet<DiArmazemEntradaEntity> DiArmazemEntrada { get; set; }
		public virtual DbSet<DiEntradaEntity> DiEntrada { get; set; }
		public virtual DbSet<DiArquivoEntradaEntity> DiArquivoEntrada { get; set; }
		public virtual DbSet<DiArquivoEntity> DiArquivo { get; set; }

		public virtual DbSet<ViaTransporteEntity> ViaTransporte { get; set; }
		public virtual DbSet<TipoEmbalagemEntity> TipoEmbalagem { get; set; }
		public virtual DbSet<RecintoAlfandegaEntity> RecintoAlfandega { get; set; }
		public virtual DbSet<SetorArmazenamentoEntity> SetorArmazenamento { get; set; }
		public virtual DbSet<ViewProdutoEmpresaExportacaoEntity> ViewProdutoEmpresaExportacao { get; set; }

		public virtual DbSet<LEProdutoEntity> LEProduto { get; set; }
		public virtual DbSet<LEProdutoHistoricoEntity> LEProdutoHistorico { get; set; }
		public virtual DbSet<LEInsumoEntity> LEInsumo { get; set; }
		public virtual DbSet<LEInsumoErroEntity> LEInsumoErro { get; set; }

		public virtual DbSet<EstruturaPropriaLEEntity> EstruturaPropriaLE { get; set; }
		public virtual DbSet<SolicitacaoLeInsumoEntity> SolicitacaoLeInsumo { get; set; }

		public virtual DbSet<ViewInsumoPadraoEntity> ViewInsumoPadrao { get; set; }
		public virtual DbSet<PlanoExportacaoEntity> PlanoExportacao { get; set; }
		public virtual DbSet<PEProdutoEntity> PlanoExportacaoProduto { get; set; }
		public virtual DbSet<PEProdutoPaisEntity> PlanoExportacaoProdutoPais { get; set; }
		public virtual DbSet<PEArquivoEntity> PlanoExportacaoArquivo { get; set; }
		public virtual DbSet<PEInsumoEntity> PEInsumo { get; set; }
		public virtual DbSet<PEDetalheInsumoEntity> PEDetalheInsumo { get; set; }
		public virtual DbSet<PEHistoricoEntity> PEHistorico { get; set; }
		public virtual DbSet<PaisEntity> ViewPais { get; set; }
		public virtual DbSet<PRJUnidadeMedidaEntity> PRJUnidadeMedida { get; set; }
		public virtual DbSet<PRJProdutoEmpresaExportacaoEntity> PRJProdutoEmpresaExportacao { get; set; }
		public virtual DbSet<ProcessoEntity> Processo { get; set; }
		public virtual DbSet<PRCStatusEntity> PRCStatus { get; set; }
		public virtual DbSet<PRCProdutoEntity> PRCProduto { get; set; }
		public virtual DbSet<PRCProdutoPaisEntity> PRCProdutoPais { get; set; }
		public virtual DbSet<PRCInsumoEntity> PRCInsumo { get; set; }
		public virtual DbSet<PRCDetalheInsumoEntity> PRCDetalheInsumo { get; set; }
		public virtual DbSet<PRCSolicitacaoAlteracaoEntity> PRCSolicitacaoAlteracao { get; set; }

		public virtual DbSet<PRCSolicDetalheEntity> PRCSolicDetalhe { get; set; }
		public virtual DbSet<TipoSolicAlteracaoEntity> TipoSolicAlteracao { get; set; }

		public virtual DbSet<SolicitacaoPELoteEntity> SolicitacaoPELote { get; set; }
		public virtual DbSet<SolicitacaoPEDetalheEntity> SolicitacaoPEDetalhe { get; set; }
		public virtual DbSet<SolicitacaoPEInsumoEntity> SolicitacaoPEInsumo { get; set; }
		public virtual DbSet<SolicitacaoPEProdutoEntity> SolicitacaoPEProduto { get; set; }
		public virtual DbSet<SolicitacaoPaisProdutoEntity> SolicitacaoPaisProduto { get; set; }
		public virtual DbSet<SolicitacaoPEArquivoEntity> SolicitacaoPEArquivo { get; set; }
		public virtual DbSet<ParecerTecnicoEntity> ParecerTecnico { get; set; }
		public virtual DbSet<ParecerComplementarEntity> ParecerComplementar { get; set; }
		public virtual DbSet<ParecerTecnicoProdutoEntity> ParecerTecnicoProduto { get; set; }
		public virtual DbSet<ST_ParecerTecnicoEntity> StoreProcedureParecerTecnico { get; set; }
		public virtual DbSet<PRCSolicProrrogacaoEntity> ProcessoSolicProrrogacao { get; set; }
		public virtual DbSet<PRCHistoricoInsumoEntity> PRCHistoricoInsumo { get; set; }
		public virtual DbSet<PlanoExportacaoDUEEntity> PlanoExportacaoDUE { get; set; }

		public DatabaseContextSciex()
					: base("name=DatabaseContextSciex")
		{

			
			// Disable Migrations
			// https://stackoverflow.com/questions/18667172/how-can-i-disable-migration-in-entity-framework-6-0
			System.Data.Entity.Database.SetInitializer<DatabaseContextSciex>(null);

			// Enable Migrations Database.SetInitializer(new
			// MigrateDatabaseToLatestVersion<DatabaseContext,
			// Suframa.Sciex.DataAccess.Database.Migrations.Configuration>());

			// Enable Log
			// https://cmatskas.com/logging-and-tracing-with-entity-framework-6/
			//DbInterception.Add(new DatabaseInterceptor());
			Database.Log = NLog.LogManager.GetLogger("db").Trace;
			SqlProviderServices.TruncateDecimalsToScale = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//modelBuilder.HasDefaultSchema(PrivateSettings.DEFAULT_DB_SCHEMA);

			modelBuilder.HasDefaultSchema("dbo");
			ConfigurePRCDue(modelBuilder);
			ConfigurePRCSolicDetalhe(modelBuilder);
			ConfigureTipoSolicAlteracao(modelBuilder);
			ConfigureProcesso(modelBuilder);
			ConfigurePRCSolicitacaoAlteracao(modelBuilder);
			ConfigurePRCStatus(modelBuilder);
			ConfigurePRCProduto(modelBuilder);
			ConfigurePRCProdutoPais(modelBuilder);
			ConfigurePRCInsumo(modelBuilder);
			ConfigurePRCDetalheInsumo(modelBuilder);
			ConfigurePRJUnidadeMedida(modelBuilder);
			ConfigurePRJProdutoEmpresaExportacao(modelBuilder);
			ConfigurePlanoExportacao(modelBuilder);
			ConfigurePlanoExportacaoProdutoPais(modelBuilder);
			ConfigurePlanoExportacaoInsumo(modelBuilder);
			ConfigurePlanoExportacaoHistorico(modelBuilder);
			ConfigurePlanoExportacaoDetalheInsumo(modelBuilder);
			ConfigurePlanoExportacaoArquivo(modelBuilder);
			ConfigureRegimeTributario(modelBuilder);
			ConfigureAladi(modelBuilder);
			ConfigureNaladi(modelBuilder);
			ConfigureUnidadeReceitaFederal(modelBuilder);
			ConfigureFundamentoLegal(modelBuilder);
			ConfigureFornecedor(modelBuilder);
			ConfigureFabricante(modelBuilder);
			ConfigureAnalista(modelBuilder);
			ConfigureParidadeCambial(modelBuilder);
			ConfigureParidadeValor(modelBuilder);			
			ConfigureMoeda(modelBuilder);
			ConfigureImportador(modelBuilder);
			ConfigureListaServico(modelBuilder);
			ConfigureControleExecucaoServico(modelBuilder);
			ConfigureParametroConfiguracao(modelBuilder);
			ConfigureParametros(modelBuilder);
			ConfigureInstituicaoFinanceira(modelBuilder);
			ConfigureMotivo(modelBuilder);
			ConfigureIncoterms(modelBuilder);
			ConfigureModalidadePagamento(modelBuilder);
			ConfigureCodigoConta(modelBuilder);
			ConfigureTipoDeclaracao(modelBuilder);
			ConfigureCodigoUtilizacao(modelBuilder);
			ConfigureRegimeTributarioMercadoria(modelBuilder);
			ConfigureControleImportacao(modelBuilder);
			ConfigureNcm(modelBuilder);
			ConfigurePLI(modelBuilder);
			ConfigurePliAnaliseVisual(modelBuilder);
			ConfigurePliAnaliseVisualAnexo(modelBuilder);
			ConfigurePLIAplicacao(modelBuilder);
			ConfigurePLIMercadoria(modelBuilder);					
			ConfigureViewNcm(modelBuilder);
			ConfigureViewProdutoEmpresa(modelBuilder);			
			ConfigureViewMercadoria(modelBuilder);
			ConfigureViewDetalheMercadoria(modelBuilder);
			ConfigureViewUnidadeMedida(modelBuilder);
			ConfigureNcmExcecao(modelBuilder);
			ConfigurePLIProduto(modelBuilder);
			ConfigureOrgaoAnuente(modelBuilder);
			ConfigurePLIProcessoAnuente(modelBuilder);
			ConfigureViewImportador(modelBuilder);
			ConfigurePLIDetalheMercadoria(modelBuilder);
			ConfigureTaxaEmpresaAtuacao(modelBuilder);
			ConfigureTaxaFatoGerador(modelBuilder);
			ConfigureTaxaGrupoBeneficio(modelBuilder);
			ConfigureTaxaNCMBeneficio(modelBuilder);
			ConfigureTaxaPli(modelBuilder);
			ConfigureTaxaPliDetalheMercadoria(modelBuilder);
			ConfigureTaxaPliHistorico(modelBuilder);
			ConfigureTaxaPliMercadoria(modelBuilder);
			ConfigureAli(modelBuilder);
			ConfigureAliArquivoEnvio(modelBuilder);
			ConfigureLi(modelBuilder);
			ConfigureLiSubstituida(modelBuilder);
			ConfigureLiArquivoRetorno(modelBuilder);
			ConfigureLiHistoricoErro(modelBuilder);
			ConfigureDi(modelBuilder);
			ConfigurerRepresentacao(modelBuilder);
			ConfigureCodigoLancamento(modelBuilder);
			ConfigureLancamento(modelBuilder);
			ConfigureErroMensagem(modelBuilder);
			ConfigureErroProcessamento(modelBuilder);
			ConfigureViewRelatorioAnaliseProcessamentoPli(modelBuilder);
			ConfigureAliArquivoEnvio(modelBuilder);
			ConfigureLiArquivo(modelBuilder);
			ConfigureAliEntradaArquivo(modelBuilder);
			ConfigureEstruturaPropriaPLI(modelBuilder);
			ConfigureEstruturaPropriaPLIArquivo(modelBuilder);
			ConfigureSolicitacaoFornecedorFabricante(modelBuilder);
			ConfigureSolicitacaoPliProcessoAnuente(modelBuilder);
			ConfigureAliHistorico(modelBuilder);
			ConfigureTaxaPliDebito(modelBuilder);
			ConfigurePliFornecedorFabricante(modelBuilder);
			ConfigureAuditoria(modelBuilder);
			ConfigureAuditoriaAplicacao(modelBuilder);
			ConfigureSequencial(modelBuilder);
			ConfigureDiLiEntity(modelBuilder);
			ConfigureDiEmbalagemEntradaEntity(modelBuilder);
			ConfigureDiEmbalagemEntity(modelBuilder);
			ConfigureDiAdicaoEntradaEntity(modelBuilder);
			ConfigureDiArmazemEntity(modelBuilder);
			ConfigureDiArmazemEntradaEntity(modelBuilder);
			ConfigureDiEntradaEntity(modelBuilder);
			ConfigureDiArquivoEntradaEntity(modelBuilder);
			ConfigureDiArquivoEntity(modelBuilder);
			ConfigureViaTransporteEntity(modelBuilder);
			ConfigureTipoEmbalagem(modelBuilder);
			ConfigureRecintoAlfandega(modelBuilder);
			ConfigureSetorArmazenamento(modelBuilder);
			ConfigureViewProdutoEmpresaExportacao(modelBuilder);
			ConfigureLEProduto(modelBuilder);
			ConfigureLEProdutoHistorico(modelBuilder);
			ConfigureLEInsumo(modelBuilder);
			ConfigureEstruturaPropriaLE(modelBuilder);
			ConfigureSolicitacaoLeInsumo(modelBuilder);
			ConfigureViewInsumoPadrao(modelBuilder);
			ConfigurePlanoExportacaoProduto(modelBuilder);
			ConfigureViewPais(modelBuilder);
			ConfigureSolicitacaoPE(modelBuilder);
			ConfigureParecerTecnico(modelBuilder);
			ConfigureParecerComplementar(modelBuilder);
			ConfigureParecerTecnicoProduto(modelBuilder);
			ConfigureSTParecerTecnico(modelBuilder);
			ConfigurePRCSolicProrrogacao(modelBuilder);
			ConfigurePRCHistoricoInsumo(modelBuilder);
			ConfigurePlanoExportacaoDue(modelBuilder);
		}

		public void DetachEntries()
		{
			foreach (var entry in this.ChangeTracker.Entries())
			{
				entry.State = System.Data.Entity.EntityState.Detached;
			}
		}

		/// <summary>
		/// https://stackoverflow.com/questions/16437083/dbcontext-discard-changes-without-disposing
		/// </summary>
		public void DiscartChanges()
		{
			foreach (var entry in this.ChangeTracker.Entries().Where(w => w.State != System.Data.Entity.EntityState.Unchanged))
			{
				switch (entry.State)
				{
					case System.Data.Entity.EntityState.Modified:
					case System.Data.Entity.EntityState.Deleted:
						entry.State = System.Data.Entity.EntityState.Modified; //Revert changes made to deleted entity.
						entry.State = System.Data.Entity.EntityState.Unchanged;
						break;

					case System.Data.Entity.EntityState.Added:
						entry.State = System.Data.Entity.EntityState.Detached;
						break;
				}
			}
		}

		[ValidateAntiForgeryToken]
		public override int SaveChanges()
		{
			try
			{
				var objectStateEntries = ChangeTracker.Entries()
				.Where(e => e.Entity is IData && (e.State == System.Data.Entity.EntityState.Modified || e.State == System.Data.Entity.EntityState.Added)).ToList();
				var currentTime = DateTime.Now;

				foreach (var entry in objectStateEntries)
				{
					var entityBase = entry.Entity as IData;
					if (entityBase == null) continue;
					if (entry.State == System.Data.Entity.EntityState.Added)
					{
						entityBase.DataInclusao = currentTime;
					}
					else
					{
						// No reescrever a data de incluso quando no for operao de incluso
						entry.Property(nameof(IData.DataInclusao)).IsModified = false;
					}

					entityBase.DataAlteracao = currentTime;
				}
				var id = base.SaveChanges();
				return id;
			}
			catch (DbEntityValidationException ex)
			{
				string a = ex.Message;


				if (a == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
					throw new StackOverflowException(ex.Message);
				else
					throw new StackOverflowException(Resources.ESTE_REGISTRO_NAO_PODE_SER_ALTERADO);

			}
			catch (DbUpdateConcurrencyException ex)
			{
				string a = ex.Message;
				throw new StackOverflowException(Resources.ESTE_REGISTRO_NAO_PODE_SER_ALTERADO);
			}
			catch (DataException ex /* dex */)
			{
				var erroInterno = ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "" : "" ;
				throw new StackOverflowException("Error Desconhecido: "+ ex.Message+" erro interno: "+ erroInterno);
				//Log the error (uncomment dex variable name after DataException and add a line here to write a log.
			}
			catch (Exception ex)
			{
				throw new StackOverflowException("0");
			}
			return base.SaveChanges();
		}
	}
}