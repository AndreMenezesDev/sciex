using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContext : DbContext, IDatabaseContext
	{
		public virtual DbSet<AgendaAtendimentoEntity> AgendaAtendimento { get; set; }
		public virtual DbSet<ArquivoEntity> Arquivo { get; set; }
		public virtual DbSet<ArquivoInformacaoEntity> ArquivoInformacao { get; set; }
		public virtual DbSet<CalendarioEntity> Calendario { get; set; }
		public virtual DbSet<CalendarioAgendamentoEntity> CalendarioAgendamento { get; set; }
		public virtual DbSet<CalendarioDiaEntity> CalendarioDia { get; set; }
		public virtual DbSet<CalendarioHoraEntity> CalendarioHora { get; set; }
		public virtual DbSet<CampoSistemaEntity> CampoSistema { get; set; }
		public virtual DbSet<CepEntity> Cep { get; set; }
		public virtual DbSet<ClasseAtividadeEntity> ClasseAtividade { get; set; }
		public virtual DbSet<ControleExecucaoJobEntity> ControleExecucaoJob { get; set; }
		public virtual DbSet<ControleExecucaoServicoEntity> ControleExecucaoServico { get; set; }
		public virtual DbSet<CredenciamentoEntity> Credenciamento { get; set; }
		public virtual DbSet<DadosSolicitanteEntity> DadosSolicitante { get; set; }
		public virtual DbSet<DicionarioDropDownEntity> DicionarioDropDown { get; set; }
		public virtual DbSet<DivisaoAtividadeEntity> DivisaoAtividade { get; set; }
		public virtual DbSet<FeriadoEntity> Feriado { get; set; }
		public virtual DbSet<FilaAnalistaEntity> FilaAnalista { get; set; }
		public virtual DbSet<GrupoAtividadeEntity> GrupoAtividade { get; set; }
		public virtual DbSet<HistoricoSituacaoInscricaoEntity> HistoricoSituacaoInscricao { get; set; }
		public virtual DbSet<InscricaoCadastralEntity> InscricaoCadastral { get; set; }
		public virtual DbSet<InscricaoCadastralCredenciamentoEntity> InscricaoCadastralCredenciamento { get; set; }
		public virtual DbSet<InscricaoSuframaLegadoEntity> InscricaoSuframaLegado { get; set; }
		public virtual DbSet<JustificativaProtocoloEntity> JustificativaProtocolo { get; set; }
		public virtual DbSet<ListaDocumentoEntity> ListaDocumentos { get; set; }
		public virtual DbSet<ListaServicoEntity> ListaServico { get; set; }
		public virtual DbSet<MensagemPadraoEntity> MensagemPadrao { get; set; }
		
		public virtual DbSet<MotivoSituacaoInscricaoEntity> MotivoSituacaoInscricao { get; set; }
		public virtual DbSet<MunicipioEntity> Municipio { get; set; }
		public virtual DbSet<NaturezaGrupoEntity> NaturezaGrupo { get; set; }
		public virtual DbSet<NaturezaJuridicaEntity> NaturezaJuridica { get; set; }
		public virtual DbSet<NaturezaQualificacaoEntity> NaturezaQualificacao { get; set; }
		public virtual DbSet<PaisEntity> Pais { get; set; }
		public virtual DbSet<PapelEntity> Papel { get; set; }		
		public virtual DbSet<ParametroAnalistaServicoEntity> ParametroAnalistaServico { get; set; }
		public virtual DbSet<ParametroDistribuicaoAutomaticaEntity> ParametroDistribuicaoAutomatica { get; set; }
		public virtual DbSet<PedidoCorrecaoEntity> PedidoAlteracao { get; set; }
		public virtual DbSet<PessoaFisicaEntity> PessoaFisica { get; set; }
		public virtual DbSet<PessoaJuridicaEntity> PessoaJuridica { get; set; }
		public virtual DbSet<PessoaJuridicaAdministradorEntity> PessoaJuridicaAdministrador { get; set; }
		public virtual DbSet<PessoaJuridicaAtividadeEntity> PessoaJuridicaAtividade { get; set; }
		public virtual DbSet<PessoaJuridicaInscricaoEntity> PessoaJuridicaInscricao { get; set; }
		public virtual DbSet<PessoaJuridicaInscricaoEstadualEntity> PessoaJuridicaInscricaoEstadual { get; set; }
		public virtual DbSet<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; set; }
		public virtual DbSet<PorteEmpresaEntity> PorteEmpresa { get; set; }
		public virtual DbSet<ProtocoloEntity> Protocolo { get; set; }
		public virtual DbSet<QuadroPessoalEntity> QuadroPessoal { get; set; }
		public virtual DbSet<QualificacaoEntity> Qualificacao { get; set; }
		public virtual DbSet<RecursoEntity> Recurso { get; set; }
		public virtual DbSet<RequerimentoEntity> Requerimento { get; set; }
		public virtual DbSet<RequerimentoDocumentoEntity> RequerimentoDocumento { get; set; }
		public virtual DbSet<SecaoAtividadeEntity> SecaoAtividade { get; set; }
		public virtual DbSet<SeqInscricaoCadastralEntity> SeqInscricaoCadastral { get; set; }
		public virtual DbSet<ServicoEntity> Servico { get; set; }
		public virtual DbSet<SetorEntity> Setor { get; set; }
		public virtual DbSet<SetorAtividadeEntity> SetorAtividade { get; set; }
		public virtual DbSet<SituacaoInscricaoEntity> SituacaoInscricao { get; set; }
		public virtual DbSet<StatusProtocoloEntity> StatusProtocolo { get; set; }
		public virtual DbSet<SubclasseAtividadeEntity> SubclasseAtividade { get; set; }
		public virtual DbSet<TaxaServicoEntity> TaxaServico { get; set; }
		public virtual DbSet<TipoDocumentoEntity> TipoDocumento { get; set; }
		public virtual DbSet<TipoIncentivoEntity> TipoIncentivo { get; set; }
		public virtual DbSet<TipoRequerimentoEntity> TipoRequerimento { get; set; }
		public virtual DbSet<TipoUsuarioEntity> TipoUsuario { get; set; }
		public virtual DbSet<UFEntity> UF { get; set; }
		public virtual DbSet<UnidadeCadastradoraEntity> UnidadeCadastradora { get; set; }
		public virtual DbSet<UnidadeSecundariaEntity> UnidadeSecundaria { get; set; }
		public virtual DbSet<UsuarioInternoEntity> UsuarioInterno { get; set; }
		public virtual DbSet<UsuarioPapelEntity> UsuarioInternoPapel { get; set; }
		public virtual DbSet<VWDocumentoEntity> VWDocumento { get; set; }
		public virtual DbSet<VWPessoaFisicaAprovadoEntity> VWPessoaFisicaAprovado { get; set; }
		public virtual DbSet<VWPessoaJuridicaAdministradorAprovadoEntity> VWPessoaJuridicaAdministradorAprovado { get; set; }
		public virtual DbSet<VWPessoaJuridicaAprovadoEntity> VWPessoaJuridicaAprovado { get; set; }
		public virtual DbSet<VWPessoaJuridicaAtividadeAprovadoEntity> VWPessoaJuridicaAtividadeAprovado { get; set; }
		public virtual DbSet<VWPessoaJuridicaInscricaoEstadualAprovadoEntity> VWPessoaJuridicaInscricaoEstadualAprovado { get; set; }
		public virtual DbSet<VWPessoaJuridicaSocioAprovadoEntity> VWPessoaJuridicaSocioAprovado { get; set; }
		public virtual DbSet<VWQuadroPessoalAprovadoEntity> VWQuadroPessoalAprovado { get; set; }
		public virtual DbSet<WorkflowMensagemPadraoEntity> WorkflowMensagemPadrao { get; set; }
		public virtual DbSet<WorkflowSituacaoInscricaoEntity> WorkflowSituacaoInscricao { get; set; }
		public virtual DbSet<RegimeTributarioTesteEntity> RegimeTributarioTeste { get; set; }
		public virtual DbSet<ParametroAnalistaEntity> ParametroAnalista { get; set; }
		public virtual DbSet<PaizEntity> Paiz { get; set; }
		public virtual DbSet<ViewMunicipioEntity> ViewMunicipio { get; set; }
		public virtual DbSet<ViewSetorEntity> ViewSetor { get; set; }
		public virtual DbSet<ViewAtividadeEconomicaPrincipalEntity> ViewAtividadeEconomicaPrincipal { get; set; }
		public virtual DbSet<ViewSetorEmpresaEntity> ViewSetorEmpresaEntity { get; set; }

		public DatabaseContext()
					: base("name=DatabaseContext")
		{
			// Disable Migrations
			// https://stackoverflow.com/questions/18667172/how-can-i-disable-migration-in-entity-framework-6-0
			System.Data.Entity.Database.SetInitializer<DatabaseContext>(null);

			// Enable Migrations Database.SetInitializer(new
			// MigrateDatabaseToLatestVersion<DatabaseContext,
			// Suframa.Sciex.DataAccess.Database.Migrations.Configuration>());

			// Enable Log
			// https://cmatskas.com/logging-and-tracing-with-entity-framework-6/
			//DbInterception.Add(new DatabaseInterceptor());
			Database.Log = NLog.LogManager.GetLogger("db").Trace;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//modelBuilder.HasDefaultSchema(PrivateSettings.DEFAULT_DB_SCHEMA);

			ConfigureAgendaAtendimento(modelBuilder);
			ConfigureArquivo(modelBuilder);
			ConfigureArquivoInformacao(modelBuilder);
			ConfigureCampoSistema(modelBuilder);
			ConfigureCep(modelBuilder);
			ConfigureDadosSolicitante(modelBuilder);
			ConfigureDicionarioDropDown(modelBuilder);
			ConfigureInscricaoSuframaLegado(modelBuilder);
			
			//ConfigurePais(modelBuilder);			
			ConfigurePedidoAlteracao(modelBuilder);
			ConfigurePessoaFisica(modelBuilder);
			ConfigurePessoaJuridica(modelBuilder);
			ConfigurePessoaJuridicaAdministrador(modelBuilder);
			ConfigurePessoaJuridicaInscricao(modelBuilder);
			ConfigurePessoaJuridicaInscricaoEstadual(modelBuilder);
			ConfigurePessoaJuridicaSocio(modelBuilder);
			ConfigurePorteEmpresa(modelBuilder);
			ConfigureProtocolo(modelBuilder);
			ConfigureQuadroPessoal(modelBuilder);
			ConfigureRequerimento(modelBuilder);
			ConfigureRequerimentoDocumento(modelBuilder);
			ConfigureServico(modelBuilder);
			ConfigureSituacaoInscricao(modelBuilder);
			ConfigureStatusProtocolo(modelBuilder);
			ConfigureSubclasseAtividade(modelBuilder);
			ConfigureTaxaServico(modelBuilder);
			ConfigureTipoDocumento(modelBuilder);
			ConfigureTipoRequerimento(modelBuilder);
			ConfigureTipoUsuario(modelBuilder);
			ConfigureVWPessoaJuridicaAprovado(modelBuilder);
			ConfigureRegimeTributarioTeste(modelBuilder);
			ConfigureParametroAnalista(modelBuilder);
			ConfigurePaiz(modelBuilder);
			ConfigureViewMunicipio(modelBuilder);
			ConfigureViewSetor(modelBuilder);
			ConfigureViewAtividadeEconomicaPrincipalEntity(modelBuilder);
			ConfigureViewSetorEmpresa(modelBuilder);
		}

		public void DetachEntries()
		{
			foreach (var entry in this.ChangeTracker.Entries())
			{
				entry.State = EntityState.Detached;
			}
		}

		/// <summary>
		/// https://stackoverflow.com/questions/16437083/dbcontext-discard-changes-without-disposing
		/// </summary>
		public void DiscartChanges()
		{
			foreach (var entry in this.ChangeTracker.Entries().Where(w => w.State != EntityState.Unchanged))
			{
				switch (entry.State)
				{
					case EntityState.Modified:
					case EntityState.Deleted:
						entry.State = EntityState.Modified; //Revert changes made to deleted entity.
						entry.State = EntityState.Unchanged;
						break;

					case EntityState.Added:
						entry.State = EntityState.Detached;
						break;
				}
			}
		}

		public override int SaveChanges()
		{
			var objectStateEntries = ChangeTracker.Entries()
				.Where(e => e.Entity is IData && (e.State == EntityState.Modified || e.State == EntityState.Added)).ToList();
			var currentTime = DateTime.Now;

			foreach (var entry in objectStateEntries)
			{
				var entityBase = entry.Entity as IData;
				if (entityBase == null) continue;
				if (entry.State == EntityState.Added)
				{
					entityBase.DataInclusao = currentTime;
				}
				else
				{
					// Não reescrever a data de inclusão quando não for operação de inclusão
					entry.Property(nameof(IData.DataInclusao)).IsModified = false;
				}

				entityBase.DataAlteracao = currentTime;
			}

			return base.SaveChanges();
		}
	}
}