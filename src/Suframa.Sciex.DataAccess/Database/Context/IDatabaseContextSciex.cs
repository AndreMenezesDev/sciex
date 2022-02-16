using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.DataAccess.Database
{
    public interface IDatabaseContextSciex
    {
        System.Data.Entity.Database Database { get; }

        void DetachEntries();

        void DiscartChanges();

        DbEntityEntry Entry(object entity);

        int SaveChanges();

        DbSet<T> Set<T>() where T : class;

		IList<LiDto> VerificaLiDoImportador(long liNumero, string cnpj);

		IList<ImportadorDto> ValidaLIReferenciaPertenceaImpotador(string importadorcodigo, string liNum);

		IList<LiDto> VerificaLiIndeferidoCancelado(string numeroLiReferencia);

		IList<LiDto> SelecionarIdOrigemLiReferencia(string li);

		IList<ParidadeCambialDto> ListarParidadeCambial(DateTime dtParidade, int idMoeda);

		IList<LiDto> SelecionarLiNuReferenciaPorAliId(long pme_id);

		ParidadeCambialVM GetParidadeCambial(DateTime dtParidade);
		ParidadeCambialVM ConsultarExistenciaParidadePorData(DateTime dtParidade);

		void ExcluirParidadeCambial(int idParidadeCambial);

		void CopiarParidadeCambialValor(int idParidadeCambialNew, int idParidadeCambialOld);

		void SalvarParidadeCambial(string sql);

		int VerificaAplicacaoLideReferencia(string li);
		
		List<string> ListarCnpjsLisDeferidas();
		
		List<LiEntity> ListarLiPorCnpj(string cnpj);
		int ContarQuantidadeInsumoPorProdutoEInscricaoCad(int inscCadastral, int codProduto, int StatusLeAlteracao);
		long BuscarUltimoCodigoSeqPlanoExportacao(string cnpjEmpresaLogada, int anoCorrente);

		void SP_ParecerTecnico(int IdProcesso);
		void SP_GerarParecerSuspensaoAlterado(int IdProcesso, int IdSolicitacaoAlteracao);
		void SP_GerarParecerHistoricoInsumo(int IdProcesso, int IdSolicitacaoAlteracao, string NomeResponsavel);
		void SP_GerarParecerSuspensaoCancelado(int IdProcesso);
	}
}