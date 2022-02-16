using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class DiligenciaEntityProfile : Profile
    {
        public DiligenciaEntityProfile()
        {
            CreateMap<DiligenciaEntity, DiligenciaVM>()
                .ForMember(dest => dest.DataDiligencia, opt => opt.MapFrom(src => src.DataDiligenciaUtc))
                .ForMember(dest => dest.Hora, opt => opt.MapFrom(src => src.DataDiligenciaUtc.Value.ToString("HH:mm")))
                .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.IdPessoaFisica.HasValue ? src.PessoaFisica.Cpf : src.PessoaJuridica.Cnpj))
                .ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src => src.IdPessoaFisica.HasValue ? src.PessoaFisica.Nome : src.PessoaJuridica.RazaoSocial));
        }
    }
}