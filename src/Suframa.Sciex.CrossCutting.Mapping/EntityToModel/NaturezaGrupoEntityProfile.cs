using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class NaturezaGrupoEntityProfile : Profile
    {
        public NaturezaGrupoEntityProfile()
        {
            CreateMap<NaturezaGrupoEntity, NaturezaGrupoDto>()
                .ForMember(dest => dest.IdNaturezaGrupo, opt => opt.MapFrom(src => src.IdNaturezaGrupo))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}