using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewNcmBll
	{
		IEnumerable<object> ListarChave(ViewNcmVM viewNcmVM);
		ViewNcmVM Selecionar(int id);
	}
}