using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class SecaoAtividadeDtoProfile : Profile
    {
        public SecaoAtividadeDtoProfile()
        {
            CreateMap<SecaoAtividadeDto, SecaoAtividadeEntity>()
               .ForMember(dest => dest.IdSecaoAtividade, opt => opt.MapFrom(src => src.IdSecaoAtividade))
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}