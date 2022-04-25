using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.Database;
using System;
using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using System.Web.UI.WebControls;

namespace Suframa.Sciex.DataAccess
{
	public interface IQueryStackSciex
	{
		IQueryRepositorySciex<SolicitacaoPEDueEntity> SolicitacaoPEDue { get; }
		IQueryRepositorySciex<PRCDueEntity> PRCDue { get; }
		IQueryRepositorySciex<RegimeTributarioEntity> RegimeTributario { get; }
		IQueryRepositorySciex<AladiEntity> Aladi { get; }
		IQueryRepositorySciex<NaladiEntity> Naladi { get; }
		IQueryRepositorySciex<UnidadeReceitaFederalEntity> UnidadeReceitaFederal { get; }
		IQueryRepositorySciex<FundamentoLegalEntity> FundamentoLegal { get; }
		IQueryRepositorySciex<FabricanteEntity> Fabricante { get; }
		IQueryRepositorySciex<AnalistaEntity> Analista { get; }
		IQueryRepositorySciex<ParametroAnalista1Entity> ParametroAnalista1 { get; }
		IQueryRepositorySciex<FornecedorEntity> Fornecedor { get; }
		IQueryRepositorySciex<ParidadeCambialEntity> ParidadeCambial { get; }
		IQueryRepositorySciex<ParidadeValorEntity> ParidadeValor { get; }		
		IQueryRepositorySciex<MoedaEntity> Moeda { get; }
		IQueryRepositorySciex<ImportadorEntity> Importador { get; }
		IQueryRepositorySciex<ListaServicoEntity> ListaServico { get; }
		IQueryRepositorySciex<ControleExecucaoServicoEntity> ControleExecucaoServico { get; }
		IQueryRepositorySciex<ParametroConfiguracaoEntity> ParametroConfiguracao { get; }
		IQueryRepositorySciex<ParametrosEntity> Parametros { get; }
		IQueryRepositorySciex<InstituicaoFinanceiraEntity> InstituicaoFinanceira { get; }
		IQueryRepositorySciex<MotivoEntity> Motivo { get; }
		IQueryRepositorySciex<IncotermsEntity> Incoterms { get; }
		IQueryRepositorySciex<ModalidadePagamentoEntity> ModalidadePagamento { get; }
		IQueryRepositorySciex<CodigoContaEntity> CodigoConta { get; }
		IQueryRepositorySciex<TipoDeclaracaoEntity> TipoDeclaracao { get; }
		IQueryRepositorySciex<CodigoUtilizacaoEntity> CodigoUtilizacao { get; }
		IQueryRepositorySciex<RegimeTributarioMercadoriaEntity> RegimeTributarioMercadoria { get; }
		IQueryRepositorySciex<ControleImportacaoEntity> ControleImportacao { get; }		
		IQueryRepositorySciex<NcmEntity> Ncm { get; }
		IQueryRepositorySciex<ViewNcmEntity> ViewNcm { get; }
		IQueryRepositorySciex<ViewProdutoEmpresaEntity> ViewProdutoEmpresa { get; }
		IQueryRepositorySciex<ViewMercadoriaEntity> ViewMercadoria { get; }
		IQueryRepositorySciex<ViewDetalheMercadoriaEntity> ViewDetalheMercadoria { get; }
		IQueryRepositorySciex<ViewUnidadeMedidaEntity> ViewUnidadeMedida { get; }

		IQueryRepositorySciex<PliEntity> Pli { get; }
		IQueryRepositorySciex<PliAnaliseVisualEntity> PliAnaliseVisual { get; }
		IQueryRepositorySciex<PliAnaliseVisualAnexoEntity> PliAnaliseVisualAnexo { get; }
		IQueryRepositorySciex<PliAplicacaoEntity> PliAplicacao { get; }				
		IQueryRepositorySciex<PliProcessoAnuenteEntity> PliProcessoAnuente { get; }
		IQueryRepositorySciex<PliHistoricoEntity> PliHistorico { get; }
		IQueryRepositorySciex<PliDetalheMercadoriaEntity> PliDetalheMercadoria { get; }
		IQueryRepositorySciex<PliMercadoriaEntity> PliMercadoria { get; }
		IQueryRepositorySciex<NcmExcecaoEntity> NcmExcecao { get; }
		IQueryRepositorySciex<PliProdutoEntity> PliProduto { get; }
		IQueryRepositorySciex<OrgaoAnuenteEntity> OrgaoAnuente { get; }
		IQueryRepositorySciex<ViewAtividadeEconomicaPrincipalEntity> ViewAtividadeEconomicaPrincipal { get; }
		IQueryRepositorySciex<ViewImportadorEntity> ViewImportador { get; }

