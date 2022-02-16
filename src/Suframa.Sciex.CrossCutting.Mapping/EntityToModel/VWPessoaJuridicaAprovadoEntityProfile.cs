using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.RestService.ApiDto;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class VWPessoaJuridicaAprovadoEntityProfile : Profile
	{
		public VWPessoaJuridicaAprovadoEntityProfile()
		{
			CreateMap<VWPessoaJuridicaAprovadoEntity, PessoaJuridicaApiDto>()
			   .ForMember(dest => dest.empCnpj, opt => opt.MapFrom(src => src.Cnpj))
			   .ForMember(dest => dest.empRazaoSocial, opt => opt.MapFrom(src => src.RazaoSocial))
			   .ForMember(dest => dest.empNomeFantasia, opt => opt.MapFrom(src => src.NomeFantasia))
			   .ForMember(dest => dest.empLogradouro, opt => opt.MapFrom(src => src.Cep.Logradouro))
			   .ForMember(dest => dest.empBairro, opt => opt.MapFrom(src => src.Cep.Bairro))
			   .ForMember(dest => dest.empCep, opt => opt.MapFrom(src => src.Cep.Codigo))
			   .ForMember(dest => dest.ufSigla, opt => opt.MapFrom(src => src.Cep.Municipio.SiglaUF))
			   .ForMember(dest => dest.munNome, opt => opt.MapFrom(src => src.Cep.Municipio.Descricao));

			CreateMap<VWPessoaJuridicaAprovadoEntity, IdentificacaoPessoaJuridicaVM>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Cep.Codigo.ToString().PadLeft(8, '0')))
				.ForMember(dest => dest.IsEntidadeEmpresarial, opt => opt.MapFrom(src => src.NaturezaJuridica.IdNaturezaGrupo == 2))
				.ForMember(dest => dest.FiltroCadastroPessoaJuridica, opt => opt.MapFrom(src => src))
				.ForMember(dest => dest.ModalidadeTransportador, opt => opt.MapFrom(src => (EnumTipoTransportador?)src.Credenciamento.FirstOrDefault(x => x.IdTipoUsuario == (int)EnumTipoUsuario.PessoaJuridicaTransportador).ModalidadeTransportador));
		}
	}
}