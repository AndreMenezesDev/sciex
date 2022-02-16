using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class ClasseAtividadeDtoProfile : Profile
    {
        public ClasseAtividadeDtoProfile()
        {
            CreateMap<ClasseAtividadeDto, ClasseAtividadeEntity>()
                .ForMember(dest => dest.IdClasseAtividade, opt => opt.MapFrom(src => src.IdClasseAtividade))
                .ForMember(dest => dest.IdGrupoAtividade, opt => opt.MapFrom(src => src.IdGrupoAtividade))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}