using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ISolicitacaoPliProcessoAnuenteBll
	{
		IEnumerable<SolicitacaoPliProcessoAnuenteVM> Listar(SolicitacaoPliProcessoAnuenteVM solicitacaoPliProcessoAnuenteVM);
		SolicitacaoPliProcessoAnuenteVM Salvar(SolicitacaoPliProcessoAnuenteVM solicitacaoPliProcessoAnuenteVM);
	}
}