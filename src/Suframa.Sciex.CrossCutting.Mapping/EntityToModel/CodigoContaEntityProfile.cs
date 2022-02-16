using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class CodigoContaEntityProfile : Profile
	{
		public CodigoContaEntityProfile()
		{
			CreateMap<CodigoContaEntity, CodigoContaVM>()
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao.ToUpper()));
		}
	}
}