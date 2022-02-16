using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class GrupoAtividadeDtoProfile : Profile
    {
        public GrupoAtividadeDtoProfile()
        {
            CreateMap<GrupoAtividadeDto, GrupoAtividadeEntity>()
                .ForMember(dest => dest.IdGrupoAtividade, opt => opt.MapFrom(src => src.IdGrupoAtividade))
                .ForMember(dest => dest.IdDivisaoAtividade, opt => opt.MapFrom(src => src.IdDivisaoAtividade))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}