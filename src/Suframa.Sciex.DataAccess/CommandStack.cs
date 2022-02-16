using Suframa.Sciex.DataAccess.Database;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;

namespace Suframa.Sciex.DataAccess
{
	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
	/// </summary>
	public class CommandStack : ICommandStack
	{
		private readonly IDatabaseContext context;
		public ICommandRepository<AgendaAtendimentoEntity> AgendaAtendimento { get; }
		public ICommandRepository<ArquivoEntity> Arquivo { get; }
		public ICommandRepository<ArquivoInformacaoEntity> ArquivoInformacao { get; }
		public ICommandRepository<CalendarioAgendamentoEntity> CalendarioAgendamento { get; }
		public ICommandRepository<CalendarioDiaEntity> CalendarioDia { get; }
		public ICommandRepository<CalendarioHoraEntity> CalendarioHora { get; }
		public ICommandRepository<CampoSistemaEntity> CampoSistema { get; }
		public ICommandRepository<CepEntity> Cep { get; }
		public ICommandRepository<ClasseAtividadeEntity> ClasseAtividade { get; }
		public ICommandRepository<ConferenciaDocumentoEntity> ConferenciaDocumento { get; }
		public ICommandRepository<ConsultaPublicaEntity> ConsultaPublica { get; }
		public ICommandRepository<ControleExecucaoJobEntity> ControleExecucaoJob { get; }
		public ICommandRepository<ControleExecucaoServicoEntity> ControleExecucaoServico { get; }
		public ICommandRepository<CredenciamentoEntity> Credenciamento { get; }
		public ICommandRepository<DadosSolicitanteEntity> DadosSolicitante { get; }
		public ICommandRepository<DiligenciaEntity> Diligencia { get; }
		public ICommandRepository<DiligenciaAnexosEntity> DiligenciaAnexos { get; }
		public ICommandRepository<DiligenciaAtividadesEntity> DiligenciaAtividades { get; }
		public ICommandRepository<DiligenciaAtividadesSetorEntity> DiligenciaAtividadesSetor { get; }
		public ICommandRepository<DivisaoAtividadeEntity> DivisaoAtividade { get; }
		public ICommandRepository<FeriadoEntity> Feriado { get; }
		public ICommandRepository<GrupoAtividadeEntity> GrupoAtividade { get; }
		public ICommandRepository<HistoricoSituacaoInscricaoEntity> HistoricoSituacaoInscricao { get; }
		public ICommandRepository<InscricaoCadastralEntity> InscricaoCadastral { get; }
		public ICommandRepository<InscricaoCadastralCredenciamentoEntity> InscricaoCadastralCredenciamento { get; }
		public ICommandRepository<InscricaoSuframaLegadoEntity> InscricaoSuframaLegado { get; }
		public ICommandRepository<JustificativaProtocoloEntity> JustificativaProtocolo { get; }
		public ICommandRepository<ListaDocumentoEntity> ListaDocumento { get; }
		public ICommandRepository<ListaServicoEntity> ListaServico { get; }
		public ICommandRepository<MensagemPadraoEntity> MensagemPadrao { get; }
		public ICommandRepository<MotivoSituacaoInscricaoEntity> MotivoSituacaoInscricao { get; }
		public ICommandRepository<MunicipioEntity> Municipio { get; }
		public ICommandRepository<NaturezaGrupoEntity> NaturezaGrupo { get; }
		public ICommandRepository<NaturezaJuridicaEntity> NaturezaJuridica { get; }
		public ICommandRepository<NaturezaQualificacaoEntity> NaturezaQualificacao { get; }
		public ICommandRepository<PaisEntity> Pais { get; }
		public ICommandRepository<PapelEntity> Papel { get; }
		public ICommandRepository<ParametroAnalistaEntity> ParametroAnalista { get; }
		public ICommandRepository<ParametroAnalistaServicoEntity> ParametroAnalistaServico { get; }
		public ICommandRepository<ParametroDistribuicaoAutomaticaEntity> ParametroDistribuicaoAutomatica { get; }
		public ICommandRepository<PedidoCorrecaoEntity> PedidoCorrecao { get; }
		public ICommandRepository<PessoaFisicaEntity> PessoaFisica { get; }
		public ICommandRepository<PessoaFisicaAprovadoEntity> PessoaFisicaAprovado { get; }
		public ICommandRepository<PessoaJuridicaEntity> PessoaJuridica { get; }
		public ICommandRepository<PessoaJuridicaAdministradorEntity> PessoaJuridicaAdministrativa { get; }
		public ICommandRepository<PessoaJuridicaAprovadoEntity> PessoaJuridicaAprovado { get; }
		public ICommandRepository<PessoaJuridicaAtividadeEntity> PessoaJuridicaAtividade { get; }
		public ICommandRepository<PessoaJuridicaInscricaoEstadualEntity> PessoaJuridicaInscricaoEstadual { get; }
		public ICommandRepository<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; }
		public ICommandRepository<PorteEmpresaEntity> PorteEmpresa { get; }
		public ICommandRepository<ProtocoloEntity> Protocolo { get; }
		public ICommandRepository<QuadroPessoalEntity> QuadroPessoal { get; }
		public ICommandRepository<QualificacaoEntity> Qualificacao { get; }
		public ICommandRepository<RecursoEntity> Recurso { get; }
		public ICommandRepository<RequerimentoEntity> Requerimento { get; }
		public ICommandRepository<RequerimentoDocumentoEntity> RequerimentoDocumento { get; }
		public ICommandRepository<SecaoAtividadeEntity> SecaoAtividade { get; }
		public ICommandRepository<SeqInscricaoCadastralEntity> SeqInscricaoCadastral { get; }
		public ICommandRepository<ServicoEntity> Servico { get; }
		public ICommandRepository<SetorEntity> Setor { get; }
		public ICommandRepository<SetorAtividadeEntity> SetorAtividade { get; }
		public ICommandRepository<SituacaoInscricaoEntity> SituacaoInscricao { get; }
		public ICommandRepository<StatusProtocoloEntity> StatusProtocolo { get; }
		public ICommandRepository<SubclasseAtividadeEntity> SubclasseAtividade { get; }
		public ICommandRepository<TaxaServicoEntity> TaxaServico { get; }
		public ICommandRepository<TipoConsultaPublicaEntity> TipoConsultaPublica { get; }
		public ICommandRepository<TipoDocumentoEntity> TipoDocumento { get; }
		public ICommandRepository<TipoIncentivoEntity> TipoIncentivo { get; }
		public ICommandRepository<TipoRequerimentoEntity> TipoRequerimento { get; }
		public ICommandRepository<TipoUsuarioEntity> TipoUsuario { get; }
		public ICommandRepository<UnidadeCadastradoraEntity> UnidadeCadastradora { get; }
		public ICommandRepository<UnidadeSecundariaEntity> UnidadeSecundaria { get; }
		public ICommandRepository<UsuarioInternoEntity> UsuarioInterno { get; }
		public ICommandRepository<UsuarioPapelEntity> UsuarioInternoPapel { get; }
		public ICommandRepository<VWDocumentoEntity> VWDocumento { get; }
		public ICommandRepository<VWPessoaFisicaAprovadoEntity> VWPessoaFisicaAprovado { get; }
		public ICommandRepository<VWPessoaJuridicaAdministradorAprovadoEntity> VWPessoaJuridicaAdministradorAprovado { get; }
		public ICommandRepository<VWPessoaJuridicaAprovadoEntity> VWPessoaJuridicaAprovado { get; }
		public ICommandRepository<VWPessoaJuridicaAtividadeAprovadoEntity> VWPessoaJuridicaAtividadeAprovado { get; }
		public ICommandRepository<VWPessoaJuridicaInscricaoEstadualAprovadoEntity> VWPessoaJuridicaInscricaoEstadualAprovado { get; }
		public ICommandRepository<VWPessoaJuridicaSocioAprovadoEntity> VWPessoaJuridicaSocioAprovado { get; }
		public ICommandRepository<VWQuadroPessoalAprovadoEntity> VWQuadroPessoalAprovado { get; }
		public ICommandRepository<WorkflowMensagemPadraoEntity> WorkflowMensagemPadrao { get; }
		public ICommandRepository<WorkflowProtocoloEntity> WorkflowProtocolo { get; }
		public ICommandRepository<WorkflowSituacaoInscricaoEntity> WorkflowSituacaoInscricao { get; }
		public ICommandRepository<AladiEntity> Aladi { get; }
		public ICommandRepository<NaladiEntity> Naladi { get; }
		public ICommandRepository<RegimeTributarioEntity> RegimeTributario { get; }
		public ICommandRepository<RegimeTributarioTesteEntity> RegimeTributarioTeste { get; }
		public ICommandRepository<UnidadeReceitaFederalEntity> UnidadeReceitaFederal { get; }
		public ICommandRepository<FundamentoLegalEntity> FundamentoLegal { get; }
		public ICommandRepository<AnalistaEntity> Analista { get; }
		public ICommandRepository<ParametroAnalista1Entity> ParametroAnalista1 { get; }
		public ICommandRepository<PaizEntity> Paiz { get; }		
		public CommandStack(IDatabaseContext databaseContext)
		{
			context = databaseContext;

			AgendaAtendimento = new CommandRepository<AgendaAtendimentoEntity>(context);
			Arquivo = new CommandRepository<ArquivoEntity>(context);
			ArquivoInformacao = new CommandRepository<ArquivoInformacaoEntity>(context);
			CalendarioAgendamento = new CommandRepository<CalendarioAgendamentoEntity>(context);
			CalendarioDia = new CommandRepository<CalendarioDiaEntity>(context);
			CalendarioHora = new CommandRepository<CalendarioHoraEntity>(context);
			CampoSistema = new CommandRepository<CampoSistemaEntity>(context);
			Cep = new CommandRepository<CepEntity>(context);
			ClasseAtividade = new CommandRepository<ClasseAtividadeEntity>(context);
			ConferenciaDocumento = new CommandRepository<ConferenciaDocumentoEntity>(context);
			ConsultaPublica = new CommandRepository<ConsultaPublicaEntity>(context);
			ControleExecucaoJob = new CommandRepository<ControleExecucaoJobEntity>(context);
			ControleExecucaoServico = new CommandRepository<ControleExecucaoServicoEntity>(context);
			Credenciamento = new CommandRepository<CredenciamentoEntity>(context);
			DadosSolicitante = new CommandRepository<DadosSolicitanteEntity>(context);
			Diligencia = new CommandRepository<DiligenciaEntity>(context);
			DiligenciaAnexos = new CommandRepository<DiligenciaAnexosEntity>(context);
			DiligenciaAtividades = new CommandRepository<DiligenciaAtividadesEntity>(context);
			DiligenciaAtividadesSetor = new CommandRepository<DiligenciaAtividadesSetorEntity>(context);
			DivisaoAtividade = new CommandRepository<DivisaoAtividadeEntity>(context);
			Feriado = new CommandRepository<FeriadoEntity>(context);
			GrupoAtividade = new CommandRepository<GrupoAtividadeEntity>(context);
			HistoricoSituacaoInscricao = new CommandRepository<HistoricoSituacaoInscricaoEntity>(context);
			InscricaoCadastral = new CommandRepository<InscricaoCadastralEntity>(context);
			InscricaoCadastralCredenciamento = new CommandRepository<InscricaoCadastralCredenciamentoEntity>(context);
			InscricaoSuframaLegado = new CommandRepository<InscricaoSuframaLegadoEntity>(context);
			JustificativaProtocolo = new CommandRepository<JustificativaProtocoloEntity>(context);
			ListaDocumento = new CommandRepository<ListaDocumentoEntity>(context);
			ListaServico = new CommandRepository<ListaServicoEntity>(context);
			MensagemPadrao = new CommandRepository<MensagemPadraoEntity>(context);
			MotivoSituacaoInscricao = new CommandRepository<MotivoSituacaoInscricaoEntity>(context);
			Municipio = new CommandRepository<MunicipioEntity>(context);
			NaturezaGrupo = new CommandRepository<NaturezaGrupoEntity>(context);
			NaturezaJuridica = new CommandRepository<NaturezaJuridicaEntity>(context);
			NaturezaQualificacao = new CommandRepository<NaturezaQualificacaoEntity>(context);
			Pais = new CommandRepository<PaisEntity>(context);
			Papel = new CommandRepository<PapelEntity>(context);
			ParametroAnalista = new CommandRepository<ParametroAnalistaEntity>(context);
			ParametroDistribuicaoAutomatica = new CommandRepository<ParametroDistribuicaoAutomaticaEntity>(context);
			ParametroAnalistaServico = new CommandRepository<ParametroAnalistaServicoEntity>(context);
			PedidoCorrecao = new CommandRepository<PedidoCorrecaoEntity>(context);
			PessoaFisica = new CommandRepository<PessoaFisicaEntity>(context);
			PessoaFisicaAprovado = new CommandRepository<PessoaFisicaAprovadoEntity>(context);
			PessoaJuridica = new CommandRepository<PessoaJuridicaEntity>(context);
			PessoaJuridicaAdministrativa = new CommandRepository<PessoaJuridicaAdministradorEntity>(context);
			PessoaJuridicaAprovado = new CommandRepository<PessoaJuridicaAprovadoEntity>(context);
			PessoaJuridicaAtividade = new CommandRepository<PessoaJuridicaAtividadeEntity>(context);
			PessoaJuridicaInscricaoEstadual = new CommandRepository<PessoaJuridicaInscricaoEstadualEntity>(context);
			PessoaJuridicaSocio = new CommandRepository<PessoaJuridicaSocioEntity>(context);
			PorteEmpresa = new CommandRepository<PorteEmpresaEntity>(context);
			Protocolo = new CommandRepository<ProtocoloEntity>(context);
			QuadroPessoal = new CommandRepository<QuadroPessoalEntity>(context);
			Qualificacao = new CommandRepository<QualificacaoEntity>(context);
			Recurso = new CommandRepository<RecursoEntity>(context);
			Requerimento = new CommandRepository<RequerimentoEntity>(context);
			RequerimentoDocumento = new CommandRepository<RequerimentoDocumentoEntity>(context);
			SecaoAtividade = new CommandRepository<SecaoAtividadeEntity>(context);
			SeqInscricaoCadastral = new CommandRepository<SeqInscricaoCadastralEntity>(context);
			Servico = new CommandRepository<ServicoEntity>(context);
			Setor = new CommandRepository<SetorEntity>(context);
			SetorAtividade = new CommandRepository<SetorAtividadeEntity>(context);
			SituacaoInscricao = new CommandRepository<SituacaoInscricaoEntity>(context);
			StatusProtocolo = new CommandRepository<StatusProtocoloEntity>(context);
			SubclasseAtividade = new CommandRepository<SubclasseAtividadeEntity>(context);
			TaxaServico = new CommandRepository<TaxaServicoEntity>(context);
			TipoConsultaPublica = new CommandRepository<TipoConsultaPublicaEntity>(context);
			TipoDocumento = new CommandRepository<TipoDocumentoEntity>(context);
			TipoIncentivo = new CommandRepository<TipoIncentivoEntity>(context);
			TipoRequerimento = new CommandRepository<TipoRequerimentoEntity>(context);
			TipoUsuario = new CommandRepository<TipoUsuarioEntity>(context);
			UnidadeCadastradora = new CommandRepository<UnidadeCadastradoraEntity>(context);
			UnidadeSecundaria = new CommandRepository<UnidadeSecundariaEntity>(context);
			UsuarioInterno = new CommandRepository<UsuarioInternoEntity>(context);
			UsuarioInternoPapel = new CommandRepository<UsuarioPapelEntity>(context);
			VWDocumento = new CommandRepository<VWDocumentoEntity>(context);
			VWPessoaFisicaAprovado = new CommandRepository<VWPessoaFisicaAprovadoEntity>(context);
			VWPessoaJuridicaAdministradorAprovado = new CommandRepository<VWPessoaJuridicaAdministradorAprovadoEntity>(context);
			VWPessoaJuridicaAprovado = new CommandRepository<VWPessoaJuridicaAprovadoEntity>(context);
			VWPessoaJuridicaAtividadeAprovado = new CommandRepository<VWPessoaJuridicaAtividadeAprovadoEntity>(context);
			VWPessoaJuridicaInscricaoEstadualAprovado = new CommandRepository<VWPessoaJuridicaInscricaoEstadualAprovadoEntity>(context);
			VWPessoaJuridicaSocioAprovado = new CommandRepository<VWPessoaJuridicaSocioAprovadoEntity>(context);
			VWQuadroPessoalAprovado = new CommandRepository<VWQuadroPessoalAprovadoEntity>(context);
			WorkflowMensagemPadrao = new CommandRepository<WorkflowMensagemPadraoEntity>(context);
			WorkflowProtocolo = new CommandRepository<WorkflowProtocoloEntity>(context);
			WorkflowSituacaoInscricao = new CommandRepository<WorkflowSituacaoInscricaoEntity>(context);
			Paiz = new CommandRepository<PaizEntity>(context);
		}

		public void DetachEntries()
		{
			context.DetachEntries();
		}

		public void Discart()
		{
			context.DiscartChanges();
		}

		public IList<ProtocoloProcedure> ListaProtocolos(ProtocoloProcedure protocolo = null)
		{
			return context.ListarProtocolos(protocolo);
		}

		public void Save()
		{
			context.SaveChanges();
		}

		public int SelecionarNumeroInscricaoUnico(int tipoPessoa, int unidadeCadastradora)
		{
			return context.SelecionarNumeroInscricaoUnico(tipoPessoa, unidadeCadastradora);
		}
	}
}