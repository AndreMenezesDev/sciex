using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliDetalheMercadoriaExcluirValidation : PliDetalheMercadoriaValidation<PliDetalheMercadoriaDto>
	{
		public PliDetalheMercadoriaExcluirValidation()
		{
			ValidarPliDetalheMercadoriaExcluir();
		}
	}
}