using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IAliArquivoBll
	{
		IEnumerable<AliArquivoVM> Listar(AliArquivoVM AliArquivoVM);
		AliArquivoVM RegrasSalvar(AliArquivoVM AliArquivoVM);
		AliArquivoVM Selecionar(long? idAliArquivo);
		void Deletar(long id);
		HttpResponseMessage GetAliArquivo(long idAliArquivo);
	}
}
