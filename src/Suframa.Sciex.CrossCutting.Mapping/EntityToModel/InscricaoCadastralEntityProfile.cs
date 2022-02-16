using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class InscricaoCadastralEntityProfile : Profile
	{
		public InscricaoCadastralEntityProfile()
		{
			CreateMap<InscricaoCadastralEntity, InscricaoCadastralEnderecoVM>()
				.ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => (src.PessoaFisica != null && src.PessoaFisica.Cep != null) ? src.PessoaFisica.Cep.Bairro : (src.PessoaJuridica != null && src.PessoaJuridica.Cep != null ? src.PessoaJuridica.Cep.Bairro : null)))
				.ForMember(dest => dest.Cep, opt => opt.MapFrom(src => (src.PessoaFisica != null && src.PessoaFisica.Cep != null) ? src.PessoaFisica.Cep.Codigo.ToString().CepFormat() : (src.PessoaJuridica != null && src.PessoaJuridica.Cep != null ? src.PessoaJuridica.Cep.Codigo.ToString().CepFormat() : null)))
				.ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => (src.PessoaFisica != null && src.PessoaFisica.Cep != null) ? src.PessoaFisica.Cep.Logradouro : (src.PessoaJuridica != null && src.PessoaJuridica.Cep != null ? src.PessoaJuridica.Cep.Logradouro : null)))
				.ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => (src.PessoaFisica != null && src.PessoaFisica.Cep != null) ? src.PessoaFisica.Cep.Endereco : (src.PessoaJuridica != null && src.PessoaJuridica.Cep != null ? src.PessoaJuridica.Cep.Endereco : null)))
				.ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => (src.PessoaFisica != null && src.PessoaFisica.Cep != null && src.PessoaFisica.Cep.Municipio != null) ? src.PessoaFisica.Cep.Municipio.Descricao : (src.PessoaJuridica != null && src.PessoaJuridica.Cep != null && src.PessoaJuridica.Cep.Municipio != null ? src.PessoaJuridica.Cep.Municipio.Descricao : null)))
				.ForMember(dest => dest.Uf, opt => opt.MapFrom(src => (src.PessoaFisica != null && src.PessoaFisica.Cep != null && src.PessoaFisica.Cep.Municipio != null) ? src.PessoaFisica.Cep.Municipio.SiglaUF : (src.PessoaJuridica != null && src.PessoaJuridica.Cep != null && src.PessoaJuridica.Cep.Municipio != null ? src.PessoaJuridica.Cep.Municipio.SiglaUF : null)))
				.ForMember(dest => dest.Complemento, opt => opt.MapFrom(src => (src.PessoaFisica != null ? src.PessoaFisica.Complemento : src.PessoaJuridica.Complemento)))
				.ForMember(dest => dest.Numero, opt => opt.MapFrom(src => (src.PessoaFisica != null ? src.PessoaFisica.NumeroEndereco : src.PessoaJuridica.NumeroEndereco)));

			CreateMap<InscricaoCadastralEntity, InscricaoCadastralVM>()
				.ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.Ano.ToString()))
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo.ToString().PadLeft(6, '0')))
				.ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.PessoaFisica != null ? Cpf.Mask(src.PessoaFisica.Cpf) : src.PessoaJuridica.Cnpj.CnpjFormat()))
				.ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data.HasValue ? src.Data.Value.ToString("dd/MM/yyyy") : null))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.PessoaFisica != null ? src.PessoaFisica.Email : src.PessoaJuridica.Email))
				.ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.PessoaFisica != null ? src.PessoaFisica.Telefone.ToString() : src.PessoaJuridica.Telefone.ToString()))
				.ForMember(dest => dest.NaturezaJuridicaCodigo, opt => opt.MapFrom(src => (src.PessoaJuridica != null && src.PessoaJuridica.NaturezaJuridica != null) ? src.PessoaJuridica.NaturezaJuridica.Codigo.ToString() : null))
				.ForMember(dest => dest.NaturezaJuridicaDescricao, opt => opt.MapFrom(src => (src.PessoaJuridica != null && src.PessoaJuridica.NaturezaJuridica != null) ? src.PessoaJuridica.NaturezaJuridica.Descricao : null))
				.ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src => src.PessoaFisica != null ? src.PessoaFisica.Nome : src.PessoaJuridica.RazaoSocial))
				.ForMember(dest => dest.SituacaoInscricaoDescricaoMotivo, opt => opt.MapFrom(src => src.SituacaoInscricao != null ? src.SituacaoInscricao.Descricao : null))
				.ForMember(dest => dest.SituacaoCadastralDescricao, opt => opt.MapFrom(src => (src.SituacaoInscricao != null) ? src.SituacaoInscricao.Descricao : null))
				.ForMember(dest => dest.TipoEstabelecimento, opt => opt.MapFrom(src => src.PessoaJuridica != null ? ((EnumTipoEstabelecimento)src.PessoaJuridica.TipoEstabelecimento).ToString() : null))
				.ForMember(dest => dest.AtividadePrincipal, opt => opt.MapFrom(src => (src.PessoaJuridica != null && src.PessoaJuridica.PessoaJuridicaAtividade != null) ? src.PessoaJuridica.PessoaJuridicaAtividade.FirstOrDefault(x => (EnumTipoAtividade)x.Tipo == EnumTipoAtividade.Principal) : null))
				.ForMember(dest => dest.AtividadesSecundarias, opt => opt.MapFrom(src => (src.PessoaJuridica != null && src.PessoaJuridica.PessoaJuridicaAtividade != null) ? src.PessoaJuridica.PessoaJuridicaAtividade.Where(x => (EnumTipoAtividade)x.Tipo == EnumTipoAtividade.Secundaria) : null))
				.ForMember(dest => dest.TipoIncentivo, opt => opt.MapFrom(src => src.TipoIncentivo ?? null))
				.ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src));
		}
	}
}