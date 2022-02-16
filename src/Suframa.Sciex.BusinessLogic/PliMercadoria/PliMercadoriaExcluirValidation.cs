using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliMercadoriaExcluirValidation : PliMercadoriaValidation<PliMercadoriaDto>
	{
		public PliMercadoriaExcluirValidation()
		{
			ValidarPliMercadoriaExcluir();
		}
	}
}