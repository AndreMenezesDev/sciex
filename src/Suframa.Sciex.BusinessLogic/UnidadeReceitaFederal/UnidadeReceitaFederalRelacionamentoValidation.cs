using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic
{
	public class UnidadeReceitaFederalExcluirRelacionamentoValidation : UnidadeReceitaFederalValidation<UnidadeReceitaFederalDto>
	{
		public UnidadeReceitaFederalExcluirRelacionamentoValidation()
		{
			ValidarUnidadeReceitaFederalExisteRelacionamento();
		}
	}
}