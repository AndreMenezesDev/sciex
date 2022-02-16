using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using Suframa.Sciex.DataAccess.Database;
using System.Collections.Generic;

namespace Suframa.Sciex.DataAccess
{
	public interface ICommandStackSciex
	{
		ICommandRepositorySciex<RegimeTributarioEntity> RegimeTributario { get; }
		ICommandRepositorySciex<AladiEntity> Aladi { get; }
		ICommandRepositorySciex<NaladiEntity> Naladi { get; }
		ICommandRepositorySciex<UnidadeReceitaFederalEntity> UnidadeReceitaFederal { get; }
		ICommandRepositorySciex<FundamentoLegalEntity> FundamentoLegal { get; }
		ICommandRepositorySciex<AnalistaEntity> Analista { get; }
		ICommandRepositorySciex<ParametroAnalista1Entity> ParametroAnalista1 { get; }
		ICommandRepositorySciex<FornecedorEntity> Fornecedor { get; }
		ICommandRepositorySciex<FabricanteEntity> Fabricante { get; }
		ICommandRepositorySciex<ParidadeCambialEntity> ParidadeCambial { get; }
		ICommandRepositorySciex<ParidadeValorEntity> ParidadeValor { get; }		
		ICommandRepositorySciex<MoedaEntity> Moeda { get; }
		ICommandRepositorySciex<ImportadorEntity> Importador { get; }
		ICommandRepositorySciex<ListaServicoEntity> ListaServico { get; }
		ICommandRepositorySciex<ControleExecucaoServicoEntity> ControleExecucaoServico { get; }
		ICommandRepositorySciex<ParametroConfiguracaoEntity> ParametroConfiguracao { get; }
		ICommandRepositorySciex<ParametrosEntity> Parametros { get; }
		ICommandRepositorySciex<InstituicaoFinanceiraEntity> InstituicaoFinanceira { get; }
		ICommandRepositorySciex<MotivoEntity> Motivo { get; }
		ICommandRepositorySciex<IncotermsEntity> Incoterms { get; }
		ICommandRepositorySciex<ModalidadePagamentoEntity> ModalidadePagamento { get; }
		ICommandRepositorySciex<CodigoContaEntity> CodigoConta { get; }
		ICommandRepositorySciex<TipoDeclaracaoEntity> TipoDeclaracao { get; }
		ICommandRepositorySciex<CodigoUtilizacaoEntity> CodigoUtilizacao { get; }
		ICommandRepositorySciex<RegimeTributarioMercadoriaEntity> RegimeTributarioMercadoria { get; }
		ICommandRepositorySciex<ControleImportacaoEntity> ControleImportacao { get; }
		ICommandRepositorySciex<ViewSetorEntity> ViewSetor { get; }
		ICommandRepositorySciex<NcmEntity> Ncm { get; }
		ICommandRepositorySciex<ViewNcmEntity> ViewNcm { get; }
		ICommandRepositorySciex<ViewAtividadeEconomicaPrincipalEntity> ViewAtividadeEconomicaPrincipal { get; }
		ICommandRepositorySciex<ViewSetorEmpresaEntity> ViewSetorEmpresa { get; }
		ICommandRepositorySciex<PliEntity> Pli { get; }
		ICommandRepositorySciex<PliAnaliseVisualEntity> PliAnaliseVisual { get; }
		ICommandRepositorySciex<PliAnaliseVisualAnexoEntity> PliAnaliseVisualAnexo { get; }
		ICommandRepositorySciex<PliAplicacaoEntity> PliAplicacao { get; }				
		ICommandRepositorySciex<PliProcessoAnuenteEntity> PliProcessoAnuente { get; }
		ICommandRepositorySciex<PliHistoricoEntity> PliHistorico { get; }
		ICommandRepositorySciex<PliDetalheMercadoriaEntity> PliDetalheMercadoria { get; }
		ICommandRepositorySciex<PliMercadoriaEntity> PliMercadoria { get; }		
		ICommandRepositorySciex<NcmExcecaoEntity> NcmExcecao { get; }
		ICommandRepositorySciex<PliProdutoEntity> PliProduto { get; }
		ICommandRepositorySciex<OrgaoAnuenteEntity> OrgaoAnuente { get; }
		ICommandRepositorySciex<TaxaEmpresaAtuacaoEntity> TaxaEmpresaAtuacao { get; }
		ICommandRepositorySciex<TaxaFatoGeradorEntity> TaxaFatoGerador { get; }
		ICommandRepositorySciex<TaxaGrupoBeneficioEntity> TaxaGrupoBeneficio { get; }
		ICommandRepositorySciex<TaxaNCMBeneficioEntity> TaxaNCMBeneficio { get; }
		ICommandRepositorySciex<TaxaPliDetalheMercadoriaEntity> TaxaPliDetalheMercadoria { get; }
		ICommandRepositorySciex<TaxaPliEntity> TaxaPli { get; }
		ICommandRepositorySciex<TaxaPliDebitoEntity> TaxaPliDebito { get; }
		ICommandRepositorySciex<TaxaPliHistoricoEntity> TaxaPliHistorico { get; }
		ICommandRepositorySciex<TaxaPliMercadoriaEntity> TaxaPliMercadoria { get; }
		ICommandRepositorySciex<ErroProcessamentoEntity> ErroProcessamento { get; }
		ICommandRepositorySciex<AliEntity> Ali { get; }
		ICommandRepositorySciex<AliArquivoEnvioEntity> AliArquivoEnvio { get; }
		ICommandRepositorySciex<LiEntity> Li { get; }
		ICommandRepositorySciex<LiSubstituidaEntity> LiSubstituida { get; }
		ICommandRepositorySciex<LiArquivoRetornoEntity> LiArquivoRetorno { get; }
		ICommandRepositorySciex<DiEntity> Di { get; }
		ICommandRepositorySciex<CodigoLancamentoEntity> CodigoLancamento { get; }
		ICommandRepositorySciex<LancamentoEntity> Lancamento { get; }
		ICommandRepositorySciex<AliArquivoEntity> AliArquivo { get; }
		ICommandRepositorySciex<LiArquivoEntity> LiArquivo { get; }
		ICommandRepositorySciex<LiHistoricoErroEntity> LiHistoricoErro { get; }
		ICommandRepositorySciex<AliEntradaArquivoEntity> AliEntradaArquivo { get; }
		ICommandRepositorySciex<EstruturaPropriaPliEntity> EstruturaPropriaPli { get; }
		ICommandRepositorySciex<EstruturaPropriaPliArquivoEntity> EstruturaPropriaPliArquivo { get; }
		ICommandRepositorySciex<SolicitacaoFornecedorFabricanteEntity> SolicitacaoFornecedorFabricante { get; }
		ICommandRepositorySciex<SolicitacaoPliProcessoAnuenteEntity> SolicitacaoPliProcessoAnuente { get; }
		ICommandRepositorySciex<SolicitacaoPliEntity> SolicitacaoPli { get; }		
		ICommandRepositorySciex<SolicitacaoPliMercadoriaEntity> SolicitacaoPliMercadoria { get; }
		ICommandRepositorySciex<SolicitacaoPliDetalheMercadoriaEntity> SolicitacaoPliDetalheMercadoria { get; }
		ICommandRepositorySciex<AliHistoricoEntity> AliHistorico { get; }
		ICommandRepositorySciex<PliFornecedorFabricanteEntity> PliFornecedorFabricante { get; }
		ICommandRepositorySciex<AuditoriaEntity> Auditoria { get; }
		ICommandRepositorySciex<AuditoriaAplicacaoEntity> AuditoriaAplicacao { get; }

