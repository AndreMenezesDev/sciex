using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class ManterSetorEmpresarialVMProfile : Profile
    {
        public ManterSetorEmpresarialVMProfile()
        {
            CreateMap<ManterSetorEmpresarialVM, SetorDto>();
        }
    }
}