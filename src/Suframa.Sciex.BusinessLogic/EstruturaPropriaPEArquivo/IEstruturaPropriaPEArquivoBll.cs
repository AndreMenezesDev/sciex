using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IEstruturaPropriaPEArquivoBll
	{
		IEnumerable<EstruturaPropriaPLIArquivoVM> Listar(EstruturaPropriaPLIArquivoVM estruturaPropriaPLIArquivoVM);
		IEnumerable<object> ListarChave(EstruturaPropriaPLIArquivoVM estruturaPropriaPLIArquivoVM);
		PagedItems<EstruturaPropriaPLIArquivoVM> ListarPaginado(EstruturaPropriaPLIArquivoVM pagedFilter);
		int Salvar(EstruturaPropriaPLIArquivoVM estruturapropriaarquivo);
		EstruturaPropriaPLIArquivoVM Selecionar(int? idEstruturaPropriaPli);
		bool ValidarEmpresasRepresentadas(string cnpjEmpresa);
		string ValidarArquivo(EstruturaPropriaPLIArquivoVM estrutura);
		void ProcessarPE();
	}
}