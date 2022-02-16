using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliAplicacaoExcluirValidation : PliAplicacaoValidation<PliAplicacaoDto>
	{
		public PliAplicacaoExcluirValidation()
		{
			ValidarPliAplicacaoExcluir();
		}
	}
}