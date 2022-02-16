using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViaTransporteBll
	{
		PagedItems<ViaTransporteVM> ListarPaginado(ViaTransporteVM pagedFilter);
		ViaTransporteVM Selecionar(int? IdViaTransporte);
		void Salvar(ViaTransporteVM viaTransporteVM);
	}
}
