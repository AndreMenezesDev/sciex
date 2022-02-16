using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class NaladiExcluirValidation : NaladiValidation<NaladiDto>
	{
		public NaladiExcluirValidation()
		{
			ValidarNaladiExcluir();
		}
	}
}