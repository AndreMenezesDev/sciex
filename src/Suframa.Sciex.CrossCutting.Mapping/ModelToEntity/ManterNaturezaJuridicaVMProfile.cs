using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class ManterNaturezaJuridicaVMProfile : Profile
    {
        public ManterNaturezaJuridicaVMProfile()
        {
            CreateMap<ManterNaturezaJuridicaVM, NaturezaJuridicaDto>();

            CreateMap<ManterNaturezaJuridicaVM, NaturezaJuridicaEntity>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.IdNaturezaGrupo, opt => opt.MapFrom(src => src.IdNaturezaGrupo))
                .ForMember(dest => dest.IdNaturezaJuridica, opt => opt.MapFrom(src => src.IdNaturezaJuridica))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusQuadroSocial, opt => opt.MapFrom(src => src.StatusQuadroSocial));
        }
    }
}