using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class ManterUnidadeCadastradoraVMProfile : Profile
    {
        public ManterUnidadeCadastradoraVMProfile()
        {
            CreateMap<ManterUnidadeCadastradoraVM, UnidadeCadastradoraDto>();
        }
    }
}