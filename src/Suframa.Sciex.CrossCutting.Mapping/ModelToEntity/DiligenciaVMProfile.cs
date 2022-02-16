using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class DiligenciaVMProfile : Profile
    {
        public DiligenciaVMProfile()
        {
            CreateMap<DiligenciaVM, DiligenciaEntity>()
                .ForMember(dest => dest.DataDiligenciaUtc, opt => opt.MapFrom(src => src.DataDiligencia))
                .ForMember(dest => dest.DiligeciaAtividade, opt => opt.MapFrom(src => src.Atividades));
        }
    }
}