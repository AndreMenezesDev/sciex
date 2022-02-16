using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class QuadroAdministradoresVMProfile : Profile
    {
        public QuadroAdministradoresVMProfile()
        {
            CreateMap<QuadroAdministradoresVM, PessoaJuridicaAdministradorEntity>()
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => Cpf.Unmask(src.Cpf)));
        }
    }
}