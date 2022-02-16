using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewMercadoriaBll
	{
		IEnumerable<object> ListarChave(ViewMercadoriaVM viewMercadoriaVM);
		PagedItems<ViewMercadoriaVM> ListarPaginado(ViewMercadoriaVM pagedFilter);
		ViewMercadoriaVM SelecionarNCM(ViewMercadoriaVM viewMercadoriaVM);
	}
}