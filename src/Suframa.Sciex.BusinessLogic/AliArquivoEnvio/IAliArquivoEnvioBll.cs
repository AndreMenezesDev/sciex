using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IAliArquivoEnvioBll
	{
		IEnumerable<AliArquivoEnvioVM> Listar(AliArquivoEnvioVM AliArquivoEnvioVM);
		AliArquivoEnvioVM RegrasSalvar(AliArquivoEnvioVM AliArquivoEnvioVM);
		AliArquivoEnvioVM Selecionar(long? idAliArquivoEnvio);
		void Deletar(long id);
		PagedItems<AliArquivoEnvioVM> ListarPaginado(AliArquivoEnvioVM pagedFilter);
	}
}
