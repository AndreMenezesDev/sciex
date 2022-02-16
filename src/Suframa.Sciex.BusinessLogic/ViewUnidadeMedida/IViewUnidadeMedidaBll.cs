using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewUnidadeMedidaBll
	{
		IEnumerable<object> ListarChave(ViewUnidadeMedidaVM viewUnidadeMedidaVM);


	}
}