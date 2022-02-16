using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class AladiExisteRelacionamentoValidation : AladiValidation<AladiDto>
	{
		public AladiExisteRelacionamentoValidation()
		{
			ValidarAladiExisteRelacionamento();
		}
	}
}