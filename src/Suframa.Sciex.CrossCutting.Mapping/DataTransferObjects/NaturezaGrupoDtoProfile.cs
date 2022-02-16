using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class NaturezaGrupoDtoProfile : Profile
    {
        public NaturezaGrupoDtoProfile()
        {
            CreateMap<NaturezaGrupoDto, NaturezaGrupoEntity>()
                .ForMember(dest => dest.IdNaturezaGrupo, opt => opt.MapFrom(src => src.IdNaturezaGrupo))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}