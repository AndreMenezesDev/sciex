using Suframa.Sciex.DataAccess.Database;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Linq;
using Suframa.Sciex.CrossCutting.Extension;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using System.Runtime.Remoting.Contexts;
using System.Web.UI.WebControls;

namespace Suframa.Sciex.DataAccess
{
	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
	/// </summary>
	public class QueryStackSciex : IQueryStackSciex
	{
		private readonly IDatabaseContextSciex contextSciex;

		public IQueryRepositorySciex<RegimeTributarioEntity> RegimeTributario { get; }
		public IQueryRepositorySciex<AladiEntity> Aladi { get; }
		public IQueryRepositorySciex<NaladiEntity> Naladi { get; }
		public IQueryRepositorySciex<UnidadeReceitaFederalEntity> UnidadeReceitaFederal { get; }
		public IQueryRepositorySciex<FundamentoLegalEntity> FundamentoLegal { get; }
		public IQueryRepositorySciex<AnalistaEntity> Analista { get; }
		public IQueryRepositorySciex<ParametroAnalista1Entity> ParametroAnalista1 { get; }
		public IQueryRepositorySciex<FornecedorEntity> Fornecedor { get; }
		public IQueryRepositorySciex<FabricanteEntity> Fabricante { get; }
		public IQueryRepositorySciex<ParidadeCambialEntity> ParidadeCambial { get; }
		public IQueryRepositorySciex<ParidadeValorEntity> ParidadeValor { get; }		
		public IQueryRepositorySciex<MoedaEntity> Moeda { get; }
		public IQueryRepositorySciex<ImportadorEntity> Importador { get; }
		public IQueryRepositorySciex<ListaServicoEntity> ListaServico { get; }
		public IQueryRepositorySciex<ControleExecucaoServicoEntity> ControleExecucaoServico { get; }
		public IQueryRepositorySciex<ParametroConfiguracaoEntity> ParametroConfiguracao { get; }
		public IQueryRepositorySciex<ParametrosEntity> Parametros { get; }
		public IQueryRepositorySciex<InstituicaoFinanceiraEntity> InstituicaoFinanceira { get; }
		public IQueryRepositorySciex<MotivoEntity> Motivo { get; }
		public IQueryRepositorySciex<ModalidadePagamentoEntity> ModalidadePagamento { get; }
		public IQueryRepositorySciex<IncotermsEntity> Incoterms { get; }
		public IQueryRepositorySciex<CodigoContaEntity> CodigoConta { get; }
		public IQueryRepositorySciex<TipoDeclaracaoEntity> TipoDeclaracao { get; }
		public IQueryRepositorySciex<CodigoUtilizacaoEntity> CodigoUtilizacao { get; }
		public IQueryRepositorySciex<RegimeTributarioMercadoriaEntity> RegimeTributarioMercadoria { get; }
		public IQueryRepositorySciex<ControleImportacaoEntity> ControleImportacao { get; }
		public IQueryRepositorySciex<NcmEntity> Ncm { get; }
		public IQueryRepositorySciex<ViewNcmEntity> ViewNcm { get; }
		public IQueryRepositorySciex<ViewMercadoriaEntity> ViewMercadoria { get; }
		public IQueryRepositorySciex<ViewDetalheMercadoriaEntity> ViewDetalheMercadoria { get; }
		public IQueryRepositorySciex<ViewUnidadeMedidaEntity> ViewUnidadeMedida { get; }

		public IQueryRepositorySciex<PliEntity> Pli { get; }
		public IQueryRepositorySciex<PliAnaliseVisualEntity> PliAnaliseVisual { get; }
		public IQueryRepositorySciex<PliAnaliseVisualAnexoEntity> PliAnaliseVisualAnexo { get; }
		public IQueryRepositorySciex<PliAplicacaoEntity> PliAplicacao { get; }				
		public IQueryRepositorySciex<PliProcessoAnuenteEntity> PliProcessoAnuente { get; }
		public IQueryRepositorySciex<PliHistoricoEntity> PliHistorico { get; }
		public IQueryRepositorySciex<PliDetalheMercadoriaEntity> PliDetalheMercadoria { get; }
		public IQueryRepositorySciex<PliMercadoriaEntity> PliMercadoria { get; }
		public IQueryRepositorySciex<ViewProdutoEmpresaEntity> ViewProdutoEmpresa { get; }				
		public IQueryRepositorySciex<PliProdutoEntity> PliProduto { get; }
		public IQueryRepositorySciex<NcmExcecaoEntity> NcmExcecao { get; }
		public IQueryRepositorySciex<OrgaoAnuenteEntity> OrgaoAnuente { get; }
		public IQueryRepositorySciex<ViewAtividadeEconomicaPrincipalEntity> ViewAtividadeEconomicaPrincipal { get; }
		public IQueryRepositorySciex<ViewImportadorEntity> ViewImportador { get; }

