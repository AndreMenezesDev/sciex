using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IParametrizarAnalistaBll
	{
		PagedItems<AnalistaVM> ListarPaginado(AnalistaVM pagedFilter);
		IEnumerable<object> ListarAnalistaPliDropDown();
		IEnumerable<object> ListarAnalistaLeDropDown();
		IEnumerable<object> ListarAnalistaPlanoDropDown();
		AnalistaVM Salvar(AnalistaVM vm);
	}
}