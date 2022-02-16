using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ICancelarLiBll
	{
		IEnumerable<LiVM> Listar();
		IEnumerable<object> ListarChave(LiVM liVM);
		PagedItems<LiVM> ListarPaginado(LiVM pagedFilter);
		void Salvar(LiVM liVM);
		LiVM Selecionar(long? numeroLI);
		LiVM Visualizar(LiVM liVM);
		void Deletar(int id);
		void LerAquivoLI(string path);

	}
}