		public IQueryRepositorySciex<TaxaEmpresaAtuacaoEntity> TaxaEmpresaAtuacao { get; }
		public IQueryRepositorySciex<TaxaFatoGeradorEntity> TaxaFatoGerador { get; }
		public IQueryRepositorySciex<TaxaGrupoBeneficioEntity> TaxaGrupoBeneficio { get; }
		public IQueryRepositorySciex<TaxaNCMBeneficioEntity> TaxaNCMBeneficio { get; }
		public IQueryRepositorySciex<TaxaPliDetalheMercadoriaEntity> TaxaPliDetalheMercadoria { get; }
		public IQueryRepositorySciex<TaxaPliEntity> TaxaPli { get; }
		public IQueryRepositorySciex<TaxaPliDebitoEntity> TaxaPliDebito { get; }
		public IQueryRepositorySciex<TaxaPliHistoricoEntity> TaxaPliHistorico { get; }
		public IQueryRepositorySciex<TaxaPliMercadoriaEntity> TaxaPliMercadoria { get; }

		public IQueryRepositorySciex<AliEntity> Ali { get; }
		public IQueryRepositorySciex<AliArquivoEnvioEntity> AliArquivoEnvio { get; }
		public IQueryRepositorySciex<LiEntity> Li { get; }
		public IQueryRepositorySciex<LiSubstituidaEntity> LiSubstituida { get; }
		public IQueryRepositorySciex<LiArquivoRetornoEntity> LiArquivoRetorno { get; }
		public IQueryRepositorySciex<DiEntity> Di { get; }
		public IQueryRepositorySciex<RepresentacaoEntity> Representacao { get; }

		public IQueryRepositorySciex<CodigoLancamentoEntity> CodigoLancamento { get; }
		public IQueryRepositorySciex<LancamentoEntity> Lancamento { get; }
		public IQueryRepositorySciex<ErroMensagemEntity> ErroMensagem { get; }
		public IQueryRepositorySciex<ErroProcessamentoEntity> ErroProcessamento { get; }
		public IQueryRepositorySciex<ViewRelatorioAnaliseProcessamentoPliEntity> ViewRelatorioAnaliseProcessamentoPli { get; }

		public IQueryRepositorySciex<AliArquivoEntity> AliArquivo { get; }
		public IQueryRepositorySciex<LiArquivoEntity> LiArquivo { get; }
		public IQueryRepositorySciex<LiHistoricoErroEntity> LiHistoricoErro { get; }

		public IQueryRepositorySciex<AliEntradaArquivoEntity> AliEntradaArquivo { get; }
		public IQueryRepositorySciex<EstruturaPropriaPliEntity> EstruturaPropriaPLI { get; }
		public IQueryRepositorySciex<EstruturaPropriaPliArquivoEntity> EstruturaPropriaPLIArquivo { get; }

		public IQueryRepositorySciex<SolicitacaoFornecedorFabricanteEntity> SolicitacaoFornecedorFabricante { get; }
		public IQueryRepositorySciex<SolicitacaoPliProcessoAnuenteEntity> SolicitacaoPliProcessoAnuente { get; }
		public IQueryRepositorySciex<SolicitacaoPliEntity> SolicitacaoPli { get; }		
		public IQueryRepositorySciex<SolicitacaoPliMercadoriaEntity> SolicitacaoPliMercadoria { get; }
		public IQueryRepositorySciex<SolicitacaoPliDetalheMercadoriaEntity> SolicitacaoPliDetalheMercadoria { get; }

		public IQueryRepositorySciex<AliHistoricoEntity> AliHistorico { get; }

		public IQueryRepositorySciex<PliFornecedorFabricanteEntity> PliFornecedorFabricante { get; }
		public IQueryRepositorySciex<AuditoriaEntity> Auditoria { get; }
		public IQueryRepositorySciex<AuditoriaAplicacaoEntity> AuditoriaAplicacao { get; }

