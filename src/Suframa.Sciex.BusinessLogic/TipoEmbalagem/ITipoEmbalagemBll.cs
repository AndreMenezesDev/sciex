using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ITipoEmbalagemBll
	{
		PagedItems<TipoEmbalagemVM> ListarPaginado(TipoEmbalagemVM pagedFilter);
		TipoEmbalagemVM VerificaCodigoCadastrado(TipoEmbalagemVM pagedFilter);
		TipoEmbalagemVM SelecionarEmbalagem(int id);
		int Deletar(int id);
		int Salvar(TipoEmbalagemVM objeto);
		int AtualizarStatus(TipoEmbalagemVM objeto);
	}
}
