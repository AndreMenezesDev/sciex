using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class GrupoAtividadeEntityProfile : Profile
    {
        public GrupoAtividadeEntityProfile()
        {
            CreateMap<GrupoAtividadeEntity, GrupoAtividadeDto>()
                .ForMember(dest => dest.IdGrupoAtividade, opt => opt.MapFrom(src => src.IdGrupoAtividade))
                .ForMember(dest => dest.IdDivisaoAtividade, opt => opt.MapFrom(src => src.IdDivisaoAtividade))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.CodigoDivisaoAtividade, opt => opt.MapFrom(src => src.DivisaoAtividade.Codigo));
        }
    }
}