using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewDetalheMercadoriaBll
	{
		IEnumerable<object> ListarChave(ViewDetalheMercadoriaVM viewDetalheMercadoriaVM);

		PagedItems<ViewDetalheMercadoriaVM> ListagemPadrao(ViewDetalheMercadoriaVM viewDetalheMercadoriaVM);
	}
}