		public IQueryRepositorySciex<SequencialEntity> Sequencial { get; }
		public IQueryRepositorySciex<DiLiEntity> DiLi { get; }
		public IQueryRepositorySciex<DiEmbalagemEntradaEntity> DiEmbalagemEntrada { get; }
		public IQueryRepositorySciex<DiEmbalagemEntity> DiEmbalagem { get; }
		public IQueryRepositorySciex<DiAdicaoEntradaEntity> DiAdicaoEntrada { get; }
		public IQueryRepositorySciex<DiArmazemEntity> DiArmazem { get; }
		public IQueryRepositorySciex<DiArmazemEntradaEntity> DiArmazemEntrada { get; }
		public IQueryRepositorySciex<DiEntradaEntity> DiEntrada { get; }
		public IQueryRepositorySciex<DiArquivoEntradaEntity> DiArquivoEntrada { get; }
		public IQueryRepositorySciex<DiArquivoEntity> DiArquivo { get; }
		public IQueryRepositorySciex<ViaTransporteEntity> ViaTransporte { get; }
		public IQueryRepositorySciex<TipoEmbalagemEntity> TipoEmbalagem { get; }
		public IQueryRepositorySciex<RecintoAlfandegaEntity> RecintoAlfandega { get; }
		public IQueryRepositorySciex<SetorArmazenamentoEntity> SetorArmazenamento { get; }
		public IQueryRepositorySciex<ViewProdutoEmpresaExportacaoEntity> ViewProdutoEmpresaExportacao { get; }
		public IQueryRepositorySciex<LEProdutoEntity> LEProduto { get; }
		public IQueryRepositorySciex<LEProdutoHistoricoEntity> LEProdutoHistorico { get; }
		public IQueryRepositorySciex<LEInsumoEntity> LEInsumo { get; }
		public IQueryRepositorySciex<LEInsumoErroEntity> LEInsumoErro { get; }
		public IQueryRepositorySciex<EstruturaPropriaLEEntity> EstruturaPropriaLE { get; }
		public IQueryRepositorySciex<SolicitacaoLeInsumoEntity> SolicitacaoLeInsumo { get; }
		public IQueryRepositorySciex<ViewInsumoPadraoEntity> ViewInsumoPadrao { get; }
		public IQueryRepositorySciex<PlanoExportacaoEntity> PlanoExportacao { get; }
		public IQueryRepositorySciex<PEProdutoEntity> PlanoExportacaoProduto { get; }
		public IQueryRepositorySciex<PEProdutoPaisEntity> PlanoExportacaoProdutoPais { get; }
		public IQueryRepositorySciex<PEArquivoEntity> PEArquivo { get; }
		public IQueryRepositorySciex<PEHistoricoEntity> PEHistorico { get; }
		public IQueryRepositorySciex<PEInsumoEntity> PEInsumo { get; }
		public IQueryRepositorySciex<PEDetalheInsumoEntity> PEDetalheInsumo { get; }
		public IQueryRepositorySciex<PaisEntity> ViewPais { get; }
		public IQueryRepositorySciex<PRJProdutoEmpresaExportacaoEntity> PRJProdutoEmpresaExportacao { get; }
		public IQueryRepositorySciex<PRJUnidadeMedidaEntity> PRJUnidadeMedida { get; }
		public IQueryRepositorySciex<ProcessoEntity> Processo { get; }
		public IQueryRepositorySciex<PRCStatusEntity> PRCStatus { get; }
		public IQueryRepositorySciex<PRCProdutoEntity> PRCProduto { get; }
		public IQueryRepositorySciex<PRCProdutoPaisEntity> PRCProdutoPais { get; }
		public IQueryRepositorySciex<PRCInsumoEntity> PRCInsumo { get; }
		public IQueryRepositorySciex<PRCDetalheInsumoEntity> PRCDetalheInsumo { get; }
		public IQueryRepositorySciex<PRCSolicitacaoAlteracaoEntity> PRCSolicitacaoAlteracao { get; }
		public IQueryRepositorySciex<PRCSolicDetalheEntity> PRCSolicDetalhe { get; }
		public IQueryRepositorySciex<TipoSolicAlteracaoEntity> TipoSolicAlteracao { get; }
		public IQueryRepositorySciex<SolicitacaoPEInsumoEntity> SolicitacaoPEInsumo { get; }
		public IQueryRepositorySciex<SolicitacaoPEDetalheEntity> SolicitacaoPEDetalhe { get; }
		public IQueryRepositorySciex<SolicitacaoPEProdutoEntity> SolicitacaoPEProduto { get; }
		public IQueryRepositorySciex<SolicitacaoPaisProdutoEntity> SolicitacaoPaisProduto { get; }
		public IQueryRepositorySciex<SolicitacaoPELoteEntity> SolicitacaoPELote { get; }
		public IQueryRepositorySciex<SolicitacaoPEArquivoEntity> SolicitacaoPEArquivo { get; }

