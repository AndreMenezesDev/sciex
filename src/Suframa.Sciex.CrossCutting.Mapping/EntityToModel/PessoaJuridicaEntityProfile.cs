using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PessoaJuridicaEntityProfile : Profile
	{
		public PessoaJuridicaEntityProfile()
		{
			CreateMap<PessoaJuridicaEntity, PessoaJuridicaEntity>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			CreateMap<PessoaJuridicaEntity, FiltroCadastroPessoaJuridicaVM>()
				.ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.CnpjUnformat()));

			CreateMap<PessoaJuridicaEntity, IdentificacaoPessoaJuridicaVM>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Cep.Codigo.ToString().PadLeft(8, '0')))
				.ForMember(dest => dest.IsEntidadeEmpresarial, opt => opt.MapFrom(src => src.NaturezaJuridica.IdNaturezaGrupo == 2))
				.ForMember(dest => dest.FiltroCadastroPessoaJuridica, opt => opt.MapFrom(src => src))
				.ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Cep))
				.ForMember(dest => dest.ModalidadeTransportador, opt => opt.MapFrom(src => (EnumTipoTransportador?)src.Credenciamento.FirstOrDefault(x => x.IdTipoUsuario == (int)EnumTipoUsuario.PessoaJuridicaTransportador).ModalidadeTransportador));

			CreateMap<PessoaJuridicaEntity, EnderecoPessoaJuridicaVM>()
				.ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Cep))
				.ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Telefone));

			CreateMap<PessoaJuridicaEntity, ConsultaPublicaResponsaveisVM>()
				.ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.Cnpj.CnpjFormat()))
				.ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src => src.RazaoSocial));
		}
	}
}