		ICommandRepositorySciex<SequencialEntity> Sequencial { get; }
		ICommandRepositorySciex<DiLiEntity> DiLi { get; }
		ICommandRepositorySciex<DiEmbalagemEntradaEntity> DiEmbalagemEntrada { get; }
		ICommandRepositorySciex<DiEmbalagemEntity> DiEmbalagem { get; }
		ICommandRepositorySciex<DiArmazemEntity> DiArmazem { get; }		
		ICommandRepositorySciex<DiAdicaoEntradaEntity> DiAdicaoEntrada { get; }
		ICommandRepositorySciex<DiArmazemEntradaEntity> DiArmazemEntrada { get; }
		ICommandRepositorySciex<DiEntradaEntity> DiEntrada { get; }
		ICommandRepositorySciex<DiArquivoEntradaEntity> DiArquivoEntrada { get; }
		ICommandRepositorySciex<DiArquivoEntity> DiArquivo { get; }
		ICommandRepositorySciex<ViaTransporteEntity> ViaTransporte { get; }
		ICommandRepositorySciex<TipoEmbalagemEntity> TipoEmbalagem { get; }
		ICommandRepositorySciex<RecintoAlfandegaEntity> RecintoAlfandega { get; }
		ICommandRepositorySciex<SetorArmazenamentoEntity> SetorArmazenamento { get; }
		ICommandRepositorySciex<LEProdutoEntity> LEProduto { get; }
		ICommandRepositorySciex<LEProdutoHistoricoEntity> LEProdutoHistorico { get; }
		ICommandRepositorySciex<LEInsumoEntity> LEInsumo { get; }
		ICommandRepositorySciex<LEInsumoErroEntity> LEInsumoErro { get; }
		ICommandRepositorySciex<EstruturaPropriaLEEntity> EstruturaPropriaLE { get; }
		ICommandRepositorySciex<SolicitacaoLeInsumoEntity> SolicitacaoLeInsumo { get; }
		ICommandRepositorySciex<PlanoExportacaoEntity> PlanoExportacao { get; }
		ICommandRepositorySciex<PEProdutoEntity> PlanoExportacaoProduto { get; }
		ICommandRepositorySciex<PEProdutoPaisEntity> PlanoExportacaoProdutoPais { get; }
		ICommandRepositorySciex<PEArquivoEntity> PEArquivo { get; }
		ICommandRepositorySciex<PEHistoricoEntity> PEHistorico { get; }
		ICommandRepositorySciex<PEInsumoEntity> PEInsumo { get; }
		ICommandRepositorySciex<PEDetalheInsumoEntity> PEDetalheInsumo { get; }
		ICommandRepositorySciex<PRJProdutoEmpresaExportacaoEntity> PRJProdutoEmpresaExportacao { get; }
		ICommandRepositorySciex<PRJUnidadeMedidaEntity> PRJUnidadeMedida { get; }
		ICommandRepositorySciex<ProcessoEntity> Processo { get; }
		ICommandRepositorySciex<PRCStatusEntity> PRCStatus { get; }
		ICommandRepositorySciex<PRCProdutoEntity> PRCProduto { get; }
		ICommandRepositorySciex<PRCProdutoPaisEntity> PRCProdutoPais { get; }
		ICommandRepositorySciex<PRCInsumoEntity> PRCInsumo { get; }
		ICommandRepositorySciex<PRCDetalheInsumoEntity> PRCDetalheInsumo { get; }
		ICommandRepositorySciex<PRCSolicitacaoAlteracaoEntity> PRCSolicitacaoAlteracao { get; }
		ICommandRepositorySciex<PRCSolicDetalheEntity> PRCSolicDetalhe { get; }
		ICommandRepositorySciex<TipoSolicAlteracaoEntity> TipoSolicAlteracao { get; }
		
