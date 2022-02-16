using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IPaisBll
    {
		IEnumerable<PaisVM> Listar(PaisVM paisVM = null);

		IEnumerable<object> ListarPaises(PaisVM paisVM);

		string ListarDescricaoPais(string codigoPais);



	}
}