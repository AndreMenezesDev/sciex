using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.BusinessLogic
{
	public class ViaTransporteBll: IViaTransporteBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public ViaTransporteBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public PagedItems<ViaTransporteVM> ListarPaginado(ViaTransporteVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ViaTransporteVM>();}

			var viatransporte = _uowSciex.QueryStackSciex.ViaTransporte.ListarPaginado<ViaTransporteVM>(o =>
			(
				(
					pagedFilter.Codigo == 0 || o.Codigo == pagedFilter.Codigo
				) &&
				(
					string.IsNullOrEmpty(pagedFilter.Descricao) ||
					o.Descricao.Contains(pagedFilter.Descricao)
				) &&
				(
					pagedFilter.Status == 2 || o.Status == pagedFilter.Status
				)
			),
			pagedFilter);

			return viatransporte;
		}


		public ViaTransporteVM Selecionar(int? IdViaTransporte)
		{
			var viatransporteVM = new ViaTransporteVM();

			if (!IdViaTransporte.HasValue) { return viatransporteVM; }

			var viaTransporteSelecionado = _uowSciex.QueryStackSciex.ViaTransporte.Selecionar(x => x.IdViaTransporte == IdViaTransporte);

			viatransporteVM = AutoMapper.Mapper.Map<ViaTransporteVM>(viaTransporteSelecionado);

			return viatransporteVM;
		}

		public void Salvar(ViaTransporteVM viaTransporteVM)
		{
			RegrasSalvar(viaTransporteVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public ViaTransporteVM RegrasSalvar(ViaTransporteVM viaTransporteVM)
		{
			if (viaTransporteVM == null) { return null; }

			var viaTransporteEntity = AutoMapper.Mapper.Map<ViaTransporteEntity>(viaTransporteVM);

			if (viaTransporteEntity == null) { return null; }

			if (viaTransporteVM.IdViaTransporte.HasValue)
			{
				viaTransporteEntity = _uowSciex.QueryStackSciex.ViaTransporte.Selecionar(x => x.IdViaTransporte == viaTransporteVM.IdViaTransporte);

				viaTransporteEntity = AutoMapper.Mapper.Map(viaTransporteVM, viaTransporteEntity);
			}

			_uowSciex.CommandStackSciex.ViaTransporte.Salvar(viaTransporteEntity);

			return viaTransporteVM;
		}


	}
}
