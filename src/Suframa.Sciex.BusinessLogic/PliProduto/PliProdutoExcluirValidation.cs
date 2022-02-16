using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliProdutoExcluirValidation : PliProdutoValidation<PliProdutoDto>
	{
		public PliProdutoExcluirValidation()
		{
			ValidarPliProdutoExcluir();
		}
	}
}