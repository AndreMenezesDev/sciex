using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class NaturezaQualificacaoEntityProfile : Profile
	{
		public NaturezaQualificacaoEntityProfile()
		{
			CreateMap<NaturezaQualificacaoEntity, NaturezaQualificacaoDto>()
				.ForMember(dest => dest.IdQualificacao, opt => opt.MapFrom(src => src.IdQualificacao))
				.ForMember(dest => dest.IdNaturezaJuridica, opt => opt.MapFrom(src => src.IdNaturezaJuridica))
				.ForMember(dest => dest.IdNaturezaQualificacao, opt => opt.MapFrom(src => src.IdNaturezaQualificacao));

			CreateMap<NaturezaQualificacaoEntity, NaturezaQualificacaoVM>();

			CreateMap<NaturezaQualificacaoEntity, QualificacaoVM>()
				.ForMember(dest => dest.IdQualificacao, opt => opt.MapFrom(src => src.IdQualificacao))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Qualificacao.Descricao));
		}
	}
}