		IQueryRepositorySciex<TaxaEmpresaAtuacaoEntity> TaxaEmpresaAtuacao { get; }
		IQueryRepositorySciex<TaxaFatoGeradorEntity> TaxaFatoGerador { get; }
		IQueryRepositorySciex<TaxaGrupoBeneficioEntity> TaxaGrupoBeneficio { get; }
		IQueryRepositorySciex<TaxaNCMBeneficioEntity> TaxaNCMBeneficio { get; }
		IQueryRepositorySciex<TaxaPliDetalheMercadoriaEntity> TaxaPliDetalheMercadoria { get; }
		IQueryRepositorySciex<TaxaPliEntity> TaxaPli { get; }
		IQueryRepositorySciex<TaxaPliDebitoEntity> TaxaPliDebito { get; }
		IQueryRepositorySciex<TaxaPliHistoricoEntity> TaxaPliHistorico { get; }
		IQueryRepositorySciex<TaxaPliMercadoriaEntity> TaxaPliMercadoria { get; }
		IQueryRepositorySciex<ViewEmitirRelatorioAnalisadorDueEntity> ViewEmitirRelatorioAnalisadorDue { get; }

		IQueryRepositorySciex<AliEntity> Ali { get; }
		IQueryRepositorySciex<AliArquivoEntity> AliArquivo { get; }
		IQueryRepositorySciex<AliArquivoEnvioEntity> AliArquivoEnvio { get; }
		IQueryRepositorySciex<LiEntity> Li { get; }
		IQueryRepositorySciex<LiSubstituidaEntity> LiSubstituida { get; }
		IQueryRepositorySciex<LiArquivoRetornoEntity> LiArquivoRetorno { get; }
		IQueryRepositorySciex<DiEntity> Di { get; }
		IQueryRepositorySciex<RepresentacaoEntity> Representacao { get; }

		IQueryRepositorySciex<CodigoLancamentoEntity> CodigoLancamento { get; }
		IQueryRepositorySciex<LancamentoEntity> Lancamento { get; }
		IQueryRepositorySciex<ErroMensagemEntity> ErroMensagem { get; }
		IQueryRepositorySciex<ErroProcessamentoEntity> ErroProcessamento { get; }
		IQueryRepositorySciex<ViewRelatorioAnaliseProcessamentoPliEntity> ViewRelatorioAnaliseProcessamentoPli { get; }

		IQueryRepositorySciex<LiArquivoEntity> LiArquivo { get; }
		IQueryRepositorySciex<LiHistoricoErroEntity> LiHistoricoErro { get; }

		IQueryRepositorySciex<AliEntradaArquivoEntity> AliEntradaArquivo { get; }
		IQueryRepositorySciex<EstruturaPropriaPliEntity> EstruturaPropriaPLI { get; }
		IQueryRepositorySciex<EstruturaPropriaPliArquivoEntity> EstruturaPropriaPLIArquivo { get; }

		IQueryRepositorySciex<SolicitacaoFornecedorFabricanteEntity> SolicitacaoFornecedorFabricante { get; }
		IQueryRepositorySciex<SolicitacaoPliProcessoAnuenteEntity> SolicitacaoPliProcessoAnuente { get; }
		IQueryRepositorySciex<SolicitacaoPliEntity> SolicitacaoPli { get; }		
		IQueryRepositorySciex<SolicitacaoPliMercadoriaEntity> SolicitacaoPliMercadoria { get; }
		IQueryRepositorySciex<SolicitacaoPliDetalheMercadoriaEntity> SolicitacaoPliDetalheMercadoria { get; }

