using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class RecursoEntityProfile : Profile
	{
		public RecursoEntityProfile()
		{
			CreateMap<RecursoEntity, RecursoVM>()
				.ForMember(dest => dest.NomeResponsavel, opt => opt.MapFrom(src => string.Format("{0} - {1}", src.UsuarioInterno.Nome, src.Papel.Descricao)));

			CreateMap<RecursoEntity, JulgamentoRecursoVM>()
				.ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.Protocolo.Requerimento.IdPessoaJuridica.HasValue ? src.Protocolo.Requerimento.PessoaJuridica.Cnpj.CnpjCpfFormat() : src.Protocolo.Requerimento.PessoaFisica.Cpf.CnpjCpfFormat()))
				.ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src => src.Protocolo.Requerimento.IdPessoaJuridica.HasValue ? src.Protocolo.Requerimento.PessoaJuridica.RazaoSocial : src.Protocolo.Requerimento.PessoaFisica.Nome))
				.ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.Protocolo.Ano))
				.ForMember(dest => dest.NumeroSequencial, opt => opt.MapFrom(src => src.Protocolo.NumeroSequencial))
				.ForMember(dest => dest.Justificativa, opt => opt.MapFrom(src => src.Justificativa))
				.ForMember(dest => dest.JustificativaIndeferimento, opt => opt.MapFrom(src => src.Protocolo.WorkflowProtocolo.LastOrDefault(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoConferenciaDocumental || x.IdStatusProtocolo == (int)EnumStatusProtocolo.IndeferidoAguardandoRecurso).Justificativa))
				.ForMember(dest => dest.MotivoIndeferimento, opt => opt.MapFrom(src => src.Protocolo.WorkflowProtocolo.LastOrDefault(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoConferenciaDocumental || x.IdStatusProtocolo == (int)EnumStatusProtocolo.IndeferidoAguardandoRecurso).WorkflowMensagemPadrao.Select(x => x.MensagemPadrao.Descricao)))
				.ForMember(dest => dest.TipoProtocolo, opt => opt.MapFrom(src => src.Protocolo.Requerimento.TipoRequerimento.Servico.Descricao));
		}
	}
}