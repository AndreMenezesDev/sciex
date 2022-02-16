using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IProcessoInsumoBll
	{
		PagedItems<PRCInsumoVM> ListarProcessoInsumosNacionalOuImportadoPorIdProcessoProduto(ListarProcessoInsumosNacionalImportadosVM vm);
		PagedItems<PRCDetalheInsumoVM> ListarProcessoInsumosNacionalOuImportadoPorIdInsumo(ListarProcessoInsumosNacionalImportadosVM vm);
		PRCProdutoVM BuscarInformacoesAdicionaisProduto(PRCProdutoVM vm);
		PRCInsumoVM SelecionarPrcInsumo(int idPEInsumo);
		DadosProcessoDetalhesInsumosVM ListarDetalhePorIdProcessoInsumo(ListarProcessoInsumosNacionalImportadosVM vm);
		List<PRCInsumoVM> BuscarValoresAtuais(BuscarValoresInsumoVM parametros);
		string ValidaSeUsuarioAlterouInsumos(ListarProcessoInsumosNacionalImportadosVM vm);
		SolicitacoesAlteracaoVM CalculaParidade(MoedaVM objeto);
		bool SalvarNovoDetalheAdicional(SalvarDetalhePRCInsumoVM vm);
		IEnumerable<object> ListarChave(CodigoDescricaoInsumoDropDownVM viewNcmVM);
		List<PRCHistoricoInsumoVM> SelecionarRelatorio(PRCHistoricoInsumoVM parametros);
	}
}