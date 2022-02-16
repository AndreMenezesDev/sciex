using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPliMercadoriaBll
	{
		IEnumerable<PliMercadoriaVM> Listar(PliMercadoriaVM pliMercadoriaVM);

		PagedItems<PliMercadoriaVM> ListarPaginado(PliMercadoriaVM pagedFilter);

		PliMercadoriaVM Salvar(PliMercadoriaVM pliMercadoriaVM);

		PliMercadoriaVM Selecionar(long? idPliMercadoria);

		void Deletar(long id);

		IEnumerable<object> Listar();

		PliMercadoriaVM AtualizarNCM(PliMercadoriaVM pliMercadoria);
	}
}