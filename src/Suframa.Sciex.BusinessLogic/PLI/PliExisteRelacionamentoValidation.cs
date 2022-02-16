using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliExisteRelacionamentoValidation : PliValidation<PliDto>
	{
		public PliExisteRelacionamentoValidation()
		{
			ValidarPliExisteRelacionamento();
		}
	}
}