		public IQueryRepositorySciex<ParecerTecnicoEntity> ParecerTecnico { get; }
		public IQueryRepositorySciex<ParecerComplementarEntity> ParecerComplementar { get; }
		public IQueryRepositorySciex<ParecerTecnicoProdutoEntity> ParecerTecnicoProduto { get; }
		public IQueryRepositorySciex<ST_ParecerTecnicoEntity> StoreProcedureParecerTecnico { get; }
		public IQueryRepositorySciex<PRCSolicProrrogacaoEntity> ProcessoSolicProrrogacao { get; }
		public IQueryRepositorySciex<PRCHistoricoInsumoEntity> PRCHistoricoInsumo { get; }
		public IQueryRepositorySciex<PRCDetalheHistoricoInsumoEntity> PRCDetalheHistoricoInsumo { get; }
		public IQueryRepositorySciex<PlanoExportacaoDUEEntity> PlanoExportacaoDue { get; }


		public QueryStackSciex(IDatabaseContextSciex databaseContextSciex)
		{
			contextSciex = databaseContextSciex;

			Processo = new QueryRepositorySciex<ProcessoEntity>(contextSciex);
			PlanoExportacaoDue = new QueryRepositorySciex<PlanoExportacaoDUEEntity>(contextSciex);
			TipoSolicAlteracao = new QueryRepositorySciex<TipoSolicAlteracaoEntity>(contextSciex);
			PRCSolicDetalhe = new QueryRepositorySciex<PRCSolicDetalheEntity>(contextSciex);
			PRCStatus = new QueryRepositorySciex<PRCStatusEntity>(contextSciex);
			PRCProduto = new QueryRepositorySciex<PRCProdutoEntity>(contextSciex);
			PRCProdutoPais = new QueryRepositorySciex<PRCProdutoPaisEntity>(contextSciex);
			PRCInsumo = new QueryRepositorySciex<PRCInsumoEntity>(contextSciex);
			PRCDetalheInsumo = new QueryRepositorySciex<PRCDetalheInsumoEntity>(contextSciex);
			PRCSolicitacaoAlteracao = new QueryRepositorySciex<PRCSolicitacaoAlteracaoEntity>(contextSciex);
			PRJProdutoEmpresaExportacao = new QueryRepositorySciex<PRJProdutoEmpresaExportacaoEntity>(contextSciex);
			PRJUnidadeMedida = new QueryRepositorySciex<PRJUnidadeMedidaEntity>(contextSciex);
			PlanoExportacao = new QueryRepositorySciex<PlanoExportacaoEntity>(contextSciex);
			PEArquivo = new QueryRepositorySciex<PEArquivoEntity>(contextSciex);
			PEHistorico = new QueryRepositorySciex<PEHistoricoEntity>(contextSciex);
			PEInsumo = new QueryRepositorySciex<PEInsumoEntity>(contextSciex);
			PEDetalheInsumo = new QueryRepositorySciex<PEDetalheInsumoEntity>(contextSciex);
			PlanoExportacaoProduto = new QueryRepositorySciex<PEProdutoEntity>(contextSciex);
			PlanoExportacaoProdutoPais = new QueryRepositorySciex<PEProdutoPaisEntity>(contextSciex);
			LEInsumoErro = new QueryRepositorySciex<LEInsumoErroEntity>(contextSciex);
			RegimeTributario = new QueryRepositorySciex<RegimeTributarioEntity>(contextSciex);
			Aladi = new QueryRepositorySciex<AladiEntity>(contextSciex);
			Naladi = new QueryRepositorySciex<NaladiEntity>(contextSciex);
			UnidadeReceitaFederal = new QueryRepositorySciex<UnidadeReceitaFederalEntity>(contextSciex);
			FundamentoLegal = new QueryRepositorySciex<FundamentoLegalEntity>(contextSciex);
			Analista = new QueryRepositorySciex<AnalistaEntity>(contextSciex);
			ParametroAnalista1 = new QueryRepositorySciex<ParametroAnalista1Entity>(contextSciex);
			Fornecedor = new QueryRepositorySciex<FornecedorEntity>(contextSciex);
			Fabricante = new QueryRepositorySciex<FabricanteEntity>(contextSciex);
			ParidadeCambial = new QueryRepositorySciex<ParidadeCambialEntity>(contextSciex);
			ParidadeValor = new QueryRepositorySciex<ParidadeValorEntity>(contextSciex);		
			Moeda = new QueryRepositorySciex<MoedaEntity>(contextSciex);
			Importador = new QueryRepositorySciex<ImportadorEntity>(contextSciex);
			Moeda = new QueryRepositorySciex<MoedaEntity>(contextSciex);
			ListaServico = new QueryRepositorySciex<ListaServicoEntity>(contextSciex);
			ControleExecucaoServico = new QueryRepositorySciex<ControleExecucaoServicoEntity>(contextSciex);
			ParametroConfiguracao = new QueryRepositorySciex<ParametroConfiguracaoEntity>(contextSciex);
			Parametros = new QueryRepositorySciex<ParametrosEntity>(contextSciex);
			InstituicaoFinanceira = new QueryRepositorySciex<InstituicaoFinanceiraEntity>(contextSciex);
			Motivo = new QueryRepositorySciex<MotivoEntity>(contextSciex);
			ModalidadePagamento = new QueryRepositorySciex<ModalidadePagamentoEntity>(contextSciex);
			Incoterms = new QueryRepositorySciex<IncotermsEntity>(contextSciex);
			CodigoConta = new QueryRepositorySciex<CodigoContaEntity>(contextSciex);
			TipoDeclaracao = new QueryRepositorySciex<TipoDeclaracaoEntity>(contextSciex);
			CodigoUtilizacao = new QueryRepositorySciex<CodigoUtilizacaoEntity>(contextSciex);
			RegimeTributarioMercadoria = new QueryRepositorySciex<RegimeTributarioMercadoriaEntity>(contextSciex);
			ControleImportacao = new QueryRepositorySciex<ControleImportacaoEntity>(contextSciex);
			Ncm = new QueryRepositorySciex<NcmEntity>(contextSciex);
			ViewNcm = new QueryRepositorySciex<ViewNcmEntity>(contextSciex);
			ViewMercadoria = new QueryRepositorySciex<ViewMercadoriaEntity>(contextSciex);
			NcmExcecao = new QueryRepositorySciex<NcmExcecaoEntity>(contextSciex);

			Pli = new QueryRepositorySciex<PliEntity>(contextSciex);
			PliAnaliseVisual = new QueryRepositorySciex<PliAnaliseVisualEntity>(contextSciex);
			PliAnaliseVisualAnexo = new QueryRepositorySciex<PliAnaliseVisualAnexoEntity>(contextSciex);
			PliAplicacao = new QueryRepositorySciex<PliAplicacaoEntity>(contextSciex);						
			PliProcessoAnuente = new QueryRepositorySciex<PliProcessoAnuenteEntity>(contextSciex);
			PliHistorico = new QueryRepositorySciex<PliHistoricoEntity>(contextSciex);
			PliDetalheMercadoria = new QueryRepositorySciex<PliDetalheMercadoriaEntity>(contextSciex);
			PliMercadoria = new QueryRepositorySciex<PliMercadoriaEntity>(contextSciex);
			ViewProdutoEmpresa = new QueryRepositorySciex<ViewProdutoEmpresaEntity>(contextSciex);			
			ViewDetalheMercadoria = new QueryRepositorySciex<ViewDetalheMercadoriaEntity>(contextSciex);
			ViewUnidadeMedida = new QueryRepositorySciex<ViewUnidadeMedidaEntity>(contextSciex);
			PliProduto = new QueryRepositorySciex<PliProdutoEntity>(contextSciex);
			OrgaoAnuente = new QueryRepositorySciex<OrgaoAnuenteEntity>(contextSciex);
			ViewImportador = new QueryRepositorySciex<ViewImportadorEntity>(contextSciex);

			TaxaEmpresaAtuacao = new QueryRepositorySciex<TaxaEmpresaAtuacaoEntity>(contextSciex);
			TaxaFatoGerador = new QueryRepositorySciex<TaxaFatoGeradorEntity>(contextSciex);
			TaxaGrupoBeneficio = new QueryRepositorySciex<TaxaGrupoBeneficioEntity>(contextSciex);
			TaxaNCMBeneficio = new QueryRepositorySciex<TaxaNCMBeneficioEntity>(contextSciex);
			TaxaPliDetalheMercadoria = new QueryRepositorySciex<TaxaPliDetalheMercadoriaEntity>(contextSciex);
			TaxaPli = new QueryRepositorySciex<TaxaPliEntity>(contextSciex);
			TaxaPliDebito = new QueryRepositorySciex<TaxaPliDebitoEntity>(contextSciex);
			TaxaPliHistorico = new QueryRepositorySciex<TaxaPliHistoricoEntity>(contextSciex);
			TaxaPliMercadoria = new QueryRepositorySciex<TaxaPliMercadoriaEntity>(contextSciex);

			Ali = new QueryRepositorySciex<AliEntity>(contextSciex);
			AliArquivoEnvio = new QueryRepositorySciex<AliArquivoEnvioEntity>(contextSciex);
			Li = new QueryRepositorySciex<LiEntity>(contextSciex);
			LiSubstituida = new QueryRepositorySciex<LiSubstituidaEntity>(contextSciex);
			LiArquivoRetorno = new QueryRepositorySciex<LiArquivoRetornoEntity>(contextSciex);
			Di = new QueryRepositorySciex<DiEntity>(contextSciex);

			CodigoLancamento = new QueryRepositorySciex<CodigoLancamentoEntity>(contextSciex);
			Lancamento = new QueryRepositorySciex<LancamentoEntity>(contextSciex);
			ErroMensagem = new QueryRepositorySciex<ErroMensagemEntity>(contextSciex);
			ErroProcessamento = new QueryRepositorySciex<ErroProcessamentoEntity>(contextSciex);
			ViewRelatorioAnaliseProcessamentoPli = new QueryRepositorySciex<ViewRelatorioAnaliseProcessamentoPliEntity>(contextSciex);
			Representacao = new QueryRepositorySciex<RepresentacaoEntity>(contextSciex);

			AliArquivo = new QueryRepositorySciex<AliArquivoEntity>(contextSciex);
			LiArquivo = new QueryRepositorySciex<LiArquivoEntity>(contextSciex);
			LiHistoricoErro = new QueryRepositorySciex<LiHistoricoErroEntity>(contextSciex);

			AliEntradaArquivo = new QueryRepositorySciex<AliEntradaArquivoEntity>(contextSciex);

			SolicitacaoFornecedorFabricante = new QueryRepositorySciex<SolicitacaoFornecedorFabricanteEntity>(contextSciex);
			SolicitacaoPliProcessoAnuente = new QueryRepositorySciex<SolicitacaoPliProcessoAnuenteEntity>(contextSciex);
			SolicitacaoPli = new QueryRepositorySciex<SolicitacaoPliEntity>(contextSciex);			
			SolicitacaoPliMercadoria = new QueryRepositorySciex<SolicitacaoPliMercadoriaEntity>(contextSciex);
			SolicitacaoPliDetalheMercadoria = new QueryRepositorySciex<SolicitacaoPliDetalheMercadoriaEntity>(contextSciex);

			EstruturaPropriaPLI = new QueryRepositorySciex<EstruturaPropriaPliEntity>(contextSciex);
			EstruturaPropriaPLIArquivo = new QueryRepositorySciex<EstruturaPropriaPliArquivoEntity>(contextSciex);

			AliHistorico = new QueryRepositorySciex<AliHistoricoEntity>(contextSciex);

			PliFornecedorFabricante = new QueryRepositorySciex<PliFornecedorFabricanteEntity>(contextSciex);
			Auditoria = new QueryRepositorySciex<AuditoriaEntity>(contextSciex);
			AuditoriaAplicacao = new QueryRepositorySciex<AuditoriaAplicacaoEntity>(contextSciex);

			Sequencial = new QueryRepositorySciex<SequencialEntity>(contextSciex);
			DiLi = new QueryRepositorySciex<DiLiEntity>(contextSciex);
			DiEmbalagemEntrada = new QueryRepositorySciex<DiEmbalagemEntradaEntity>(contextSciex);
			DiEmbalagem = new QueryRepositorySciex<DiEmbalagemEntity>(contextSciex);
			DiAdicaoEntrada = new QueryRepositorySciex<DiAdicaoEntradaEntity>(contextSciex);
			DiArmazem = new QueryRepositorySciex<DiArmazemEntity>(contextSciex);
			DiArmazemEntrada = new QueryRepositorySciex<DiArmazemEntradaEntity>(contextSciex);
			DiEntrada = new QueryRepositorySciex<DiEntradaEntity>(contextSciex);
			DiArquivoEntrada = new QueryRepositorySciex<DiArquivoEntradaEntity>(contextSciex);
			DiArquivo = new QueryRepositorySciex<DiArquivoEntity>(contextSciex);

			ViaTransporte = new QueryRepositorySciex<ViaTransporteEntity>(contextSciex);
			TipoEmbalagem = new QueryRepositorySciex<TipoEmbalagemEntity>(contextSciex);
			RecintoAlfandega = new QueryRepositorySciex<RecintoAlfandegaEntity>(contextSciex);
			SetorArmazenamento = new QueryRepositorySciex<SetorArmazenamentoEntity>(contextSciex);

			ViewProdutoEmpresaExportacao = new QueryRepositorySciex<ViewProdutoEmpresaExportacaoEntity>(contextSciex);

			LEProduto = new QueryRepositorySciex<LEProdutoEntity>(contextSciex);
			LEProdutoHistorico = new QueryRepositorySciex<LEProdutoHistoricoEntity>(contextSciex);
			LEInsumo = new QueryRepositorySciex<LEInsumoEntity>(contextSciex);

			EstruturaPropriaLE = new QueryRepositorySciex<EstruturaPropriaLEEntity>(contextSciex);
			SolicitacaoLeInsumo = new QueryRepositorySciex<SolicitacaoLeInsumoEntity>(contextSciex);

			ViewInsumoPadrao = new QueryRepositorySciex<ViewInsumoPadraoEntity>(contextSciex);
			PlanoExportacaoProduto = new QueryRepositorySciex<PEProdutoEntity>(contextSciex);

			ViewPais = new QueryRepositorySciex<PaisEntity>(contextSciex);

			SolicitacaoPEInsumo = new QueryRepositorySciex<SolicitacaoPEInsumoEntity>(contextSciex);
			SolicitacaoPEDetalhe = new QueryRepositorySciex<SolicitacaoPEDetalheEntity>(contextSciex);
			SolicitacaoPEProduto = new QueryRepositorySciex<SolicitacaoPEProdutoEntity>(contextSciex);
			SolicitacaoPaisProduto = new QueryRepositorySciex<SolicitacaoPaisProdutoEntity>(contextSciex);
			SolicitacaoPELote = new QueryRepositorySciex<SolicitacaoPELoteEntity>(contextSciex);
			SolicitacaoPEArquivo = new QueryRepositorySciex<SolicitacaoPEArquivoEntity>(contextSciex);

			ParecerTecnico = new QueryRepositorySciex<ParecerTecnicoEntity>(contextSciex);
			ParecerComplementar = new QueryRepositorySciex<ParecerComplementarEntity>(contextSciex);

			ParecerComplementar = new QueryRepositorySciex<ParecerComplementarEntity>(contextSciex);
			StoreProcedureParecerTecnico = new QueryRepositorySciex<ST_ParecerTecnicoEntity>(contextSciex);
			ProcessoSolicProrrogacao = new QueryRepositorySciex<PRCSolicProrrogacaoEntity>(contextSciex);
			PRCHistoricoInsumo = new QueryRepositorySciex<PRCHistoricoInsumoEntity>(contextSciex);
			PRCDetalheHistoricoInsumo = new QueryRepositorySciex<PRCDetalheHistoricoInsumoEntity>(contextSciex);
		}

