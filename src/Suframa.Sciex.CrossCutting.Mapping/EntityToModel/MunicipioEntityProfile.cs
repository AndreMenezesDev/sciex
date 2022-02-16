using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class MunicipioEntityProfile : Profile
    {
        public MunicipioEntityProfile()
        {
            CreateMap<MunicipioEntity, MunicipioVM>();

            CreateMap<MunicipioEntity, MunicipioDto>()
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.UF, opt => opt.MapFrom(src => src.SiglaUF));

            CreateMap<MunicipioEntity, UnidadeCadastradoraEntity>()
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio));
        }
    }
}