using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class AladiExcluirValidation : AladiValidation<AladiDto>
	{
		public AladiExcluirValidation()
		{
			ValidarAladiExcluir();
		}
	}
}