		public IList<LiDto> VerificaLiDoImportador(long liNumero, string cnpj)
		{
			return contextSciex.VerificaLiDoImportador(liNumero, cnpj);
		}

		public IList<LiDto> SelecionarLiNuReferenciaPorAliId(long pme_id)
		{
			return contextSciex.SelecionarLiNuReferenciaPorAliId(pme_id);
		}

		public IList<LiDto> VerificaLiIndeferidoCancelado(string numeroLiReferencia)
		{
			return contextSciex.VerificaLiIndeferidoCancelado(numeroLiReferencia);
		}

		public IList<LiDto> SelecionarIdOrigemLiReferencia(string li)
		{
			return contextSciex.SelecionarIdOrigemLiReferencia(li);
		}

		public IList<ImportadorDto> ValidaLIReferenciaPertenceaImpotador(string importadorcodigo, string liNum)
		{
			return contextSciex.ValidaLIReferenciaPertenceaImpotador(importadorcodigo, liNum);
		}

		public IList<ParidadeCambialDto> ListarParidadeCambial(DateTime dtParidade, int idMoeda)
		{
			return contextSciex.ListarParidadeCambial(dtParidade, idMoeda);
		}

		public ParidadeCambialVM GetParidadeCambial(DateTime dtParidade)
		{
			return contextSciex.GetParidadeCambial(dtParidade);
		}
		public ParidadeCambialVM ConsultarExistenciaParidadePorData(DateTime dtParidade)
		{
			return contextSciex.ConsultarExistenciaParidadePorData(dtParidade);
		}

