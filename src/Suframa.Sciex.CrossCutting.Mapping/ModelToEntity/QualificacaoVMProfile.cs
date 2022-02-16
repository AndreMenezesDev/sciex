using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class QualificacaoVMProfile : Profile
    {
        public QualificacaoVMProfile()
        {
            CreateMap<QualificacaoVM, QualificacaoDto>();
        }
    }
}