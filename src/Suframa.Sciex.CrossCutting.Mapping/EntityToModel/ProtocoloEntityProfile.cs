using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class ProtocoloEntityProfile : Profile
	{
		public ProtocoloEntityProfile()
		{
			CreateMap<ProtocoloEntity, SacDto>()
				.ForMember(dest => dest.AnoDebito, opt => opt.MapFrom(src => src.TaxaServico.SingleOrDefault().AnoDebito))
				.ForMember(dest => dest.AnoSolicitacao, opt => opt.MapFrom(src => src.Ano))
				.ForMember(dest => dest.DataExpiracao, opt => opt.MapFrom(src => src.TaxaServico.SingleOrDefault().DataExpiracao))
				.ForMember(dest => dest.DataLiquidacao, opt => opt.MapFrom(src => src.TaxaServico.SingleOrDefault().DataPagamento))
				.ForMember(dest => dest.NumeroDebito, opt => opt.MapFrom(src => src.TaxaServico.SingleOrDefault().NumeroDebito))
				.ForMember(dest => dest.NumeroSolicitacao, opt => opt.MapFrom(src => src.NumeroSequencial));

			CreateMap<ProtocoloEntity, ProtocoloVM>()
				.ForMember(dest => dest.DescricaoServico, opt => opt.MapFrom(src => src.Requerimento.TipoRequerimento.Servico.Descricao))
				.ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao))
				.ForMember(dest => dest.DataDesignacao, opt => opt.MapFrom(src => src.WorkflowProtocolo.Where(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoAnalise).OrderBy(x => x.IdWorkflowProtocolo).FirstOrDefault().Data))
				.ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.StatusProtocolo.Descricao))
				.ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.StatusProtocolo.Rank))
				.ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.Requerimento.PessoaFisica != null ? Cpf.Mask(src.Requerimento.PessoaFisica.Cpf) : src.Requerimento.PessoaJuridica.Cnpj.CnpjFormat()))
				.ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src => src.Requerimento.PessoaFisica != null ? src.Requerimento.PessoaFisica.Nome : src.Requerimento.PessoaJuridica.RazaoSocial))
				.ForMember(dest => dest.NumeroProtocolo, opt => opt.MapFrom(src => string.Format("{0}/{1}", src.NumeroSequencial.ToString().PadLeft(6, '0'), src.Ano)))
				.ForMember(dest => dest.InscricaoCadastral, opt => opt.MapFrom(src => src.Requerimento.PessoaJuridica.InscricaoCadastral.FirstOrDefault().Codigo))
				.ForMember(dest => dest.NomeUsuarioInterno, opt => opt.MapFrom(src => src.UsuarioInterno.Nome))
				.ForMember(dest => dest.Workflows, opt => opt.MapFrom(src => src.WorkflowProtocolo))
				.ForMember(dest => dest.DescricaoUsuario, opt => opt.MapFrom(src => src.Requerimento.TipoRequerimento.TipoUsuario.Descricao))
				.ForMember(dest => dest.NomeUnidadeCadastradora, opt => opt.MapFrom(src => src.Requerimento.UnidadeCadastradora.Descricao));

			CreateMap<ProtocoloEntity, ProtocoloHtmlVM>()
				.ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.Ano))
				.ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao.ToLocalTime()))
				.ForMember(dest => dest.NumeroSequencial, opt => opt.MapFrom(src => src.NumeroSequencial))
				.ForMember(dest => dest.DescricaoServico, opt => opt.MapFrom(src => src.Requerimento.TipoRequerimento.Servico.Descricao))
				.ForMember(dest => dest.DescricaoUnidadeCadastradora, opt => opt.MapFrom(src => src.Requerimento.UnidadeCadastradora.Descricao))
				.ForMember(dest => dest.NomeSolicitacao, opt => opt.MapFrom(src => src.Requerimento.PessoaFisica.Nome))
				.ForMember(dest => dest.CpfSolicitacao, opt => opt.MapFrom(src => src.Requerimento.PessoaFisica.Cpf))
				.ForMember(dest => dest.RazaoSocialSolicitacao, opt => opt.MapFrom(src => src.Requerimento.PessoaJuridica.RazaoSocial))
				.ForMember(dest => dest.CnpjSolicitacao, opt => opt.MapFrom(src => src.Requerimento.PessoaJuridica.Cnpj))
				.ForMember(dest => dest.NomeSolicitante, opt => opt.MapFrom(src => src.Requerimento.DadosSolicitante.Nome))
				.ForMember(dest => dest.CpfSolicitante, opt => opt.MapFrom(src => src.Requerimento.DadosSolicitante.Cpf));

			CreateMap<ProtocoloEntity, ConsultaProtocoloResultadoVM>()
				.ForMember(dest => dest.IdStatusProtocolo, opt => opt.MapFrom(src => (EnumStatusProtocolo)src.IdStatusProtocolo))
				.ForMember(dest => dest.DescricaoStatusProtocolo, opt => opt.MapFrom(src => src.StatusProtocolo.Descricao))
				.ForMember(dest => dest.DescricaoOrientacaoStatusProtocolo, opt => opt.MapFrom(src => src.StatusProtocolo.Orientacao))
				.ForMember(dest => dest.IdServico, opt => opt.MapFrom(src => (src.Requerimento != null && src.Requerimento.TipoRequerimento != null && src.Requerimento.TipoRequerimento.Servico != null) ? src.Requerimento.TipoRequerimento.Servico.IdServico : new int?()))
				.ForMember(dest => dest.DescricaoServico, opt => opt.MapFrom(src => (src.Requerimento != null && src.Requerimento.TipoRequerimento != null && src.Requerimento.TipoRequerimento.Servico != null) ? src.Requerimento.TipoRequerimento.Servico.Descricao : null))
				.ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao))
				.ForMember(dest => dest.Workflows, opt => opt.MapFrom(src => src.WorkflowProtocolo));

			CreateMap<ProtocoloEntity, EnviaRecursoVM>()
				.ForMember(dest => dest.DescricaoServico, opt => opt.MapFrom(src => (src.Requerimento != null && src.Requerimento.TipoRequerimento != null && src.Requerimento.TipoRequerimento.Servico != null) ? src.Requerimento.TipoRequerimento.Servico.Descricao : null))
				.ForMember(dest => dest.DescricaoUnidadeCadastradora, opt => opt.MapFrom(src => (src.Requerimento != null && src.Requerimento.UnidadeCadastradora != null) ? src.Requerimento.UnidadeCadastradora.Descricao : null))
				.ForMember(dest => dest.IsConferenciaAdministrativa, opt => opt.MapFrom(src => src.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoConferenciaDocumental))
				.ForMember(dest => dest.IsIndeferidoAguardandoRecurso, opt => opt.MapFrom(src => src.IdStatusProtocolo == (int)EnumStatusProtocolo.IndeferidoAguardandoRecurso))
				.ForMember(dest => dest.JustificativaIndeferimento, opt => opt.MapFrom(src => src.WorkflowProtocolo.LastOrDefault(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoConferenciaDocumental || x.IdStatusProtocolo == (int)EnumStatusProtocolo.IndeferidoAguardandoRecurso).Justificativa))
				.ForMember(dest => dest.Documentos, opt => opt.MapFrom(src => src.WorkflowProtocolo.LastOrDefault(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoConferenciaDocumental || x.IdStatusProtocolo == (int)EnumStatusProtocolo.IndeferidoAguardandoRecurso).ConferenciaDocumento.Select(x => x.TipoDocumento.Descricao)))
				.ForMember(dest => dest.MotivoIndeferimento, opt => opt.MapFrom(src => src.WorkflowProtocolo.LastOrDefault(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoConferenciaDocumental || x.IdStatusProtocolo == (int)EnumStatusProtocolo.IndeferidoAguardandoRecurso).WorkflowMensagemPadrao.Select(x => x.MensagemPadrao.Descricao)));

			CreateMap<ProtocoloEntity, DadosCadastroVM>()
				.ForMember(dest => dest.IdTipoRequerimento, opt => opt.MapFrom(src => src.Requerimento.IdTipoRequerimento))
				.ForMember(dest => dest.IdRequerimento, opt => opt.MapFrom(src => src.Requerimento.IdRequerimento));

			CreateMap<ProtocoloEntity, WorkflowProtocoloEntity>()
				.ForMember(dest => dest.Data, opt => opt.MapFrom(src => DateTime.Now));

			CreateMap<ProtocoloEntity, WorkflowProtocoloDto>()
				.ForMember(dest => dest.DataAtual, opt => opt.MapFrom(src => DateTime.Now))
				.ForMember(dest => dest.QuantidadeDiasAnalise, opt => opt.MapFrom(src => src.Requerimento.TipoRequerimento.Servico.QuantidadeDiasAnalise))
				.ForMember(dest => dest.DataAguardandoReanalise, opt => opt.MapFrom(src => src.WorkflowProtocolo.Where(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoReanalise).OrderBy(x => x.Data).FirstOrDefault().Data))
				.ForMember(dest => dest.DataComPendencia, opt => opt.MapFrom(src => src.WorkflowProtocolo.Where(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.ComPendencia).OrderBy(x => x.Data).FirstOrDefault().Data))
				.ForMember(dest => dest.DataConferenciaAdministrativa, opt => opt.MapFrom(src => src.WorkflowProtocolo.Where(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoConferenciaDocumental).OrderByDescending(x => x.Data).FirstOrDefault().Data))
				.ForMember(dest => dest.DataDesignacao, opt => opt.MapFrom(src => src.WorkflowProtocolo.Where(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.AguardandoAnalise).OrderBy(x => x.IdWorkflowProtocolo).FirstOrDefault().Data))
				.ForMember(dest => dest.DataEmAnalise, opt => opt.MapFrom(src => src.WorkflowProtocolo.Where(x => x.IdStatusProtocolo == (int)EnumStatusProtocolo.EmAnalise).OrderByDescending(x => x.Data).FirstOrDefault().Data));
		}
	}
}