using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IEstruturaPropriaPliArquivoBll
	{
		IEnumerable<EstruturaPropriaPLIArquivoVM> Listar(EstruturaPropriaPLIArquivoVM estruturaPropriaPLIArquivoVM);
		IEnumerable<object> ListarChave(EstruturaPropriaPLIArquivoVM estruturaPropriaPLIArquivoVM);
		PagedItems<EstruturaPropriaPLIArquivoVM> ListarPaginado(EstruturaPropriaPLIArquivoVM pagedFilter);
		int Salvar(EstruturaPropriaPLIArquivoVM estruturapropriaarquivo);
		EstruturaPropriaPLIArquivoVM Selecionar(int? idEstruturaPropriaPli);
		void Deletar(int id);
		bool ValidarEstrutura(string[] arquivoLinhas, string[] dados);
		bool ValidarDataHoraArquivo(string data);
		bool ValidarItensPli();
		bool ValidarTipoPLi(string[] arquivoLinhas);
		bool ValidarPLIsEmpresa(string[] arquivoLinhas);
		bool ValidarEmpresasRepresentadas(string cnpjEmpresa);
		string ValidarArquivo(EstruturaPropriaPLIArquivoVM estrutura);
	}
}