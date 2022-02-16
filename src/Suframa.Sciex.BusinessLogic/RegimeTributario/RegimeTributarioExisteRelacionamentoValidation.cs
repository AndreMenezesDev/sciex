using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class RegimeTributarioExisteRelacionamentoValidation : RegimeTributarioValidation<RegimeTributarioDto>
	{
		public RegimeTributarioExisteRelacionamentoValidation()
		{
			ValidarRegimeTributarioExisteRelacionamento();
		}
	}
}