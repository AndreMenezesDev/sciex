using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class JulgamentoRecursoVMProfile : Profile
    {
        public JulgamentoRecursoVMProfile()
        {
            CreateMap<JulgamentoRecursoVM, WorkflowProtocoloEntity>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Justificativa, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ParecerSuperintendente) ? src.ParecerSuperintendente : src.ParecerCoordenador));

            CreateMap<JulgamentoRecursoVM, ProtocoloVM>()
                .ForMember(dest => dest.Justificativa, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ParecerSuperintendente) ? src.ParecerSuperintendente : src.ParecerCoordenador));
        }
    }
}