using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IRegimeTributarioMercadoriaBll
	{
		IEnumerable<RegimeTributarioMercadoriaVM> Listar(RegimeTributarioMercadoriaVM regimeTributarioMercadoriaVM);

		PagedItems<RegimeTributarioMercadoriaVM> ListarPaginado(RegimeTributarioMercadoriaVM pagedFilter);

		void Salvar(RegimeTributarioMercadoriaVM regimeTributarioMercadoriaVM);

		RegimeTributarioMercadoriaVM Selecionar(int? idRegimeTributarioMercadoria);

		void Deletar(int id);

		IEnumerable<object> ListarUF();

		IEnumerable<object> ListarRegimeTributario();
	}
}