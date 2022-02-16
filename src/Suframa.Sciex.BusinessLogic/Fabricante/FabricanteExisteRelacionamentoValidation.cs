using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class FabricanteExisteRelacionamentoValidation : FabricanteValidation<FabricanteDto>
	{
		public FabricanteExisteRelacionamentoValidation()
		{
			ValidarFabricanteExisteRelacionamento();
		}
	}
}