		public int VerificaAplicacaoLideReferencia(string li)
		{
			return contextSciex.VerificaAplicacaoLideReferencia(li);
		}
		public int ContarQuantidadeInsumoPorProdutoEInscricaoCad(int inscCadastral, int idLe, int StatusLeAlteracao = 0)
		{
			return contextSciex.ContarQuantidadeInsumoPorProdutoEInscricaoCad(inscCadastral, idLe, StatusLeAlteracao);
		}

		public PagedItems<T> ListarPaginadoSql<T>(string sql, PagedOptions pagedFilter)
		{
			var skip = (pagedFilter.Page.Value * pagedFilter.Size.Value) - (pagedFilter.Size.Value - 1);
			var ateh = (pagedFilter.Page.Value * pagedFilter.Size.Value);

			var orderAsc = pagedFilter.Reverse == true ? " DESC" : " ASC";

			if (string.IsNullOrEmpty(pagedFilter.Sort))
			{
				pagedFilter.Sort = "(Select 1)";
			}

			var vlQuery = "with unorderedItems as ( " + sql +
						  "),orderedItems as ( select row_number() over (order by " + pagedFilter.Sort + " " + orderAsc + " ) as rowNumber," +
						  "unorderedItems.* from unorderedItems)" +
						  "select  * from orderedItems where rowNumber between " + skip + " and " + ateh;

			var list = contextSciex.Database.SqlQuery<T>(vlQuery).ToList();

			PagedItems<T> paged = new PagedItems<T>();

			var _QdeReg = contextSciex.Database.SqlQuery<CounterVM>("Select Count(*) as QtdeReg From (" + sql + ") a").FirstOrDefault();

			paged.Total = _QdeReg.QtdeReg;

			paged.Items = list;

			return paged;
		}

