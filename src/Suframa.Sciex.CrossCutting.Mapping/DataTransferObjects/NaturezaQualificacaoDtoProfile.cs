using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class NaturezaQualificacaoDtoProfile : Profile
    {
        public NaturezaQualificacaoDtoProfile()
        {
            CreateMap<NaturezaQualificacaoDto, NaturezaQualificacaoEntity>()
                .ForMember(dest => dest.IdQualificacao, opt => opt.MapFrom(src => src.IdQualificacao))
                .ForMember(dest => dest.IdNaturezaJuridica, opt => opt.MapFrom(src => src.IdNaturezaJuridica))
                .ForMember(dest => dest.IdNaturezaQualificacao, opt => opt.MapFrom(src => src.IdNaturezaQualificacao));
        }
    }
}