using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliProcessoAnuenteExisteRelacionamentoValidation : PliProcessoAnuenteValidation<PliProcessoAnuenteDto>
	{
		public PliProcessoAnuenteExisteRelacionamentoValidation()
		{
			ValidarPliProcessoAnuenteExisteRelacionamento();
		}
	}
}