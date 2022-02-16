using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class DadosCadastroVMProfile : Profile
    {
        public DadosCadastroVMProfile()
        {
            CreateMap<DadosCadastroVM, EnderecoPessoaJuridicaVM>();

            CreateMap<DadosCadastroVM, RequerimentoEntity>()
                .ForMember(dest => dest.IdPessoaJuridica, opt => opt.MapFrom(src => src.IdentificacaoPessoaJuridica.IdPessoaJuridica))
                .ForMember(dest => dest.IdPessoaFisica, opt => opt.MapFrom(src => src.IdentificacaoPessoaFisica.IdPessoaFisica));

            CreateMap<DadosCadastroVM, ProtocoloEntity>();
        }
    }
}