		IQueryRepositorySciex<AliHistoricoEntity> AliHistorico { get; }

		IQueryRepositorySciex<PliFornecedorFabricanteEntity> PliFornecedorFabricante { get; }
		IQueryRepositorySciex<AuditoriaEntity> Auditoria { get; }
		IQueryRepositorySciex<AuditoriaAplicacaoEntity> AuditoriaAplicacao { get; }

		IQueryRepositorySciex<EstruturaPropriaLEEntity>  EstruturaPropriaLE { get; }
		IQueryRepositorySciex<SolicitacaoLeInsumoEntity> SolicitacaoLeInsumo { get; }
		IQueryRepositorySciex<ViewInsumoPadraoEntity> ViewInsumoPadrao { get; }
		IQueryRepositorySciex<PlanoExportacaoEntity> PlanoExportacao { get; }
		IQueryRepositorySciex<PEProdutoEntity> PlanoExportacaoProduto { get; }
		IQueryRepositorySciex<PEProdutoPaisEntity> PlanoExportacaoProdutoPais { get; }
		IQueryRepositorySciex<PEArquivoEntity> PEArquivo { get; }
		IQueryRepositorySciex<PEHistoricoEntity> PEHistorico { get; }
		IQueryRepositorySciex<PEInsumoEntity> PEInsumo { get; }
		IQueryRepositorySciex<PEDetalheInsumoEntity> PEDetalheInsumo { get; }
		IQueryRepositorySciex<PaisEntity> ViewPais { get; }
		IQueryRepositorySciex<PRJProdutoEmpresaExportacaoEntity> PRJProdutoEmpresaExportacao { get; }
		IQueryRepositorySciex<PRJUnidadeMedidaEntity> PRJUnidadeMedida { get; }
		IQueryRepositorySciex<ProcessoEntity> Processo { get; }
		IQueryRepositorySciex<PRCStatusEntity> PRCStatus { get; }
		IQueryRepositorySciex<PRCProdutoEntity> PRCProduto { get; }
		IQueryRepositorySciex<PRCProdutoPaisEntity> PRCProdutoPais { get; }
		IQueryRepositorySciex<PRCInsumoEntity> PRCInsumo { get; }
		IQueryRepositorySciex<PRCDetalheInsumoEntity> PRCDetalheInsumo { get; }
		IQueryRepositorySciex<PRCSolicitacaoAlteracaoEntity> PRCSolicitacaoAlteracao { get; }
		IQueryRepositorySciex<PRCSolicDetalheEntity> PRCSolicDetalhe { get; }
		IQueryRepositorySciex<TipoSolicAlteracaoEntity> TipoSolicAlteracao { get; }
		IQueryRepositorySciex<ST_ParecerTecnicoEntity> StoreProcedureParecerTecnico { get; }
		IQueryRepositorySciex<PRCSolicProrrogacaoEntity> ProcessoSolicProrrogacao { get; }
		IQueryRepositorySciex<PRCHistoricoInsumoEntity> PRCHistoricoInsumo { get; }
		IQueryRepositorySciex<PRCDetalheHistoricoInsumoEntity> PRCDetalheHistoricoInsumo { get; }
		IQueryRepositorySciex<PlanoExportacaoDUEEntity> PlanoExportacaoDue { get; }



		IList<LiDto> VerificaLiDoImportador(long liNumero, string cnpj);

		IList<LiDto> SelecionarLiNuReferenciaPorAliId(long pme_id);

		IList<LiDto> VerificaLiIndeferidoCancelado(string numeroLiReferencia);

		IList<ImportadorDto> ValidaLIReferenciaPertenceaImpotador(string importadorcodigo, string liNum);

		IList<ParidadeCambialDto> ListarParidadeCambial(DateTime dtParidade, int idMoeda);

		ParidadeCambialVM GetParidadeCambial(DateTime dtParidade);

		PagedItems<T> ListarPaginadoSql<T>(string sql, PagedOptions pagedFilter);

