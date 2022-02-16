using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class ServicoVMProfile : Profile
    {
        public ServicoVMProfile()
        {
            CreateMap<ServicoVM, ServicoEntity>();
        }
    }
}