		public IList<T> ListarSql<T>(string sql)
		{
			return contextSciex.Database.SqlQuery<T>(sql).ToList();
		}

		public long BuscarUltimoCodigoSeqPlanoExportacao(string cnpjEmpresaLogada, int anoCorrente)
		{
			return contextSciex.BuscarUltimoCodigoSeqPlanoExportacao(cnpjEmpresaLogada, anoCorrente);
		}

		public void IniciarStoreProcedureParecerTecnico(int IdProcesso)
		{
			contextSciex.SP_ParecerTecnico(IdProcesso);
		}
		public void IniciarStoreProcedureParecerSuspensaoAlterado(int IdProcesso, int IdSolicitacaoAlteracao)
		{
			contextSciex.SP_GerarParecerSuspensaoAlterado(IdProcesso, IdSolicitacaoAlteracao);
		}
		public void IniciarStoreProcedureGerarHistoricoInsumo(int IdProcesso, int IdSolicitacaoAlteracao, string NomeResponsavel)
		{
			contextSciex.SP_GerarParecerHistoricoInsumo(IdProcesso, IdSolicitacaoAlteracao, NomeResponsavel);
		}
		public void IniciarStoreProcedureParecerSuspensaoCancelado(int IdProcesso)
		{
			contextSciex.SP_GerarParecerSuspensaoCancelado(IdProcesso);
		}
	}
}