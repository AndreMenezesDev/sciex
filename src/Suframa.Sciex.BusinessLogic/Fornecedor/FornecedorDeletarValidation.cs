using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class FornecedorDeletarValidation : FornecedorValidation<FornecedorDto>
	{
		public FornecedorDeletarValidation()
		{
			ValidarFornecedorExcluir();
		}
	}
}