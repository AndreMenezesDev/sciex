using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliAplicacaoExisteRelacionamentoValidation : PliAplicacaoValidation<PliAplicacaoDto>
	{
		public PliAplicacaoExisteRelacionamentoValidation()
		{
			ValidarPliAplicacaoExisteRelacionamento();
		}
	}
}