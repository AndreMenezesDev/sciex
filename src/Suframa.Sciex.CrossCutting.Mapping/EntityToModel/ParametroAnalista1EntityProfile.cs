using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class ParametroAnalista1Profile : Profile
	{		
		public ParametroAnalista1Profile()
		{		
			CreateMap<ParametroAnalista1Entity, ParametroAnalista1VM>()
				.ForMember(dest => dest.IdAnalista, opt => opt.MapFrom(src => src.Analista.IdAnalista))
				.ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Analista.Nome))
				.ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.Analista.CPF.CnpjCpfFormat()))
				.ForMember(dest => dest.Analista, opt => opt.MapFrom(src => Mapper.Map<AnalistaEntity, AnalistaVM>(src.Analista)));
			;
			;
		}
	}
}
