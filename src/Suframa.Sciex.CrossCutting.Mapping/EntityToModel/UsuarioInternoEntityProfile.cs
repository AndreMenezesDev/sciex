using AutoMapper;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class UsuarioInternoEntityProfile : Profile
    {
        public UsuarioInternoEntityProfile()
        {
            CreateMap<UsuarioInternoEntity, UsuarioInternoVM>()
                .ForMember(dest => dest.IsUnidadeCadastradoraManaus, opt => opt.MapFrom(src => src.ParametroAnalista.Any(x => x.UnidadeCadastradora.Municipio.IdMunicipio == new PublicSettings().ID_MUNICIPIO_UNIDADE_CADASTRADORA)))
                .ForMember(dest => dest.IsCoordenadorDescentralizada, opt => opt.MapFrom(src => src.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorDescentralizada)))
                .ForMember(dest => dest.IsCoordenadorGeralCOCAD, opt => opt.MapFrom(src => src.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorGeralCocad)))
                .ForMember(dest => dest.IsCoordenadorOutrasAreas, opt => opt.MapFrom(src => src.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.CoordenadorOutrasAreas)))
                .ForMember(dest => dest.IsSuperintendenteAdjunto, opt => opt.MapFrom(src => src.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.SuperintendenteAdjunto)))
                .ForMember(dest => dest.IsTecnico, opt => opt.MapFrom(src => src.UsuarioInternoPapel.Any(x => x.IdPapel == (int)EnumPapel.Tecnico)))
            ;
        }
    }
}