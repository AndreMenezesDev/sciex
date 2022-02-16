using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IParidadeCambialBll
	{
		PagedItems<ParidadeCambialDto> ListarPaginado(ParidadeCambialPagedFilterVM pagedFilter);

		ParidadeCambialGenerator Gerar(ParidadeCambialGenerator paridadeCambialGenerator);

		ParidadeCambialGenerator BaixarArquivoParidadeRemoto(ParidadeCambialGenerator paridadeCambialGenerator);

		ParidadeCambialGenerator BaixarArquivoParidadeEmail(ParidadeCambialGenerator paridadeCambialGenerator);
	}
}