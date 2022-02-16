using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class WorkflowProtocoloEntityProfile : Profile
	{
		public WorkflowProtocoloEntityProfile()
		{
			CreateMap<WorkflowProtocoloEntity, WorkflowProtocoloVM>()
				.ForMember(dest => dest.NomeUsuarioInterno, opt => opt.MapFrom(src => src.UsuarioInterno.Nome))
				.ForMember(dest => dest.IdStatusProtocolo, opt => opt.MapFrom(src => (EnumStatusProtocolo)src.IdStatusProtocolo))
				.ForMember(dest => dest.DescricaoStatusProtocolo, opt => opt.MapFrom(src => src.StatusProtocolo == null ? null : src.StatusProtocolo.Descricao))
				.ForMember(dest => dest.TiposDocumentos, opt => opt.MapFrom(src => src.ConferenciaDocumento.Select(x => x.TipoDocumento)));

			CreateMap<WorkflowProtocoloEntity, NotificacaoVM>()
				.ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.Protocolo.Ano))
				.ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Protocolo.Requerimento.PessoaJuridica.Cnpj.CnpjCpfFormat()))
				.ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Protocolo.Requerimento.PessoaFisica.Cpf.CnpjCpfFormat()))
				.ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.Protocolo.DataInclusao))
				.ForMember(dest => dest.Documentos, opt => opt.MapFrom(src => src.ConferenciaDocumento.Select(x => x.TipoDocumento.Descricao)))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Protocolo.Requerimento.IdDadosSolicitante.HasValue ? src.Protocolo.Requerimento.DadosSolicitante.Email : src.Protocolo.Requerimento.IdPessoaFisica.HasValue ? src.Protocolo.Requerimento.PessoaFisica.Email : src.Protocolo.Requerimento.PessoaJuridica.Email))
				.ForMember(dest => dest.IdDadosSolicitante, opt => opt.MapFrom(src => src.Protocolo.Requerimento.IdDadosSolicitante))
				.ForMember(dest => dest.IdPessoaFisica, opt => opt.MapFrom(src => src.Protocolo.Requerimento.IdPessoaFisica))
				.ForMember(dest => dest.IdPessoaJuridica, opt => opt.MapFrom(src => src.Protocolo.Requerimento.IdPessoaJuridica))
				.ForMember(dest => dest.Justificativa, opt => opt.MapFrom(src => src.Justificativa))
				.ForMember(dest => dest.MensagemPadrao, opt => opt.MapFrom(src => src.WorkflowMensagemPadrao.Select(x => x.MensagemPadrao).Select(y => y.Descricao)))
				.ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Protocolo.Requerimento.PessoaFisica.Nome))
				.ForMember(dest => dest.NumeroSequencial, opt => opt.MapFrom(src => src.Protocolo.NumeroSequencial))
				.ForMember(dest => dest.Orientacao, opt => opt.MapFrom(src => src.StatusProtocolo.Orientacao))
				.ForMember(dest => dest.RazaoSocial, opt => opt.MapFrom(src => src.Protocolo.Requerimento.PessoaJuridica.RazaoSocial))
				.ForMember(dest => dest.Servico, opt => opt.MapFrom(src => src.Protocolo.Requerimento.TipoRequerimento.Servico.Descricao))
				.ForMember(dest => dest.StatusProtocolo, opt => opt.MapFrom(src => (EnumStatusProtocolo)src.Protocolo.StatusProtocolo.IdStatusProtocolo))
				.ForMember(dest => dest.TipoRequerimento, opt => opt.MapFrom(src => (EnumTipoRequerimento)src.Protocolo.Requerimento.TipoRequerimento.IdTipoRequerimento))
				.ForMember(dest => dest.UnidadeCadastradora, opt => opt.MapFrom(src => src.Protocolo.Requerimento.UnidadeCadastradora.Descricao));
		}
	}
}