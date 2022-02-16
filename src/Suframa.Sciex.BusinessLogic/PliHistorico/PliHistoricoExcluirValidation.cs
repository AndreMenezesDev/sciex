using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliHistoricoExcluirValidation : PliHistoricoValidation<PliHistoricoDto>
	{
		public PliHistoricoExcluirValidation()
		{
			ValidarPliHistoricoExcluir();
		}
	}
}