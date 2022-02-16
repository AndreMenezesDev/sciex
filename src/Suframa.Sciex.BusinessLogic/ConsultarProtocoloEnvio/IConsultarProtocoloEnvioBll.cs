using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IConsultarProtocoloEnvioBll
	{

		PagedItems<EstruturaPropriaPLIVM> ListarPaginado(EstruturaPropriaPLIVM pagedFilter);

		EstruturaPropriaPLIVM Selecionar(long? idEstruturaPropria);
	}
}