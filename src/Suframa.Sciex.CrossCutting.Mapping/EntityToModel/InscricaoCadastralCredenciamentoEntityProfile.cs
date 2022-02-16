using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class InscricaoCadastralCredenciamentoEntityProfile : Profile
	{
		public InscricaoCadastralCredenciamentoEntityProfile()
		{
			CreateMap<InscricaoCadastralCredenciamentoEntity, InscricaoCadastralCredenciamentoVM>()
				.ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src =>
					(src.InscricaoCadastral != null) ?
					(src.InscricaoCadastral.PessoaJuridica != null ? src.InscricaoCadastral.PessoaJuridica.Cnpj.CnpjCpfFormat() : src.InscricaoCadastral.PessoaFisica.Cpf.CnpjCpfFormat()) :
					(src.Credenciamento.PessoaJuridica != null ? src.Credenciamento.PessoaJuridica.Cnpj.CnpjCpfFormat() : src.Credenciamento.PessoaFisica.Cpf.CnpjCpfFormat())
				))

				.ForMember(dest => dest.IdPessoaJuridica, opt => opt.MapFrom(src =>
					(src.InscricaoCadastral != null) ?
					src.InscricaoCadastral.PessoaJuridica.IdPessoaJuridica :
					src.Credenciamento.PessoaJuridica.IdPessoaJuridica
				))

				.ForMember(dest => dest.IdPessoaFisica, opt => opt.MapFrom(src =>
					(src.InscricaoCadastral != null) ?
					src.InscricaoCadastral.PessoaFisica.IdPessoaFisica :
					src.Credenciamento.PessoaFisica.IdPessoaFisica
				))

				.ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src =>
					(src.InscricaoCadastral != null) ?
					(src.InscricaoCadastral.PessoaJuridica != null ? src.InscricaoCadastral.PessoaJuridica.RazaoSocial : src.InscricaoCadastral.PessoaFisica.Nome) :
					(src.Credenciamento.PessoaJuridica != null ? src.Credenciamento.PessoaJuridica.RazaoSocial : src.Credenciamento.PessoaFisica.Nome)
				))

				.ForMember(dest => dest.InscricaoCadastral, opt => opt.MapFrom(src => src.InscricaoCadastral.Codigo))

				.ForMember(dest => dest.DataAbertura, opt => opt.MapFrom(src => src.Credenciamento != null ? src.Credenciamento.DataCredenciamento : src.InscricaoCadastral.Data))

				.ForMember(dest => dest.DataValidade, opt => opt.MapFrom(src => src.Credenciamento.DataValidade))

				.ForMember(dest => dest.DescricaoUnidadeCadastradora, opt => opt.MapFrom(src => src.InscricaoCadastral != null ? src.InscricaoCadastral.UnidadeCadastradora.Descricao : src.Credenciamento.UnidadeCadastradora.Descricao))

				.ForMember(dest => dest.DescricaoTipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario.Descricao))

				.ForMember(dest => dest.DescricaoStatus, opt => opt.MapFrom(src => src.InscricaoCadastral != null ? src.InscricaoCadastral.SituacaoInscricao.Descricao : (src.Credenciamento.IsAtivo ? "Ativo" : "Inativo")))

				.ForMember(dest => dest.HistoricoSituacaoInscricao, opt => opt.MapFrom(src => src.InscricaoCadastral.WorkflowSituacaoInscricao.SelectMany(x => x.HistoricoSituacaoInscricao)));
		}
	}
}