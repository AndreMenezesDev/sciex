using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.DeferirPlanoExportacao
{
	public interface IDeferirPlanoExportacaoBll
	{
		ResultadoProcessamentoVM DeferirPlano(int IdPlanoExportacao);
	}
}
