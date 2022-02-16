using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class QuadroSocietarioVMProfile : Profile
    {
        public QuadroSocietarioVMProfile()
        {
            CreateMap<QuadroSocietarioVM, PessoaJuridicaSocioEntity>()
                .ForMember(dest => dest.CnpjCpf, opt => opt.MapFrom(src => src.CnpjCpf.CnpjCpfUnformat()))
                .ForMember(dest => dest.TipoPessoa, opt => opt.MapFrom(src => (decimal)src.TipoPessoa));
        }
    }
}