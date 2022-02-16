using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IServicoGerarParecerSuspensaoBll
	{
		ParecerTecnicoVM GerarParecerSuspensaoCancelado(GerarParecerSuspensaoVM view);
		ParecerTecnicoVM GerarParecerSuspensaoAlterado(GerarParecerSuspensaoVM view);

	}
}