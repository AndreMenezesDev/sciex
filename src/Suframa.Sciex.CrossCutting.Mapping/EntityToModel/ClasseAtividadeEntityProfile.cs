using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class ClasseAtividadeEntityProfile : Profile
    {
        public ClasseAtividadeEntityProfile()
        {
            CreateMap<ClasseAtividadeEntity, ClasseAtividadeDto>()
                .ForMember(dest => dest.IdClasseAtividade, opt => opt.MapFrom(src => src.IdClasseAtividade))
                .ForMember(dest => dest.IdGrupoAtividade, opt => opt.MapFrom(src => src.IdGrupoAtividade))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.CodigoGrupo, opt => opt.MapFrom(src => src.GrupoAtividade.Codigo))
                .ForMember(dest => dest.CodigoDivisao, opt => opt.MapFrom(src => src.GrupoAtividade.DivisaoAtividade.Codigo));
        }
    }
}