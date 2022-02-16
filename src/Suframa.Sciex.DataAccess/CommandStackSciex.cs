using Suframa.Sciex.DataAccess.Database;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Suframa.Sciex.DataAccess
{
	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
	/// </summary>
	public class CommandStackSciex : ICommandStackSciex
	{
		private readonly IDatabaseContextSciex contextSciex;

		public ICommandRepositorySciex<RegimeTributarioEntity> RegimeTributario { get; }
		public ICommandRepositorySciex<AladiEntity> Aladi { get; }
		public ICommandRepositorySciex<NaladiEntity> Naladi { get; }
		public ICommandRepositorySciex<UnidadeReceitaFederalEntity> UnidadeReceitaFederal { get; }
		public ICommandRepositorySciex<FundamentoLegalEntity> FundamentoLegal { get; }
		public ICommandRepositorySciex<FornecedorEntity> Fornecedor { get; }
		public ICommandRepositorySciex<FabricanteEntity> Fabricante { get; set; }
		public ICommandRepositorySciex<AnalistaEntity> Analista { get; }
		public ICommandRepositorySciex<ParametroAnalista1Entity> ParametroAnalista1 { get; }
		public ICommandRepositorySciex<ParidadeCambialEntity> ParidadeCambial { get; }
		public ICommandRepositorySciex<ParidadeValorEntity> ParidadeValor { get; }
		public ICommandRepositorySciex<MoedaEntity> Moeda { get; }
		public ICommandRepositorySciex<ImportadorEntity> Importador { get; }
		public ICommandRepositorySciex<ListaServicoEntity> ListaServico { get; }
		public ICommandRepositorySciex<ControleExecucaoServicoEntity> ControleExecucaoServico { get; }
		public ICommandRepositorySciex<ParametroConfiguracaoEntity> ParametroConfiguracao { get; }
		public ICommandRepositorySciex<ParametrosEntity> Parametros { get; }
		public ICommandRepositorySciex<InstituicaoFinanceiraEntity> InstituicaoFinanceira { get; }
		public ICommandRepositorySciex<MotivoEntity> Motivo { get; }
		public ICommandRepositorySciex<IncotermsEntity> Incoterms { get; }
		public ICommandRepositorySciex<ModalidadePagamentoEntity> ModalidadePagamento { get; }
		public ICommandRepositorySciex<CodigoContaEntity> CodigoConta { get; }
		public ICommandRepositorySciex<TipoDeclaracaoEntity> TipoDeclaracao { get; }
		public ICommandRepositorySciex<CodigoUtilizacaoEntity> CodigoUtilizacao { get; }
		public ICommandRepositorySciex<RegimeTributarioMercadoriaEntity> RegimeTributarioMercadoria { get; }
		public ICommandRepositorySciex<ControleImportacaoEntity> ControleImportacao { get; }
		public ICommandRepositorySciex<ViewSetorEntity> ViewSetor { get; }
		public ICommandRepositorySciex<NcmEntity> Ncm { get; }
		public ICommandRepositorySciex<ViewNcmEntity> ViewNcm { get; }
		public ICommandRepositorySciex<ViewSetorEmpresaEntity> ViewSetorEmpresa { get; }
		public ICommandRepositorySciex<ViewAtividadeEconomicaPrincipalEntity> ViewAtividadeEconomicaPrincipal { get; }
		public ICommandRepositorySciex<PliEntity> Pli { get; }
		public ICommandRepositorySciex<PliAnaliseVisualEntity> PliAnaliseVisual { get; }
		public ICommandRepositorySciex<PliAnaliseVisualAnexoEntity> PliAnaliseVisualAnexo { get; }
		public ICommandRepositorySciex<PliAplicacaoEntity> PliAplicacao { get; }
		public ICommandRepositorySciex<PliProcessoAnuenteEntity> PliProcessoAnuente { get; }
		public ICommandRepositorySciex<PliHistoricoEntity> PliHistorico { get; }
		public ICommandRepositorySciex<PliDetalheMercadoriaEntity> PliDetalheMercadoria { get; }
		public ICommandRepositorySciex<PliMercadoriaEntity> PliMercadoria { get; }
		public ICommandRepositorySciex<NcmExcecaoEntity> NcmExcecao { get; }
		public ICommandRepositorySciex<PliProdutoEntity> PliProduto { get; }
		public ICommandRepositorySciex<OrgaoAnuenteEntity> OrgaoAnuente { get; }

		public ICommandRepositorySciex<TaxaEmpresaAtuacaoEntity> TaxaEmpresaAtuacao { get; }
		public ICommandRepositorySciex<TaxaFatoGeradorEntity> TaxaFatoGerador { get; }
		public ICommandRepositorySciex<TaxaGrupoBeneficioEntity> TaxaGrupoBeneficio { get; }
		public ICommandRepositorySciex<TaxaNCMBeneficioEntity> TaxaNCMBeneficio { get; }
		public ICommandRepositorySciex<TaxaPliDetalheMercadoriaEntity> TaxaPliDetalheMercadoria { get; }
		public ICommandRepositorySciex<TaxaPliEntity> TaxaPli { get; }
		public ICommandRepositorySciex<TaxaPliDebitoEntity> TaxaPliDebito { get; }
		public ICommandRepositorySciex<TaxaPliHistoricoEntity> TaxaPliHistorico { get; }
		public ICommandRepositorySciex<TaxaPliMercadoriaEntity> TaxaPliMercadoria { get; }

		public ICommandRepositorySciex<AliEntity> Ali { get; }
		public ICommandRepositorySciex<AliArquivoEnvioEntity> AliArquivoEnvio { get; }
		public ICommandRepositorySciex<LiEntity> Li { get; }
		public ICommandRepositorySciex<LiSubstituidaEntity> LiSubstituida { get; }
		public ICommandRepositorySciex<LiArquivoRetornoEntity> LiArquivoRetorno { get; }
		public ICommandRepositorySciex<DiEntity> Di { get; }
		public ICommandRepositorySciex<CodigoLancamentoEntity> CodigoLancamento { get; }
		public ICommandRepositorySciex<LancamentoEntity> Lancamento { get; }
		public ICommandRepositorySciex<ErroProcessamentoEntity> ErroProcessamento { get; }
		public ICommandRepositorySciex<AliArquivoEntity> AliArquivo { get; }
		public ICommandRepositorySciex<LiArquivoEntity> LiArquivo { get; }
		public ICommandRepositorySciex<LiHistoricoErroEntity> LiHistoricoErro { get; }

		public ICommandRepositorySciex<AliEntradaArquivoEntity> AliEntradaArquivo { get; }
		public ICommandRepositorySciex<EstruturaPropriaPliEntity> EstruturaPropriaPli { get; }
		public ICommandRepositorySciex<EstruturaPropriaPliArquivoEntity> EstruturaPropriaPliArquivo { get; }

		public ICommandRepositorySciex<SolicitacaoFornecedorFabricanteEntity> SolicitacaoFornecedorFabricante { get; }
		public ICommandRepositorySciex<SolicitacaoPliProcessoAnuenteEntity> SolicitacaoPliProcessoAnuente { get; }
		public ICommandRepositorySciex<SolicitacaoPliEntity> SolicitacaoPli { get; }		
		public ICommandRepositorySciex<SolicitacaoPliMercadoriaEntity> SolicitacaoPliMercadoria { get; }
		public ICommandRepositorySciex<SolicitacaoPliDetalheMercadoriaEntity> SolicitacaoPliDetalheMercadoria { get; }

		public ICommandRepositorySciex<AliHistoricoEntity> AliHistorico { get; }

		public ICommandRepositorySciex<PliFornecedorFabricanteEntity> PliFornecedorFabricante { get; }
		public ICommandRepositorySciex<AuditoriaEntity> Auditoria { get; }
		public ICommandRepositorySciex<AuditoriaAplicacaoEntity> AuditoriaAplicacao { get; }
		public ICommandRepositorySciex<SequencialEntity> Sequencial { get; }
		public ICommandRepositorySciex<DiLiEntity> DiLi { get; }
		public ICommandRepositorySciex<DiEmbalagemEntradaEntity> DiEmbalagemEntrada { get; }
		public ICommandRepositorySciex<DiEmbalagemEntity> DiEmbalagem { get; }
		public ICommandRepositorySciex<DiAdicaoEntradaEntity> DiAdicaoEntrada { get; }
		public ICommandRepositorySciex<DiArmazemEntity> DiArmazem{ get; }
		public ICommandRepositorySciex<DiArmazemEntradaEntity> DiArmazemEntrada { get; }
		public ICommandRepositorySciex<DiEntradaEntity> DiEntrada { get; }
		public ICommandRepositorySciex<DiArquivoEntradaEntity> DiArquivoEntrada { get; }
		public ICommandRepositorySciex<DiArquivoEntity> DiArquivo { get; }

		public ICommandRepositorySciex<ViaTransporteEntity> ViaTransporte { get; }
		public ICommandRepositorySciex<TipoEmbalagemEntity> TipoEmbalagem { get; }
		public ICommandRepositorySciex<RecintoAlfandegaEntity> RecintoAlfandega { get; }
		public ICommandRepositorySciex<SetorArmazenamentoEntity> SetorArmazenamento { get; }

		public ICommandRepositorySciex<LEProdutoEntity> LEProduto { get; }
		public ICommandRepositorySciex<LEProdutoHistoricoEntity> LEProdutoHistorico { get; }
		public ICommandRepositorySciex<LEInsumoEntity> LEInsumo { get; }
		public ICommandRepositorySciex<LEInsumoErroEntity> LEInsumoErro { get; }

		public ICommandRepositorySciex<EstruturaPropriaLEEntity> EstruturaPropriaLE { get; }
		public ICommandRepositorySciex<SolicitacaoLeInsumoEntity> SolicitacaoLeInsumo { get; }
		public ICommandRepositorySciex<PlanoExportacaoEntity> PlanoExportacao { get; }
		public ICommandRepositorySciex<PEProdutoEntity> PlanoExportacaoProduto { get; }
		public ICommandRepositorySciex<PEProdutoPaisEntity> PlanoExportacaoProdutoPais { get; }
		public ICommandRepositorySciex<PEArquivoEntity> PEArquivo { get; }
		public ICommandRepositorySciex<PEHistoricoEntity> PEHistorico { get; }
		public ICommandRepositorySciex<PEInsumoEntity> PEInsumo { get; }
		public ICommandRepositorySciex<PEDetalheInsumoEntity> PEDetalheInsumo { get; }
		public ICommandRepositorySciex<PRJProdutoEmpresaExportacaoEntity> PRJProdutoEmpresaExportacao { get; }
		public ICommandRepositorySciex<PRJUnidadeMedidaEntity> PRJUnidadeMedida { get; }
		public ICommandRepositorySciex<ProcessoEntity> Processo { get; }
		public ICommandRepositorySciex<PRCStatusEntity> PRCStatus { get; }
		public ICommandRepositorySciex<PRCProdutoEntity> PRCProduto { get; }
		public ICommandRepositorySciex<PRCProdutoPaisEntity> PRCProdutoPais { get; }
		public ICommandRepositorySciex<PRCInsumoEntity> PRCInsumo { get; }
		public ICommandRepositorySciex<PRCDetalheInsumoEntity> PRCDetalheInsumo { get; }
		public ICommandRepositorySciex<PRCSolicitacaoAlteracaoEntity> PRCSolicitacaoAlteracao { get; }
		public ICommandRepositorySciex<PRCSolicDetalheEntity> PRCSolicDetalhe { get; }
		public ICommandRepositorySciex<TipoSolicAlteracaoEntity> TipoSolicAlteracao { get; }
		public ICommandRepositorySciex<SolicitacaoPEInsumoEntity> SolicitacaoPEInsumo { get; }
		public ICommandRepositorySciex<SolicitacaoPEDetalheEntity> SolicitacaoPEDetalhe { get; }
		public ICommandRepositorySciex<SolicitacaoPEProdutoEntity> SolicitacaoPEProduto { get; }
		public ICommandRepositorySciex<SolicitacaoPaisProdutoEntity> SolicitacaoPaisProduto { get; }
		public ICommandRepositorySciex<SolicitacaoPELoteEntity> SolicitacaoPELote { get; }
		public ICommandRepositorySciex<SolicitacaoPEArquivoEntity> SolicitacaoPEArquivo { get; }
		public ICommandRepositorySciex<ParecerTecnicoEntity> ParecerTecnico { get; }
		public ICommandRepositorySciex<ParecerComplementarEntity> ParecerComplementar { get; }
		public ICommandRepositorySciex<ParecerTecnicoProdutoEntity> ParecerTecnicoProduto { get; }
		public ICommandRepositorySciex<ST_ParecerTecnicoEntity> StoreProcedureParecerTecnico { get; }
		public ICommandRepositorySciex<PEProdutoEntity> PEProduto { get; }
		public ICommandRepositorySciex<PRCSolicProrrogacaoEntity> ProcessoSolicitacaoProrrogacao { get; }
		public ICommandRepositorySciex<PRCHistoricoInsumoEntity> PRCHistoricoInsumo { get; }
		public ICommandRepositorySciex<PRCDetalheHistoricoInsumoEntity> PRCDetalheHistoricoInsumo { get; }
		public ICommandRepositorySciex<PlanoExportacaoDUEEntity> PlanoExportacaoDue { get; }



		public CommandStackSciex(IDatabaseContextSciex databaseContextSciex)
		{
			contextSciex = databaseContextSciex;

			Processo = new CommandRepositorySciex<ProcessoEntity>(contextSciex);
			PlanoExportacaoDue = new CommandRepositorySciex<PlanoExportacaoDUEEntity>(contextSciex);
			StoreProcedureParecerTecnico = new CommandRepositorySciex<ST_ParecerTecnicoEntity>(contextSciex);
			PEProduto = new CommandRepositorySciex<PEProdutoEntity>(contextSciex);
			PRCStatus = new CommandRepositorySciex<PRCStatusEntity>(contextSciex);
			PRCProduto = new CommandRepositorySciex<PRCProdutoEntity>(contextSciex);
			PRCProdutoPais = new CommandRepositorySciex<PRCProdutoPaisEntity>(contextSciex);
			PRCInsumo = new CommandRepositorySciex<PRCInsumoEntity>(contextSciex);
			PRCDetalheInsumo = new CommandRepositorySciex<PRCDetalheInsumoEntity>(contextSciex);
			PRCSolicitacaoAlteracao = new CommandRepositorySciex<PRCSolicitacaoAlteracaoEntity>(contextSciex);
			PRCSolicDetalhe = new CommandRepositorySciex<PRCSolicDetalheEntity>(contextSciex);
			TipoSolicAlteracao = new CommandRepositorySciex<TipoSolicAlteracaoEntity>(contextSciex);
			PRJProdutoEmpresaExportacao = new CommandRepositorySciex<PRJProdutoEmpresaExportacaoEntity>(contextSciex);
			PRJUnidadeMedida = new CommandRepositorySciex<PRJUnidadeMedidaEntity>(contextSciex);
			PlanoExportacao = new CommandRepositorySciex<PlanoExportacaoEntity>(contextSciex);
			PlanoExportacaoProduto = new CommandRepositorySciex<PEProdutoEntity>(contextSciex);
			PlanoExportacaoProdutoPais = new CommandRepositorySciex<PEProdutoPaisEntity>(contextSciex);
			PEArquivo = new CommandRepositorySciex<PEArquivoEntity>(contextSciex);
			PEHistorico = new CommandRepositorySciex<PEHistoricoEntity>(contextSciex);
			PEInsumo = new CommandRepositorySciex<PEInsumoEntity>(contextSciex);
			PEDetalheInsumo = new CommandRepositorySciex<PEDetalheInsumoEntity>(contextSciex);
			LEInsumoErro = new CommandRepositorySciex<LEInsumoErroEntity>(contextSciex);
			RegimeTributario = new CommandRepositorySciex<RegimeTributarioEntity>(contextSciex);
			Aladi = new CommandRepositorySciex<AladiEntity>(contextSciex);
			Naladi = new CommandRepositorySciex<NaladiEntity>(contextSciex);
			UnidadeReceitaFederal = new CommandRepositorySciex<UnidadeReceitaFederalEntity>(contextSciex);
			FundamentoLegal = new CommandRepositorySciex<FundamentoLegalEntity>(contextSciex);
			Analista = new CommandRepositorySciex<AnalistaEntity>(contextSciex);
			ParametroAnalista1 = new CommandRepositorySciex<ParametroAnalista1Entity>(contextSciex);
			Fornecedor = new CommandRepositorySciex<FornecedorEntity>(contextSciex);
			Fabricante = new CommandRepositorySciex<FabricanteEntity>(contextSciex);
			ParidadeCambial = new CommandRepositorySciex<ParidadeCambialEntity>(contextSciex);
			ParidadeValor = new CommandRepositorySciex<ParidadeValorEntity>(contextSciex);
			Moeda = new CommandRepositorySciex<MoedaEntity>(contextSciex);
			Importador = new CommandRepositorySciex<ImportadorEntity>(contextSciex);
			Moeda = new CommandRepositorySciex<MoedaEntity>(contextSciex);
			ListaServico = new CommandRepositorySciex<ListaServicoEntity>(contextSciex);
			ControleExecucaoServico = new CommandRepositorySciex<ControleExecucaoServicoEntity>(contextSciex);
			ParametroConfiguracao = new CommandRepositorySciex<ParametroConfiguracaoEntity>(contextSciex);
			Parametros = new CommandRepositorySciex<ParametrosEntity>(contextSciex);
			InstituicaoFinanceira = new CommandRepositorySciex<InstituicaoFinanceiraEntity>(contextSciex);
			Motivo = new CommandRepositorySciex<MotivoEntity>(contextSciex);
			Incoterms = new CommandRepositorySciex<IncotermsEntity>(contextSciex);
			ModalidadePagamento = new CommandRepositorySciex<ModalidadePagamentoEntity>(contextSciex);
			CodigoConta = new CommandRepositorySciex<CodigoContaEntity>(contextSciex);
			TipoDeclaracao = new CommandRepositorySciex<TipoDeclaracaoEntity>(contextSciex);
			CodigoUtilizacao = new CommandRepositorySciex<CodigoUtilizacaoEntity>(contextSciex);
			RegimeTributarioMercadoria = new CommandRepositorySciex<RegimeTributarioMercadoriaEntity>(contextSciex);
			ControleImportacao = new CommandRepositorySciex<ControleImportacaoEntity>(contextSciex);
			ViewSetor = new CommandRepositorySciex<ViewSetorEntity>(contextSciex);
			Ncm = new CommandRepositorySciex<NcmEntity>(contextSciex);
			ViewNcm = new CommandRepositorySciex<ViewNcmEntity>(contextSciex);
			ViewSetorEmpresa = new CommandRepositorySciex<ViewSetorEmpresaEntity>(contextSciex);
			ViewAtividadeEconomicaPrincipal = new CommandRepositorySciex<ViewAtividadeEconomicaPrincipalEntity>(contextSciex);
			ProcessoSolicitacaoProrrogacao = new CommandRepositorySciex<PRCSolicProrrogacaoEntity>(contextSciex);

			Pli = new CommandRepositorySciex<PliEntity>(contextSciex);
			PliAnaliseVisual = new CommandRepositorySciex<PliAnaliseVisualEntity>(contextSciex);
			PliAnaliseVisualAnexo = new CommandRepositorySciex<PliAnaliseVisualAnexoEntity>(contextSciex);
			PliAplicacao = new CommandRepositorySciex<PliAplicacaoEntity>(contextSciex);
			PliProcessoAnuente = new CommandRepositorySciex<PliProcessoAnuenteEntity>(contextSciex);
			PliHistorico = new CommandRepositorySciex<PliHistoricoEntity>(contextSciex);
			PliDetalheMercadoria = new CommandRepositorySciex<PliDetalheMercadoriaEntity>(contextSciex);
			PliMercadoria = new CommandRepositorySciex<PliMercadoriaEntity>(contextSciex);
			NcmExcecao = new CommandRepositorySciex<NcmExcecaoEntity>(contextSciex);
			PliProduto = new CommandRepositorySciex<PliProdutoEntity>(contextSciex);
			OrgaoAnuente = new CommandRepositorySciex<OrgaoAnuenteEntity>(contextSciex);

			TaxaEmpresaAtuacao = new CommandRepositorySciex<TaxaEmpresaAtuacaoEntity>(contextSciex);
			TaxaFatoGerador = new CommandRepositorySciex<TaxaFatoGeradorEntity>(contextSciex);
			TaxaGrupoBeneficio = new CommandRepositorySciex<TaxaGrupoBeneficioEntity>(contextSciex);
			TaxaNCMBeneficio = new CommandRepositorySciex<TaxaNCMBeneficioEntity>(contextSciex);
			TaxaPliDetalheMercadoria = new CommandRepositorySciex<TaxaPliDetalheMercadoriaEntity>(contextSciex);
			TaxaPli = new CommandRepositorySciex<TaxaPliEntity>(contextSciex);
			TaxaPliDebito = new CommandRepositorySciex<TaxaPliDebitoEntity>(contextSciex);
			TaxaPliHistorico = new CommandRepositorySciex<TaxaPliHistoricoEntity>(contextSciex);
			TaxaPliMercadoria = new CommandRepositorySciex<TaxaPliMercadoriaEntity>(contextSciex);

			Ali = new CommandRepositorySciex<AliEntity>(contextSciex);
			AliArquivoEnvio = new CommandRepositorySciex<AliArquivoEnvioEntity>(contextSciex);
			Li = new CommandRepositorySciex<LiEntity>(contextSciex);
			LiSubstituida = new CommandRepositorySciex<LiSubstituidaEntity>(contextSciex);
			LiArquivoRetorno = new CommandRepositorySciex<LiArquivoRetornoEntity>(contextSciex);
			Di = new CommandRepositorySciex<DiEntity>(contextSciex);
			CodigoLancamento = new CommandRepositorySciex<CodigoLancamentoEntity>(contextSciex);
			Lancamento = new CommandRepositorySciex<LancamentoEntity>(contextSciex);
			ErroProcessamento = new CommandRepositorySciex<ErroProcessamentoEntity>(contextSciex);

			AliArquivo = new CommandRepositorySciex<AliArquivoEntity>(contextSciex);
			LiArquivo = new CommandRepositorySciex<LiArquivoEntity>(contextSciex);
			LiHistoricoErro = new CommandRepositorySciex<LiHistoricoErroEntity>(contextSciex);

			AliEntradaArquivo = new CommandRepositorySciex<AliEntradaArquivoEntity>(contextSciex);
			EstruturaPropriaPli = new CommandRepositorySciex<EstruturaPropriaPliEntity>(contextSciex);
			EstruturaPropriaPliArquivo = new CommandRepositorySciex<EstruturaPropriaPliArquivoEntity>(contextSciex);

			SolicitacaoFornecedorFabricante = new CommandRepositorySciex<SolicitacaoFornecedorFabricanteEntity>(contextSciex);
			SolicitacaoPliProcessoAnuente = new CommandRepositorySciex<SolicitacaoPliProcessoAnuenteEntity>(contextSciex);
			SolicitacaoPli = new CommandRepositorySciex<SolicitacaoPliEntity>(contextSciex);			
			SolicitacaoPliMercadoria = new CommandRepositorySciex<SolicitacaoPliMercadoriaEntity>(contextSciex);
			SolicitacaoPliDetalheMercadoria = new CommandRepositorySciex<SolicitacaoPliDetalheMercadoriaEntity>(contextSciex);

			AliHistorico = new CommandRepositorySciex<AliHistoricoEntity>(contextSciex);
			PliFornecedorFabricante = new CommandRepositorySciex<PliFornecedorFabricanteEntity>(contextSciex);
			Auditoria = new CommandRepositorySciex<AuditoriaEntity>(contextSciex);
			AuditoriaAplicacao = new CommandRepositorySciex<AuditoriaAplicacaoEntity>(contextSciex);

			Sequencial = new CommandRepositorySciex<SequencialEntity>(contextSciex);
			DiLi = new CommandRepositorySciex<DiLiEntity>(contextSciex);
			DiEmbalagemEntrada = new CommandRepositorySciex<DiEmbalagemEntradaEntity>(contextSciex);
			DiEmbalagem = new CommandRepositorySciex<DiEmbalagemEntity>(contextSciex);
			DiAdicaoEntrada = new CommandRepositorySciex<DiAdicaoEntradaEntity>(contextSciex);
			DiArmazem = new CommandRepositorySciex<DiArmazemEntity>(contextSciex);
			DiArmazemEntrada = new CommandRepositorySciex<DiArmazemEntradaEntity>(contextSciex);
			DiEntrada = new CommandRepositorySciex<DiEntradaEntity>(contextSciex);
			DiArquivoEntrada = new CommandRepositorySciex<DiArquivoEntradaEntity>(contextSciex);
			DiArquivo = new CommandRepositorySciex<DiArquivoEntity>(contextSciex);

			ViaTransporte = new CommandRepositorySciex<ViaTransporteEntity>(contextSciex);
			TipoEmbalagem = new CommandRepositorySciex<TipoEmbalagemEntity>(contextSciex);
			RecintoAlfandega = new CommandRepositorySciex<RecintoAlfandegaEntity>(contextSciex);
			SetorArmazenamento = new CommandRepositorySciex<SetorArmazenamentoEntity>(contextSciex);
			LEProduto = new CommandRepositorySciex<LEProdutoEntity>(contextSciex);
			LEProdutoHistorico = new CommandRepositorySciex<LEProdutoHistoricoEntity>(contextSciex);
			LEInsumo = new CommandRepositorySciex<LEInsumoEntity>(contextSciex);
			EstruturaPropriaLE = new CommandRepositorySciex<EstruturaPropriaLEEntity>(contextSciex);
			SolicitacaoLeInsumo = new CommandRepositorySciex<SolicitacaoLeInsumoEntity>(contextSciex);
			PlanoExportacaoProduto = new CommandRepositorySciex<PEProdutoEntity>(contextSciex);
			SolicitacaoPEInsumo = new CommandRepositorySciex<SolicitacaoPEInsumoEntity>(contextSciex);
			SolicitacaoPEDetalhe = new CommandRepositorySciex<SolicitacaoPEDetalheEntity>(contextSciex);
			SolicitacaoPEProduto = new CommandRepositorySciex<SolicitacaoPEProdutoEntity>(contextSciex);
			SolicitacaoPaisProduto = new CommandRepositorySciex<SolicitacaoPaisProdutoEntity>(contextSciex);
			SolicitacaoPELote = new CommandRepositorySciex<SolicitacaoPELoteEntity>(contextSciex);
			SolicitacaoPEArquivo = new CommandRepositorySciex<SolicitacaoPEArquivoEntity>(contextSciex);

			ParecerTecnico = new CommandRepositorySciex<ParecerTecnicoEntity>(contextSciex);
			ParecerComplementar = new CommandRepositorySciex<ParecerComplementarEntity>(contextSciex);
			ParecerTecnicoProduto = new CommandRepositorySciex<ParecerTecnicoProdutoEntity>(contextSciex);
			PRCHistoricoInsumo = new CommandRepositorySciex<PRCHistoricoInsumoEntity>(contextSciex);
			PRCDetalheHistoricoInsumo = new CommandRepositorySciex<PRCDetalheHistoricoInsumoEntity>(contextSciex);
		}

		public void DetachEntries()
		{
			contextSciex.DetachEntries();
		}

		public void Discart()
		{
			contextSciex.DiscartChanges();
		}

		public void Save()
		{
			contextSciex.SaveChanges();
		}

		public void ExcluirParidadeCambial(int idParidadeCambial)
		{
			contextSciex.ExcluirParidadeCambial(idParidadeCambial);
		}

		public void CopiarParidadeCambialValor(int idParidadeCambialNew, int idParidadeCambialOld)
		{
			contextSciex.CopiarParidadeCambialValor(idParidadeCambialNew, idParidadeCambialOld);
		}

		public void SalvarParidadeCambial(string sql)
		{
			contextSciex.SalvarParidadeCambial(sql);
		}

		public Int64 Salvar(string sql)
		{
			Int64 newProdID = 0;
			var connString = System.Configuration.ConfigurationManager.ConnectionStrings["databasecontextsciex"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connString))
			{
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.CommandTimeout = 0;
				try
				{
					conn.Open();
					var resultado = cmd.ExecuteScalar();
					newProdID = resultado != null ? (Int64)resultado : 0;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			return (Int64)newProdID;
		}

		public string Salvar(string sql, string sql1)
		{
			string resultado = "";
			var connString = System.Configuration.ConfigurationManager.ConnectionStrings["databasecontextsciex"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connString))
			{
				SqlCommand cmd = new SqlCommand(sql, conn);
				try
				{
					cmd.CommandTimeout = 0;
					conn.Open();
					resultado = (string)cmd.ExecuteScalar();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			return (string)resultado;
		}

		public List<string> ListarCnpjsLisDeferidas()
		{
			return contextSciex.ListarCnpjsLisDeferidas();
		}

		public List<LiEntity> ListarLiPorCnpj(string cnpj)
		{
			return contextSciex.ListarLiPorCnpj(cnpj);
		}

	}
}