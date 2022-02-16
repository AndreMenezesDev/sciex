using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class ViaTransporteEntityProfile : Profile
	{
		public ViaTransporteEntityProfile()
		{
			CreateMap<ViaTransporteEntity, ViaTransporteVM>()
				.ForMember(dest => dest.IdViaTransporte, opt => opt.MapFrom(src => src.IdViaTransporte))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
		}
	}
}
