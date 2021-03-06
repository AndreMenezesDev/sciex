using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class CancelamentoVMProfile : Profile
    {
        public CancelamentoVMProfile()
        {
            CreateMap<CancelamentoVM, RequerimentoEntity>();
        }
    }
}