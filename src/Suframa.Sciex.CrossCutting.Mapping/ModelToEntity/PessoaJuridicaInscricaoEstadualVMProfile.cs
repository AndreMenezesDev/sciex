using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class PessoaJuridicaInscricaoEstadualVMProfile : Profile
    {
        public PessoaJuridicaInscricaoEstadualVMProfile()
        {
            CreateMap<PessoaJuridicaInscricaoEstadualVM, PessoaJuridicaInscricaoEstadualEntity>();
        }
    }
}