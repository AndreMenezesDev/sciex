using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPliDetalheMercadoriaBll
	{
		IEnumerable<PliDetalheMercadoriaVM> Listar(PliDetalheMercadoriaVM pliDetalheMercadoriaVM);

		PagedItems<PliDetalheMercadoriaVM> ListarPaginado(PliDetalheMercadoriaVM pagedFilter);

		void Salvar(PliDetalheMercadoriaVM pliDetalheMercadoriaVM);

		PliDetalheMercadoriaVM Selecionar(int? idPliDetalheMercadoria);

		void Deletar(long id);

		IEnumerable<object> Listar();
	}
}