using AutoMapper;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class UnidadeCadastradoraEntityProfile : Profile
    {
        public UnidadeCadastradoraEntityProfile()
        {
            CreateMap<UnidadeCadastradoraEntity, UnidadeCadastradoraEntity>();

            CreateMap<UnidadeCadastradoraEntity, UnidadeCadastradoraVM>();

            CreateMap<UnidadeCadastradoraEntity, UnidadeCadastradoraDto>()
                .ForMember(dest => dest.IdUnidadeCadastradora, opt => opt.MapFrom(src => src.IdUnidadeCadastradora))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao))
                .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao))
                .ForMember(dest => dest.IsUnidadeCadastradoraManaus, opt => opt.MapFrom(src => src.IdMunicipio == new PublicSettings().ID_MUNICIPIO_UNIDADE_CADASTRADORA));

            CreateMap<UnidadeCadastradoraEntity, ManterUnidadeCadastradoraVM>()
                .ForMember(dest => dest.IdUnidadeCadastradora, opt => opt.MapFrom(src => src.IdUnidadeCadastradora))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.UF, opt => opt.MapFrom(src => src.Municipio.SiglaUF))
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio));

            CreateMap<UnidadeCadastradoraEntity, ManterUnidadeCadastradoraGridVM>()
                .ForMember(dest => dest.IdUnidadeCadastradora, opt => opt.MapFrom(src => src.IdUnidadeCadastradora))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio.Descricao));
        }
    }
}