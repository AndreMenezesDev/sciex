using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class UnidadeSecundariaEntityProfile : Profile
    {
        public UnidadeSecundariaEntityProfile()
        {
            CreateMap<UnidadeSecundariaEntity, UnidadeSecundariaDto>()
                .ForMember(dest => dest.IdUnidadeSecundaria, opt => opt.MapFrom(src => src.IdUnidadeSecundaria))
                .ForMember(dest => dest.IdUnidadeCadastradora, opt => opt.MapFrom(src => src.IdUnidadeCadastradora))
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio));

            CreateMap<UnidadeSecundariaEntity, MunicipioVM>()
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Municipio.Descricao))
                .ForMember(dest => dest.UnidadeCadastradora, opt => opt.Ignore());
        }
    }
}