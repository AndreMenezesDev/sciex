using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class MunicipioDtoProfile : Profile
    {
        public MunicipioDtoProfile()
        {
            CreateMap<MunicipioDto, MunicipioEntity>()
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.SiglaUF, opt => opt.MapFrom(src => src.UF));
        }
    }
}