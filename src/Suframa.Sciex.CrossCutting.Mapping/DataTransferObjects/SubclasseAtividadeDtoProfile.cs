using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class SubclasseAtividadeDtoProfile : Profile
    {
        public SubclasseAtividadeDtoProfile()
        {
            CreateMap<SubClasseAtividadeDto, SubclasseAtividadeEntity>()
                .ForMember(dest => dest.IdSubclasseAtividade, opt => opt.MapFrom(src => src.IdSubClasseAtividade))
                .ForMember(dest => dest.IdClasseAtividade, opt => opt.MapFrom(src => src.IdClasseAtividade))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ClasseAtividade, opt => opt.Ignore());
        }
    }
}