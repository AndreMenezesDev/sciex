using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class FiltroCadastroPessoaJuridicaVMProfile : Profile
    {
        public FiltroCadastroPessoaJuridicaVMProfile()
        {
            CreateMap<FiltroCadastroPessoaJuridicaVM, PessoaJuridicaEntity>()
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.CnpjUnformat())); ;
        }
    }
}