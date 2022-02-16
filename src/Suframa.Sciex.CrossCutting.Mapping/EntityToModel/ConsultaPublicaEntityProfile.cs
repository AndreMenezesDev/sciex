using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class ConsultaPublicaEntityProfile : Profile
    {
        public ConsultaPublicaEntityProfile()
        {
            CreateMap<ConsultaPublicaEntity, ConsultaPublicaVM>()
                .ForMember(dest => dest.NomeConsulta, opt => opt.MapFrom(src => src.IdTipoConsultaPublica.HasValue ? src.TipoConsultaPublica.TipoConsulta : src.NomeConsulta))
                .ForMember(dest => dest.TipoOrigem, opt => opt.MapFrom(src => src.TipoOrigem))
                .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.IdPessoaJuridicaSocio.HasValue ? src.PessoaJuridicaSocio.CnpjCpf.CnpjCpfFormat() : src.IdPessoaJuridicaAdministrador.HasValue ? src.PessoaJuridicaAdministrador.Cpf.CnpjCpfFormat() : src.IdPessoaJuridica.HasValue ? src.PessoaJuridica.Cnpj.CnpjCpfFormat() : src.PessoaFisica.Cpf.CnpjCpfFormat()))
                .ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src => src.IdPessoaJuridicaSocio.HasValue ? src.PessoaJuridicaSocio.Nome : src.IdPessoaJuridicaAdministrador.HasValue ? src.PessoaJuridicaAdministrador.Nome : src.IdPessoaJuridica.HasValue ? src.PessoaJuridica.RazaoSocial : src.PessoaFisica.Nome));
        }
    }
}