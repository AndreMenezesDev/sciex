using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
	public class ParametroAnalista1VMProfile : Profile
	{
		public ParametroAnalista1VMProfile()
		{
			CreateMap<ParametroAnalista1VM, ParametroAnalista1Entity>()
				.ForMember(dest => dest.IdAnalista, opt => opt.MapFrom(src => src.IdAnalista));
			
		}
	}
}
