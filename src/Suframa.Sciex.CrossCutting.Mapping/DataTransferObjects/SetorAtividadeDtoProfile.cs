using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class SetorAtividadeDtoProfile : Profile
    {
        public SetorAtividadeDtoProfile()
        {
            CreateMap<SetorAtividadeDto, SetorAtividadeEntity>()
                .ForMember(dest => dest.IdSetorAtividade, opt => opt.MapFrom(src => src.IdSetorAtividade))
                .ForMember(dest => dest.IdSetor, opt => opt.MapFrom(src => src.IdSetor))
                .ForMember(dest => dest.IdSubclasseAtividade, opt => opt.MapFrom(src => src.IdSubClasseAtividade));
        }
    }
}