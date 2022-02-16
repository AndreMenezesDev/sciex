using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IDesignarPliBll
	{
		IEnumerable<PliVM> Listar(PliVM pliVM);

		PagedItems<PliVM> ListarPaginado(PliVM pagedFilter);
		PagedItems<PRCSolicitacaoAlteracaoVM> ListarPaginadoSolicitacao(DesignarSolicitacaoVM pagedFilter);

		PagedItems<LEProdutoVM> ListarPaginadoLes(LEProdutoVM pagedFilter);

		PagedItems<LEProdutoVM> ListarPaginadoLesSql(LEProdutoVM pagedFilter);

		PagedItems<PlanoExportacaoVM> ListarPaginadoPlanosSql(PlanoExportacaoVM pagedFilter);

		LEProdutoVM DesignarAnalistaLe(ListaLeVM listaVM);
		PlanoExportacaoVM DesignarAnalistaPlano(ListaPlanoVM listaVM);
		PlanoExportacaoVM DesignarAnalistaSolicitacao(ListaSolicitacaoVM listaVM);
		PliVM Salvar(ListaPliVM pliVM);

		PliVM Selecionar(long? idPli);

		void Deletar(long id);

		IEnumerable<object> Listar();
	}
}