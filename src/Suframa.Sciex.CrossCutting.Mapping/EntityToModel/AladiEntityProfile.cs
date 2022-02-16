using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class AladiEntityProfile : Profile
	{
		public AladiEntityProfile()
		{
			CreateMap<AladiEntity, AladiVM>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
			;
		}
	}
}
