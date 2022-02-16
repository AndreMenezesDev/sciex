using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic
{
	public class ParametrosExisteRelacionamentoValidation : ParametrosValidation<ParametrosDto>
	{
		public ParametrosExisteRelacionamentoValidation()
		{
			ValidarParametrosExisteRelacionamento();
		}
	}
}