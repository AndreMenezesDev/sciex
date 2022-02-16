using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ViaTransporteController: ApiController
	{
		private readonly IViaTransporteBll _viatransporte;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="viatransporte"></param>
		public ViaTransporteController(IViaTransporteBll viatransporte)
		{
			_viatransporte = viatransporte;
		}

		/// <summary>Seleciona a CodigoConta pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ViaTransporteVM Get(int id)
		{
			return _viatransporte.Selecionar(id);
		}

		/// <summary>Salva a CodigoConta</summary>
		/// <param name="viaTransporteVM"></param>
		/// <returns></returns>
		public ViaTransporteVM Put([FromBody] ViaTransporteVM viaTransporteVM)
		{
			_viatransporte.Salvar(viaTransporteVM);
			return viaTransporteVM;
		}
	}
}