using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class RegimeTributarioTesteEntityProfile : Profile
	{
		public RegimeTributarioTesteEntityProfile()
		{
			CreateMap<RegimeTributarioTesteEntity, RegimeTributarioTesteVM>()
				.ForMember(dest => dest.IdRegimeTributario, opt => opt.MapFrom(src => src.IdRegimeTributario))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
		}
	}
}