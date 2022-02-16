using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class IdentificacaoPessoaJuridicaVMProfile : Profile
    {
        public IdentificacaoPessoaJuridicaVMProfile()
        {
            CreateMap<IdentificacaoPessoaJuridicaVM, PessoaJuridicaEntity>()
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.CnpjUnformat()))
                .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.Endereco));

            CreateMap<IdentificacaoPessoaJuridicaVM, EnderecoPessoaJuridicaVM>();
        }
    }
}