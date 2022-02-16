using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class QualificaçãoDtoProfile : Profile
    {
        public QualificaçãoDtoProfile()
        {
            CreateMap<QualificacaoDto, QualificacaoVM>();

            CreateMap<QualificacaoDto, QualificacaoEntity>()
              .ForMember(dest => dest.IdQualificacao, opt => opt.MapFrom(src => src.IdQualificacao))
              .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
              .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}