using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class UnidadeReceitaFederalExcluirValidation : UnidadeReceitaFederalValidation<UnidadeReceitaFederalDto>
	{
		public UnidadeReceitaFederalExcluirValidation()
		{
			ValidarUnidadeReceitaFederalExcluir();
		}
	}
}