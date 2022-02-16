using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class RequerimentoEntityProfile : Profile
	{
		public RequerimentoEntityProfile()
		{
			CreateMap<RequerimentoEntity, DadosSolicitanteVM>();

			CreateMap<RequerimentoEntity, CancelamentoVM>();

			CreateMap<RequerimentoEntity, DadosCadastroVM>();

			CreateMap<RequerimentoEntity, NotificacaoVM>()
				//.ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.Protocolo.Ano))
				.ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.PessoaJuridica.Cnpj.CnpjCpfFormat()))
				.ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.PessoaFisica.Cpf.CnpjCpfFormat()))
				//.ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.Protocolo.DataInclusao))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.IdDadosSolicitante.HasValue ? src.DadosSolicitante.Email : src.IdPessoaFisica.HasValue ? src.PessoaFisica.Email : src.PessoaJuridica.Email))
				.ForMember(dest => dest.IdDadosSolicitante, opt => opt.MapFrom(src => src.IdDadosSolicitante))
				.ForMember(dest => dest.IdPessoaFisica, opt => opt.MapFrom(src => src.IdPessoaFisica))
				.ForMember(dest => dest.IdPessoaJuridica, opt => opt.MapFrom(src => src.IdPessoaJuridica))
				.ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.PessoaFisica.Nome))
				//.ForMember(dest => dest.NumeroSequencial, opt => opt.MapFrom(src => src.Protocolo.NumeroSequencial))
				.ForMember(dest => dest.RazaoSocial, opt => opt.MapFrom(src => src.PessoaJuridica.RazaoSocial))
				.ForMember(dest => dest.Servico, opt => opt.MapFrom(src => src.TipoRequerimento.Servico.Descricao))
				.ForMember(dest => dest.TipoRequerimento, opt => opt.MapFrom(src => (EnumTipoRequerimento)src.IdTipoRequerimento))
				.ForMember(dest => dest.UnidadeCadastradora, opt => opt.MapFrom(src => src.UnidadeCadastradora.Descricao));
		}
	}
}