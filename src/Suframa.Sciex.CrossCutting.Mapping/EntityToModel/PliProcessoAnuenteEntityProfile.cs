using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PliProcessoAnuenteEntityProfile : Profile
	{
		public PliProcessoAnuenteEntityProfile()
		{
			CreateMap<PliProcessoAnuenteEntity, PliProcessoAnuenteVM>()
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.OrgaoAnuente.Descricao.ToUpper()))
				.ForMember(dest => dest.Sigla, opt => opt.MapFrom(src => src.OrgaoAnuente.OrgaoSigla.ToUpper()))
				;
		}
	}
}
