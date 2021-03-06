using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class ParidadeCambialVMProfile : Profile
    {
        public ParidadeCambialVMProfile()
        {
            CreateMap<ParidadeCambialVM, ParidadeCambialEntity>();
        }
    }
}