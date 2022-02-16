using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class RegimeTributarioExcluirValidation : RegimeTributarioValidation<RegimeTributarioDto>
	{
		public RegimeTributarioExcluirValidation()
		{
			ValidarRegimeTributarioExcluir();
		}
	}
}