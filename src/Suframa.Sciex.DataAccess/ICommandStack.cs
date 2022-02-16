using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;

namespace Suframa.Sciex.DataAccess
{
	public interface ICommandStack
	{
		ICommandRepository<AgendaAtendimentoEntity> AgendaAtendimento { get; }
		ICommandRepository<ArquivoEntity> Arquivo { get; }
		ICommandRepository<ArquivoInformacaoEntity> ArquivoInformacao { get; }
		ICommandRepository<CalendarioAgendamentoEntity> CalendarioAgendamento { get; }
		ICommandRepository<CalendarioDiaEntity> CalendarioDia { get; }
		ICommandRepository<CalendarioHoraEntity> CalendarioHora { get; }
		ICommandRepository<CampoSistemaEntity> CampoSistema { get; }
		ICommandRepository<CepEntity> Cep { get; }
		ICommandRepository<ClasseAtividadeEntity> ClasseAtividade { get; }
		ICommandRepository<ConferenciaDocumentoEntity> ConferenciaDocumento { get; }
		ICommandRepository<ConsultaPublicaEntity> ConsultaPublica { get; }
		ICommandRepository<ControleExecucaoJobEntity> ControleExecucaoJob { get; }
		ICommandRepository<ControleExecucaoServicoEntity> ControleExecucaoServico { get; }
		ICommandRepository<CredenciamentoEntity> Credenciamento { get; }
		ICommandRepository<DadosSolicitanteEntity> DadosSolicitante { get; }
		ICommandRepository<DiligenciaEntity> Diligencia { get; }
		ICommandRepository<DiligenciaAnexosEntity> DiligenciaAnexos { get; }
		ICommandRepository<DiligenciaAtividadesEntity> DiligenciaAtividades { get; }
		ICommandRepository<DiligenciaAtividadesSetorEntity> DiligenciaAtividadesSetor { get; }
		ICommandRepository<DivisaoAtividadeEntity> DivisaoAtividade { get; }
		ICommandRepository<FeriadoEntity> Feriado { get; }
		ICommandRepository<GrupoAtividadeEntity> GrupoAtividade { get; }
		ICommandRepository<HistoricoSituacaoInscricaoEntity> HistoricoSituacaoInscricao { get; }
		ICommandRepository<InscricaoCadastralEntity> InscricaoCadastral { get; }
		ICommandRepository<InscricaoCadastralCredenciamentoEntity> InscricaoCadastralCredenciamento { get; }
		ICommandRepository<InscricaoSuframaLegadoEntity> InscricaoSuframaLegado { get; }
		ICommandRepository<ListaDocumentoEntity> ListaDocumento { get; }
		ICommandRepository<ListaServicoEntity> ListaServico { get; }
		ICommandRepository<MensagemPadraoEntity> MensagemPadrao { get; }
		ICommandRepository<MotivoSituacaoInscricaoEntity> MotivoSituacaoInscricao { get; }
		ICommandRepository<MunicipioEntity> Municipio { get; }
		ICommandRepository<NaturezaGrupoEntity> NaturezaGrupo { get; }
		ICommandRepository<NaturezaJuridicaEntity> NaturezaJuridica { get; }
		ICommandRepository<NaturezaQualificacaoEntity> NaturezaQualificacao { get; }
		ICommandRepository<PaisEntity> Pais { get; }
		ICommandRepository<PapelEntity> Papel { get; }
		ICommandRepository<ParametroAnalistaEntity> ParametroAnalista { get; }
		ICommandRepository<ParametroAnalistaServicoEntity> ParametroAnalistaServico { get; }
		ICommandRepository<ParametroDistribuicaoAutomaticaEntity> ParametroDistribuicaoAutomatica { get; }
		ICommandRepository<PedidoCorrecaoEntity> PedidoCorrecao { get; }
		ICommandRepository<PessoaFisicaEntity> PessoaFisica { get; }
		ICommandRepository<PessoaFisicaAprovadoEntity> PessoaFisicaAprovado { get; }
		ICommandRepository<PessoaJuridicaEntity> PessoaJuridica { get; }
		ICommandRepository<PessoaJuridicaAdministradorEntity> PessoaJuridicaAdministrativa { get; }
		ICommandRepository<PessoaJuridicaAprovadoEntity> PessoaJuridicaAprovado { get; }
		ICommandRepository<PessoaJuridicaAtividadeEntity> PessoaJuridicaAtividade { get; }
		ICommandRepository<PessoaJuridicaInscricaoEstadualEntity> PessoaJuridicaInscricaoEstadual { get; }
		ICommandRepository<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; }
		ICommandRepository<PorteEmpresaEntity> PorteEmpresa { get; }
		ICommandRepository<ProtocoloEntity> Protocolo { get; }
		ICommandRepository<QuadroPessoalEntity> QuadroPessoal { get; }
		ICommandRepository<QualificacaoEntity> Qualificacao { get; }
		ICommandRepository<RecursoEntity> Recurso { get; }		
		ICommandRepository<RequerimentoEntity> Requerimento { get; }
		ICommandRepository<RequerimentoDocumentoEntity> RequerimentoDocumento { get; }
		ICommandRepository<SecaoAtividadeEntity> SecaoAtividade { get; }
		ICommandRepository<SeqInscricaoCadastralEntity> SeqInscricaoCadastral { get; }
		ICommandRepository<ServicoEntity> Servico { get; }
		ICommandRepository<SetorEntity> Setor { get; }
		ICommandRepository<SetorAtividadeEntity> SetorAtividade { get; }
		ICommandRepository<SituacaoInscricaoEntity> SituacaoInscricao { get; }
		ICommandRepository<StatusProtocoloEntity> StatusProtocolo { get; }
		ICommandRepository<SubclasseAtividadeEntity> SubclasseAtividade { get; }
		ICommandRepository<TaxaServicoEntity> TaxaServico { get; }
		ICommandRepository<TipoConsultaPublicaEntity> TipoConsultaPublica { get; }
		ICommandRepository<TipoDocumentoEntity> TipoDocumento { get; }
		ICommandRepository<TipoIncentivoEntity> TipoIncentivo { get; }
		ICommandRepository<TipoRequerimentoEntity> TipoRequerimento { get; }
		ICommandRepository<TipoUsuarioEntity> TipoUsuario { get; }
		ICommandRepository<UnidadeCadastradoraEntity> UnidadeCadastradora { get; }
		ICommandRepository<UnidadeSecundariaEntity> UnidadeSecundaria { get; }
		ICommandRepository<UsuarioInternoEntity> UsuarioInterno { get; }
		ICommandRepository<UsuarioPapelEntity> UsuarioInternoPapel { get; }
		ICommandRepository<VWDocumentoEntity> VWDocumento { get; }
		ICommandRepository<VWPessoaFisicaAprovadoEntity> VWPessoaFisicaAprovado { get; }
		ICommandRepository<VWPessoaJuridicaAdministradorAprovadoEntity> VWPessoaJuridicaAdministradorAprovado { get; }
		ICommandRepository<VWPessoaJuridicaAprovadoEntity> VWPessoaJuridicaAprovado { get; }
		ICommandRepository<VWPessoaJuridicaAtividadeAprovadoEntity> VWPessoaJuridicaAtividadeAprovado { get; }
		ICommandRepository<VWPessoaJuridicaInscricaoEstadualAprovadoEntity> VWPessoaJuridicaInscricaoEstadualAprovado { get; }
		ICommandRepository<VWPessoaJuridicaSocioAprovadoEntity> VWPessoaJuridicaSocioAprovado { get; }
		ICommandRepository<VWQuadroPessoalAprovadoEntity> VWQuadroPessoalAprovado { get; }
		ICommandRepository<WorkflowMensagemPadraoEntity> WorkflowMensagemPadrao { get; }
		ICommandRepository<WorkflowProtocoloEntity> WorkflowProtocolo { get; }
		ICommandRepository<WorkflowSituacaoInscricaoEntity> WorkflowSituacaoInscricao { get; }
		ICommandRepository<AladiEntity> Aladi { get; }
		ICommandRepository<NaladiEntity> Naladi { get; }
		ICommandRepository<RegimeTributarioEntity> RegimeTributario { get; }
		ICommandRepository<RegimeTributarioTesteEntity> RegimeTributarioTeste { get; }
		ICommandRepository<UnidadeReceitaFederalEntity> UnidadeReceitaFederal { get; }
		ICommandRepository<FundamentoLegalEntity> FundamentoLegal { get; }
		ICommandRepository<AnalistaEntity> Analista { get; }
		ICommandRepository<ParametroAnalista1Entity> ParametroAnalista1 { get; }
		ICommandRepository<PaizEntity> Paiz { get; }

		void DetachEntries();

		void Discart();

		IList<ProtocoloProcedure> ListaProtocolos(ProtocoloProcedure protocolo = null);

		void Save();

		int SelecionarNumeroInscricaoUnico(int tipoPessoa, int unidadeCadastradora);
	}
}