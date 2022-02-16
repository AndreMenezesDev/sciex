using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class QualificacaoEntityProfile : Profile
    {
        public QualificacaoEntityProfile()
        {
            CreateMap<QualificacaoEntity, QualificacaoDto>()
                .ForMember(dest => dest.IdQualificacao, opt => opt.MapFrom(src => src.IdQualificacao))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}