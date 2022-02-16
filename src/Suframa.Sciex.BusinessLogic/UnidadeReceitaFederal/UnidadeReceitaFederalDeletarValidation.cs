using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class UnidadeReceitaFederalDeletarValidation : UnidadeReceitaFederalValidation<UnidadeReceitaFederalDto>
	{
		public UnidadeReceitaFederalDeletarValidation()
		{
			ValidarUnidadeReceitaFederalExistente();
		}
	}
}