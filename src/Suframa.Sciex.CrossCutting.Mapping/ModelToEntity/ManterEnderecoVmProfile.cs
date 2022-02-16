using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class ManterEnderecoVMProfile : Profile
    {
        public ManterEnderecoVMProfile()
        {
            CreateMap<ManterEnderecoVM, CepDto>();

            CreateMap<ManterEnderecoVM, CepEntity>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo.ExtractNumbers()));

            CreateMap<ManterEnderecoVM, PessoaJuridicaEntity>();

            CreateMap<ManterEnderecoVM, PessoaFisicaEntity>();
        }
    }
}