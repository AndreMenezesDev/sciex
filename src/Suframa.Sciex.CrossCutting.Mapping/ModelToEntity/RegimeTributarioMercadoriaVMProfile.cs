using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
	public class RegimeTributarioMercadoriaVMProfile : Profile
	{
		public RegimeTributarioMercadoriaVMProfile()
		{
			CreateMap<RegimeTributarioMercadoriaVM, RegimeTributarioMercadoriaEntity>();
				//.ForMember(dest => dest.DataInicioVigencia, opt => opt.MapFrom(src => src.DataInicioVigencia.ConvertendoData()));
		}
	}
}