		ICommandRepositorySciex<SolicitacaoPEInsumoEntity> SolicitacaoPEInsumo { get; }
		ICommandRepositorySciex<SolicitacaoPEDetalheEntity> SolicitacaoPEDetalhe { get; }
		ICommandRepositorySciex<SolicitacaoPEProdutoEntity> SolicitacaoPEProduto { get; }
		ICommandRepositorySciex<SolicitacaoPaisProdutoEntity> SolicitacaoPaisProduto { get; }
		ICommandRepositorySciex<SolicitacaoPELoteEntity> SolicitacaoPELote { get; }
		ICommandRepositorySciex<SolicitacaoPEArquivoEntity> SolicitacaoPEArquivo { get; }

		ICommandRepositorySciex<ParecerTecnicoEntity> ParecerTecnico { get; }
		ICommandRepositorySciex<ParecerComplementarEntity> ParecerComplementar { get; }

		ICommandRepositorySciex<ParecerTecnicoProdutoEntity> ParecerTecnicoProduto { get; }
		ICommandRepositorySciex<ST_ParecerTecnicoEntity> StoreProcedureParecerTecnico { get; }
	    ICommandRepositorySciex<PRCSolicProrrogacaoEntity> ProcessoSolicitacaoProrrogacao { get; }
		ICommandRepositorySciex<PRCHistoricoInsumoEntity> PRCHistoricoInsumo { get; }
		ICommandRepositorySciex<PRCDetalheHistoricoInsumoEntity> PRCDetalheHistoricoInsumo { get; }
		ICommandRepositorySciex<PlanoExportacaoDUEEntity> PlanoExportacaoDue { get; }


		void DetachEntries();

		void Discart();

		void Save();

		void ExcluirParidadeCambial(int idParidadeCambial);

		void CopiarParidadeCambialValor(int idParidadeCambialNew, int idParidadeCambialOld);

		void SalvarParidadeCambial(string sql);

		string Salvar(string sql, string sql1);

		Int64 Salvar(string sql);

		List<string> ListarCnpjsLisDeferidas();

		List<LiEntity> ListarLiPorCnpj(string cnpj);
	}
}