using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ISolicitacaoPliDetalheMercadoriaBll
	{
		IEnumerable<SolicitacaoPliDetalheMercadoriaVM> Listar(SolicitacaoPliDetalheMercadoriaVM solicitacaoPliDetalheMercadoriaVM);
		SolicitacaoPliDetalheMercadoriaVM Salvar(SolicitacaoPliDetalheMercadoriaVM solicitacaoPliDetalheMercadoriaVM);
	}
}