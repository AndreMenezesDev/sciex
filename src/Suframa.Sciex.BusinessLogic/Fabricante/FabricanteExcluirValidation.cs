using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class FabricanteExcluirValidation : FabricanteValidation<FabricanteDto>
	{
		public FabricanteExcluirValidation()
		{
			ValidarFabricanteExcluir();
		}
	}
}