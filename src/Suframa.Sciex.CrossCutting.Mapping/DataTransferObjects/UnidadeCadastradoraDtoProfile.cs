using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class UnidadeCadastradoraDtoProfile : Profile
    {
        public UnidadeCadastradoraDtoProfile()
        {
            CreateMap<UnidadeCadastradoraDto, UnidadeCadastradoraEntity>()
               .ForMember(dest => dest.IdUnidadeCadastradora, opt => opt.MapFrom(src => src.IdUnidadeCadastradora))
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
               .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio))
               .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao))
               .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao));
        }
    }
}