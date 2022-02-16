using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliExcluirValidation : PliValidation<PliDto>
	{
		public PliExcluirValidation()
		{
			ValidarPliExcluir();
		}
	}
}