using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class DocumentoComprobatorioVMProfile : Profile
    {
        public DocumentoComprobatorioVMProfile()
        {
            CreateMap<DocumentoComprobatorioVM, RequerimentoDocumentoEntity>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.TipoOrigem, opt => opt.MapFrom(src => (int)src.TipoOrigem));

            CreateMap<DocumentosComprobatoriosVM, RequerimentoDocumentoEntity>();
        }
    }
}