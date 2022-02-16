using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewMunicipioBll
	{
		IEnumerable<ViewMunicipioVM> Listar(ViewMunicipioVM viewMunicipioVM);

		IEnumerable<object> ListarViewMunicipio();


	}
}