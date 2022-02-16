using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class IdentificacaoPessoaFisicaVMProfile : Profile
    {
        public IdentificacaoPessoaFisicaVMProfile()
        {
            CreateMap<IdentificacaoPessoaFisicaVM, PessoaFisicaEntity>()
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => Cpf.Unmask(src.Cpf)))
                .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.Endereco));

            CreateMap<IdentificacaoPessoaFisicaVM, ManterEnderecoVM>();
        }
    }
}