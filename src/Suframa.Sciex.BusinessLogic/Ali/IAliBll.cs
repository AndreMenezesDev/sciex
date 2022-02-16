using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IAliBll
	{
		IEnumerable<AliVM> Listar(AliVM aliVM);
		IEnumerable<object> ListarChave(AliVM aliVM);
		PagedItems<AliVM> ListarPaginado(AliVM pagedFilter);
		void Salvar(AliVM aliVM);
		AliVM Selecionar(int? numeroAli);
		AliVM Visualizar(AliVM aliVM);
		void Deletar(int id);
		void GerarAquivoEnvio();
		bool DowwloadArquivoAli();
		string EnviarArquivoALI();
		PagedItems<AliVM> ListarPaginadoRelatorioAli(AliVM pagedFilter);
		string GerarArquivoCancelamento();
		string EnviarArquivoALICancelamento();
	}
}