		IList<LiDto> SelecionarIdOrigemLiReferencia(string li);

		int VerificaAplicacaoLideReferencia(string li);
		int ContarQuantidadeInsumoPorProdutoEInscricaoCad(int inscCadastral, int idLe, int StatusLeAlteracao = 0);
		long BuscarUltimoCodigoSeqPlanoExportacao(string cnpjEmpresaLogada, int anoCorrente);

		IQueryRepositorySciex<SequencialEntity> Sequencial { get; }

		IQueryRepositorySciex<DiLiEntity> DiLi { get; }

		IQueryRepositorySciex<DiEmbalagemEntradaEntity> DiEmbalagemEntrada { get; }

		IQueryRepositorySciex<DiEmbalagemEntity> DiEmbalagem { get; }

		IQueryRepositorySciex<DiAdicaoEntradaEntity> DiAdicaoEntrada { get; }

		IQueryRepositorySciex<DiArmazemEntity> DiArmazem { get; }

		IQueryRepositorySciex<DiArmazemEntradaEntity> DiArmazemEntrada { get; }

		IQueryRepositorySciex<DiEntradaEntity> DiEntrada { get; }

		IQueryRepositorySciex<DiArquivoEntradaEntity> DiArquivoEntrada { get; }

		IQueryRepositorySciex<DiArquivoEntity> DiArquivo { get; }

		IQueryRepositorySciex<ViaTransporteEntity> ViaTransporte { get;  }
		IQueryRepositorySciex<TipoEmbalagemEntity> TipoEmbalagem { get; }
		IQueryRepositorySciex<RecintoAlfandegaEntity> RecintoAlfandega { get; }
		IQueryRepositorySciex<SetorArmazenamentoEntity> SetorArmazenamento { get; }
		IQueryRepositorySciex<ViewProdutoEmpresaExportacaoEntity> ViewProdutoEmpresaExportacao { get; }

		IQueryRepositorySciex<LEProdutoEntity> LEProduto { get; }
		IQueryRepositorySciex<LEProdutoHistoricoEntity> LEProdutoHistorico { get; }
		IQueryRepositorySciex<LEInsumoEntity> LEInsumo { get; }
		IQueryRepositorySciex<LEInsumoErroEntity> LEInsumoErro { get; }

		IQueryRepositorySciex<SolicitacaoPEInsumoEntity> SolicitacaoPEInsumo { get; }
		IQueryRepositorySciex<SolicitacaoPEDetalheEntity> SolicitacaoPEDetalhe { get; }
		IQueryRepositorySciex<SolicitacaoPEProdutoEntity> SolicitacaoPEProduto { get; }
		IQueryRepositorySciex<SolicitacaoPEProdutoPaisEntity> SolicitacaoPaisProduto { get; }
		IQueryRepositorySciex<SolicitacaoPELoteEntity> SolicitacaoPELote { get; }
		IQueryRepositorySciex<SolicitacaoPEArquivoEntity> SolicitacaoPEArquivo { get; }
		IQueryRepositorySciex<ParecerTecnicoEntity> ParecerTecnico { get; }
		IQueryRepositorySciex<ParecerComplementarEntity> ParecerComplementar { get; }

		IQueryRepositorySciex<ParecerTecnicoProdutoEntity> ParecerTecnicoProduto { get; }
		void IniciarStoreProcedureParecerTecnico(int IdProcesso);
		void IniciarStoreProcedureParecerTecnico(int IdProcesso, bool IsTipoComprovacao);
		void IniciarStoreProcedureParecerSuspensaoAlterado(int IdProcesso, int IdSolicitacaoAlteracao);
		void IniciarStoreProcedureGerarHistoricoInsumo(int IdProcesso, int IdSolicitacaoAlteracao, string NomeResponsavel);
		void IniciarStoreProcedureParecerSuspensaoCancelado(int IdProcesso);
		ParidadeCambialVM ConsultarExistenciaParidadePorData(DateTime dtParidade);

		IList<T> ListarSql<T>(string sql);
	}
}