using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IMenuBll
	{
		IEnumerable<IGrouping<string, MenuDto>> Listar();
	}
}