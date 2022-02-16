using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.BusinessLogic.Pss;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IHistoricoProcessoBll
	{
		PagedItems<PRCSolicHistoricoVM> ListarPaginado(PRCStatusVM objeto);
	}
}
