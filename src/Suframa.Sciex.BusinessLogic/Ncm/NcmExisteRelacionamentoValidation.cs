using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class NcmExisteRelacionamentoValidation : NcmValidation<NcmDto>
	{
		public NcmExisteRelacionamentoValidation()
		{
			ValidarNcmExisteRelacionamento();
		}
	}
}