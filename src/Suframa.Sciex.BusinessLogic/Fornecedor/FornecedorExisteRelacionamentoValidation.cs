using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class FornecedorExisteRelacionamentoValidation : FornecedorValidation<FornecedorDto>
	{
		public FornecedorExisteRelacionamentoValidation()
		{
			ValidarFornecedorExisteRelacionamento();
		}
	}
}