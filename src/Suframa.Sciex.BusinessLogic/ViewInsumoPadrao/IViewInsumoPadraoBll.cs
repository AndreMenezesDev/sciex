using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewInsumoPadraoBll
	{
		IEnumerable<object> ListarChave(ViewInsumoPadraoVM insumoPadraoVM);
		IEnumerable<object> ListarChaveParaNCM(ViewInsumoPadraoDropDown insumoPadraoVM);
		PagedItems<ViewInsumoPadraoVM> ListarPaginado(ViewInsumoPadraoVM pagedFilter);
		ViewInsumoPadraoVM SelecionarNCM(ViewInsumoPadraoVM insumoPadraoVM);
	}
}