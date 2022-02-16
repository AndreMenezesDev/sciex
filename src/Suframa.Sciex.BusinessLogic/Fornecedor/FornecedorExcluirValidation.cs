using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class FornecedorExcluirValidation : FornecedorValidation<FornecedorDto>
	{
		public FornecedorExcluirValidation()
		{
			ValidarFornecedorExcluir();
		}
	}
}