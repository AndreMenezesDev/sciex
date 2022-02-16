using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class MunicipioVMProfile : Profile
    {
        public MunicipioVMProfile()
        {
            CreateMap<MunicipioVM, MunicipioDto>();

            CreateMap<MunicipioVM, UnidadeCadastradoraVM>();

            CreateMap<MunicipioVM, MunicipioEntity>();
        }
    }
}