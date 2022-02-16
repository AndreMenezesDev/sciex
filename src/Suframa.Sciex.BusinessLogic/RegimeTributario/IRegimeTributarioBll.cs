using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IRegimeTributarioBll
	{
		PagedItems<RegimeTributarioVM> ListarPaginado(RegimeTributarioVM pagedFilter);

		void Salvar(RegimeTributarioVM regimeTributarioVM);

		RegimeTributarioVM Selecionar(int? idRegimeTributario);

		RegimeTributarioVM Visualizar(RegimeTributarioVM regimeTributarioVM);

		void Deletar(int id);

		IEnumerable<object> ListarDrop();

		IEnumerable<object> ListarChave(RegimeTributarioVM regimeTributarioVM);

		IEnumerable<object> ListarDropOpcoesFixas();


	}
}