using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class FabricanteDeletarValidation : FabricanteValidation<FabricanteDto>
	{
		public FabricanteDeletarValidation()
		{
			ValidarFabricanteExcluir();
		}
	}
}