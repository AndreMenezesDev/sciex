using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class DivisaoAtividadeEntityProfile : Profile
    {
        public DivisaoAtividadeEntityProfile()
        {
            CreateMap<DivisaoAtividadeEntity, DivisaoAtividadeDto>()
                .ForMember(dest => dest.IdDivisaoAtividade, opt => opt.MapFrom(src => src.IdDivisaoAtividade))
                .ForMember(dest => dest.IdSecaoAtividade, opt => opt.MapFrom(src => src.IdSecaoAtividade))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}