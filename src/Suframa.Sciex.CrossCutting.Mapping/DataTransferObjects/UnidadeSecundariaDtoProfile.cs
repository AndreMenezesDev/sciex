using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class UnidadeSecundariaDtoProfile : Profile
    {
        public UnidadeSecundariaDtoProfile()
        {
            CreateMap<UnidadeSecundariaDto, UnidadeSecundariaEntity>()
                .ForMember(dest => dest.IdUnidadeSecundaria, opt => opt.MapFrom(src => src.IdUnidadeSecundaria))
                .ForMember(dest => dest.IdUnidadeCadastradora, opt => opt.MapFrom(src => src.IdUnidadeCadastradora))
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio));
        }
    }
}