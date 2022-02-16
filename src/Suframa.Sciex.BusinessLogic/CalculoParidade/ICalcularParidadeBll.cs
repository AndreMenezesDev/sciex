using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.DeferirPlanoExportacao
{
	public interface ICalculoParidadeBll
	{
		decimal? CalcularParidade(int? codMoeda);
	}
}
