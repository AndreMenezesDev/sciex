using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IMotivoBll
	{
		IEnumerable<MotivoVM> Listar(MotivoVM motivoVM);

		PagedItems<MotivoVM> ListarPaginado(MotivoVM pagedFilter);

		void Salvar(MotivoVM motivoVM);

		MotivoVM Selecionar(int? idMotivo);

		void Deletar(int id);
		
		IEnumerable<object> ListarChave(MotivoVM motivoVM);
	}
}