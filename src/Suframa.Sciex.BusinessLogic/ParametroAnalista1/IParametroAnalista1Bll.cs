using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IParametroAnalista1Bll
    {

		IEnumerable<ParametroAnalista1VM> Listar(ParametroAnalista1VM parametroAnalista1VM);

		void Atualizar(ParametroAnalista1VM parametroAnalista1);
        
        void Inserir(ParametroAnalista1VM parametroAnalista1);

		ParametroAnalista1VM Selecionar(int? idParametroAnalista);

		PagedItems<ParametroAnalista1VM> ListarPaginado(ParametroAnalista1VM parametroAnalista1);

		void Salvar(ParametroAnalista1VM parametroAnalista1VM);

	}
}