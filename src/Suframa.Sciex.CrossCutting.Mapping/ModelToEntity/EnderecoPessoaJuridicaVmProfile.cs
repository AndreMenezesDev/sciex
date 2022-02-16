using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class EnderecoPessoaJuridicaVMProfile : Profile
    {
        public EnderecoPessoaJuridicaVMProfile()
        {
            CreateMap<EnderecoPessoaJuridicaVM, PessoaJuridicaEntity>()
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Telefone.ExtractNumbers()) ? (Decimal?)null : Convert.ToDecimal(src.Telefone.ExtractNumbers())))
                .ForMember(dest => dest.Ramal, opt => opt.MapFrom(src => src.Ramal.RamalUnformat()))
                .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.Endereco));

            CreateMap<EnderecoPessoaJuridicaVM, DadosCadastroVM>();
        }
    }
}