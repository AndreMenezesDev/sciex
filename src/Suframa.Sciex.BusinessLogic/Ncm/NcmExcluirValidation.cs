using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class NcmExcluirValidation : NcmValidation<NcmDto>
	{
		public NcmExcluirValidation()
		{
			ValidarNcmExcluir();
		}
	}
}