using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class NaladiDeletarValidation : NaladiValidation<NaladiDto>
	{
		public NaladiDeletarValidation()
		{
			ValidarNaladiExcluir();
		}
	}
}