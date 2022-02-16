using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using System.Text;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContextSciex : DbContext, IDatabaseContextSciex
    {
		public virtual IList<LiDto> VerificaLiDoImportador(long liNumero, string cnpj)
		{
			StringBuilder vlQuery = new StringBuilder();

			vlQuery.AppendLine("select SCIEX_PLI_MERCADORIA.PME_ID, LI_NU, LI_ST, DI_ID, PLI_NU_CNPJ, IMP_DS_RAZAO_SOCIAL  ");
			vlQuery.AppendLine("from SCIEX_PLI_MERCADORIA																   ");
			vlQuery.AppendLine("INNER JOIN SCIEX_LI on SCIEX_LI.PME_ID = SCIEX_PLI_MERCADORIA.PME_ID					   ");
			vlQuery.AppendLine("INNER JOIN SCIEX_PLI on  SCIEX_PLI.PLI_ID = SCIEX_PLI_MERCADORIA.PLI_ID					   ");
			vlQuery.AppendLine("where LI_NU = '"+ liNumero +"' 															   ");
			vlQuery.AppendLine("AND LI_ST = 1																			   ");
			vlQuery.AppendLine("AND DI_ID IS NULL																		   ");
			vlQuery.AppendLine("AND PLI_NU_CNPJ = '"+cnpj+"'  												               ");

			return Database.SqlQuery<LiDto>(vlQuery.ToString()).ToList();
		}

		public virtual IList<LiDto> SelecionarLiNuReferenciaPorAliId(long pme_id)
		{
			StringBuilder vlQuery = new StringBuilder();

			vlQuery.AppendLine("SELECT pli_nu_li_referencia as NumeroLiReferencia");
			vlQuery.AppendLine("FROM SCIEX_PLI_MERCADORIA");
			vlQuery.AppendLine("INNER JOIN SCIEX_PLI ON SCIEX_PLI.PLI_ID = SCIEX_PLI_MERCADORIA.PLI_ID");
			vlQuery.AppendLine("WHERE SCIEX_PLI_MERCADORIA.PME_ID = " + pme_id);

			return Database.SqlQuery<LiDto>(vlQuery.ToString()).ToList();
		}

		public virtual IList<LiDto> VerificaLiIndeferidoCancelado(string numeroLiReferencia)
		{
			StringBuilder vlQuery = new StringBuilder();
			vlQuery.AppendLine("select c.PLI_ID as IdPli				   ");
			vlQuery.AppendLine("from SCIEX_PLI c						   ");
			vlQuery.AppendLine("where c.PLI_NU_LI_REFERENCIA = '" + numeroLiReferencia + "'");
			vlQuery.AppendLine("and not exists							   ");
			vlQuery.AppendLine("(										   ");
			vlQuery.AppendLine("	select								   ");
			vlQuery.AppendLine("	*									   ");
			vlQuery.AppendLine("	from SCIEX_PLI_MERCADORIA a			   ");
			vlQuery.AppendLine("		inner							   ");
			vlQuery.AppendLine("	join SCIEX_ALI b on a.PME_ID = b.PME_ID ");
			vlQuery.AppendLine("	where a.PLI_ID = c.PLI_ID		 	   ");
			vlQuery.AppendLine("	and b.ALI_ST IN (4,5,7,8,9)	    	   ");
			vlQuery.AppendLine(")										   ");

			return Database.SqlQuery<LiDto>(vlQuery.ToString()).ToList();
		}

		public virtual IList<LiDto> SelecionarIdOrigemLiReferencia(string li)
		{
			var vlQuery = $@"
								select SCIEX_LI_SUBSTITUIDA.pme_id_origem as IdLiRef
								from SCIEX_LI_SUBSTITUIDA, SCIEX_LI
								where pme_id_origem in ((SELECT PME_ID from SCIEX_LI where LI_NU = {li}))
								and SCIEX_LI.PME_ID = SCIEX_LI_SUBSTITUIDA.pme_id_substituida
							";

			return Database.SqlQuery<LiDto>(vlQuery).ToList();
		}

		public virtual IList<ImportadorDto> ValidaLIReferenciaPertenceaImpotador(string importadorcodigo , string liNum)
		{
			StringBuilder vlQuery = new StringBuilder();

			vlQuery.AppendLine("select SCIEX_PLI.INS_CO														");
			vlQuery.AppendLine("from SCIEX_PLI_MERCADORIA													");
			vlQuery.AppendLine("INNER JOIN SCIEX_LI on SCIEX_LI.PME_ID = SCIEX_PLI_MERCADORIA.PME_ID		");
			vlQuery.AppendLine("INNER JOIN SCIEX_PLI on  SCIEX_PLI.PLI_ID = SCIEX_PLI_MERCADORIA.PLI_ID		");
			vlQuery.AppendLine("where SCIEX_LI.LI_NU = '"+ liNum +"'										");
			vlQuery.AppendLine("AND SCIEX_PLI.INS_CO = '" + importadorcodigo+ "'							");

			return Database.SqlQuery<ImportadorDto>(vlQuery.ToString()).ToList();
		}

		public virtual IList<ParidadeCambialDto> ListarParidadeCambial(DateTime dtParidade, int idMoeda)
        {
			String dataDataridade = dtParidade.ToShortDateString();
			String consultaMoeda = "";

			if (idMoeda > 0)
				consultaMoeda = " and pva.MOE_ID = "+ idMoeda.ToString();

			String vlQuery;

			vlQuery = "Select pca.PCA_ID AS IdParidadeCambial," +
	                  "       Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) AS DataParidade," +
	                  "       Convert(Varchar(10), pca.PCA_DH_CADASTRO, 103) +' ' + Convert(Varchar(8), pca.PCA_DH_CADASTRO, 24) AS DataCadastro," +
	                  "       Convert(Varchar(10), pca.PCA_DT_ARQUIVO, 103) AS DataArquivo," +
	                  "       pca.PCA_NO_USUARIO AS NomeUsuario," +
                      "       pva.PVA_VL_PARIDADE AS Valor," +
                      "       moe.MOE_CO + ' - ' + moe.MOE_DS AS CodDscMoeda" +
                      "   From dbo.SCIEX_PARIDADE_CAMBIAL pca," +
		              "        dbo.SCIEX_PARIDADE_VALOR pva," +
		              "        dbo.SCIEX_MOEDA moe" +
//					  "   Where Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) = " + dtParidade.ToShortDateString() + " And" +
					  "   Where Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) = '"+ dataDataridade+"' And" +
					  "         pva.PCA_ID = pca.PCA_ID and" +
					  "         moe.MOE_ID = pva.MOE_ID" +consultaMoeda +
						"   Order by moe.MOE_DS";

			return Database.SqlQuery<ParidadeCambialDto>(vlQuery).ToList();
        }

		public virtual ParidadeCambialVM GetParidadeCambial(DateTime dtParidade)
		{
			String vlQuery;
			
			vlQuery = $@"Select pca.PCA_ID AS IdParidadeCambial,
					         pca.PCA_DT_PARIDADE AS DataParidade,
					         pca.PCA_DH_CADASTRO AS DataCadastro,
					         pca.PCA_DT_ARQUIVO AS DataArquivo,
					         pca.PCA_NO_USUARIO AS NomeUsuario,
					         pca.PCA_NU_USUARIO AS NumeroUsuario
					     From dbo.SCIEX_PARIDADE_CAMBIAL pca
					     Where Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) = '" + dtParidade.ToShortDateString() + "'";
					  //"   Where Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) = '13/08/2018'";

			return Database.SqlQuery<ParidadeCambialVM>(vlQuery).FirstOrDefault();
		}

		public virtual ParidadeCambialVM ConsultarExistenciaParidadePorData(DateTime dtParidade)
		{
			String vlQuery;

			vlQuery = $@"Select pca.PCA_ID AS IdParidadeCambial,
					         pca.PCA_DT_PARIDADE AS DataParidade,
					         pca.PCA_DH_CADASTRO AS DataCadastro,
					         pca.PCA_DT_ARQUIVO AS DataArquivo,
					         pca.PCA_NO_USUARIO AS NomeUsuario,
					         pca.PCA_NU_USUARIO AS NumeroUsuario
					     From dbo.SCIEX_PARIDADE_CAMBIAL pca
					     Where Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) = '" + dtParidade.ToShortDateString() + "'";

			return Database.SqlQuery<ParidadeCambialVM>(vlQuery).FirstOrDefault();
		}

		public int VerificaAplicacaoLideReferencia(string li)
		{
			StringBuilder query = new StringBuilder();
			query.AppendLine("select PAP_CO");
			query.AppendLine("from SCIEX_PLI A, SCIEX_PLI_MERCADORIA B, SCIEX_LI C, SCIEX_PLI_APLICACAO D");
			query.AppendLine("where A.PLI_ID = B.PLI_ID");
			query.AppendLine("AND B.PME_ID = C.PME_ID");
			query.AppendLine("AND A.PAP_ID = D.PAP_ID");
			query.AppendLine("AND C.LI_NU = " + li);

			return Database.SqlQuery<Int16>(query.ToString()).FirstOrDefault();
		}

		public int ContarQuantidadeInsumoPorProdutoEInscricaoCad(int inscCadastral, int idLe, int StatusLeAlteracao)
		{
			List<int> status = new List<int>() { 1, 2, 3};
			StringBuilder query = new StringBuilder();
			query.AppendLine("SELECT COUNT(b.lei_id) FROM SCIEX_LE_PRODUTO A ");
			query.AppendLine("LEFT JOIN SCIEX_LE_INSUMO B ON A.lep_id = B.lep_id ");
			query.AppendLine("WHERE A.LEP_NU_INSCRICAO_CADASTRAL = " + inscCadastral);
			query.AppendLine("AND A.LEP_ID = " + idLe);

			//if (StatusLeAlteracao != 3)
				query.AppendLine("AND (B.LEI_ST_INSUMO IN (1, 2) or b.lei_st_insumo is null)");
				//query.AppendLine("OR B.LEI_ST_INSUMO = null");


			return Database.SqlQuery<Int32>(query.ToString()).FirstOrDefault();
		}

		public void ExcluirParidadeCambial(int idParidadeCambial)
		{
			String vlQuery;

			try
			{
				vlQuery = "Delete From dbo.SCIEX_PARIDADE_VALOR  Where PCA_ID = " + idParidadeCambial.ToString();

				Database.ExecuteSqlCommand(vlQuery);

				vlQuery = "Delete  From dbo.SCIEX_PARIDADE_CAMBIAL  Where PCA_ID = " + idParidadeCambial.ToString();

				Database.ExecuteSqlCommand(vlQuery);
			}
			catch (Exception ex)
			{
				
			}			
		}

		public void CopiarParidadeCambialValor(int idParidadeCambialNew, int idParidadeCambialOld)
		{
			String vlQuery;

			vlQuery = "Insert Into dbo.SCIEX_PARIDADE_VALOR (PCA_ID," +
															  "MOE_ID," +
															  "PVA_VL_PARIDADE) " +
					  "Select " + idParidadeCambialNew.ToString() + "," +
					  		 "MOE_ID," +
							 "PVA_VL_PARIDADE" +
					  "   From dbo.SCIEX_PARIDADE_VALOR" +
					  "   Where PCA_ID = " + idParidadeCambialOld.ToString();

			Database.ExecuteSqlCommand(vlQuery);
		}

		public void SalvarParidadeCambial(string sql)
		{
			Database.ExecuteSqlCommand(sql);
		}

		public void Salvar(string sql)
		{
			Database.ExecuteSqlCommand(sql);

		}

		public virtual List<string> ListarCnpjsLisDeferidas()
		{
			String vlQuery = "select top(5) c.pli_nu_cnpj "
				+ "from sciex_li a, sciex_pli_mercadoria b, sciex_pli c "
				+ "where "
				+ "	a.pme_id = b.pme_id "
				+ "	and b.pli_id = c.pli_id "
				+ "	and li_st = 1 "
				+ "group by pli_nu_cnpj" +
				" ORDER BY NEWID()";

			return Database.SqlQuery<string>(vlQuery).ToList();
		}

		public virtual List<LiEntity> ListarLiPorCnpj(string cnpj)
		{
			String vlQuery = "select top(5) a.* from sciex_li a, sciex_pli_mercadoria b, " +
				"sciex_pli c where a.pme_id = b.pme_id  and b.pli_id = c.pli_id and li_st = 1  " +
				"and PLI_NU_CNPJ = '" + cnpj + "'";

			return Database.SqlQuery<LiEntity>(vlQuery).ToList();
		}

		public long BuscarUltimoCodigoSeqPlanoExportacao(string cnpjEmpresaLogada, int anoCorrente)
		{
			var query = $@"SELECT ISNULL(MAX(PEX_NU_PLANO),0)+1 
							 FROM SCIEX_PLANO_EXPORTACAO
					 WHERE PEX_NU_CNPJ = '{cnpjEmpresaLogada}'
					   AND PEX_NU_ANO_PLANO = {anoCorrente}
						";

			return Database.SqlQuery<Int64>(query.ToString()).FirstOrDefault();
		}

		public void SP_ParecerTecnico(int IdProcesso)
		{
			var result = Database.SqlQuery<List<object>>($@" 
														DECLARE @PCO_NU bigint;
														DECLARE @PCO_ANO int;
														EXEC ST_SCIEX_PARECER_TECNICO {IdProcesso}, @PCO_NU = @PCO_NU OUTPUT, @PCO_ANO = @PCO_ANO OUTPUT 
														", 
						new SqlParameter("@PCO_NU", System.Data.SqlDbType.BigInt).Direction = System.Data.ParameterDirection.Output,
						new SqlParameter("@PCO_ANO", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output
			).ToList();

		}
		public void SP_GerarParecerSuspensaoAlterado(int IdProcesso, int IdSolicitacaoAlteracao)
		{
			var result = Database.SqlQuery<List<object>>($@" 
						DECLARE @PCO_NU bigint;
						DECLARE @PCO_ANO int;
						EXEC ST_SCIEX_PARECER_TECNICO_SUSAL {IdProcesso}, {IdSolicitacaoAlteracao} , @PCO_NU = @PCO_NU OUTPUT, @PCO_ANO = @PCO_ANO OUTPUT
						", 
						new SqlParameter("@PCO_NU", System.Data.SqlDbType.BigInt).Direction = System.Data.ParameterDirection.Output,
						new SqlParameter("@PCO_ANO", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output
			).ToList();

		}
		public void SP_GerarParecerHistoricoInsumo(int IdProcesso, int IdSolicitacaoAlteracao, string NomeResponsavel)
		{
			var result = Database.SqlQuery<List<object>>($@" 
						EXEC ST_SCIEX_INSERE_HISTORICO_INSUMO {IdProcesso}, {IdSolicitacaoAlteracao}, '{NomeResponsavel}'
						"
			).ToList();

		}
		public void SP_GerarParecerSuspensaoCancelado(int IdProcesso)
		{
			var result = Database.SqlQuery<List<object>>($@" 
														DECLARE @PCO_NU bigint;
														DECLARE @PCO_ANO int;
														EXEC ST_SCIEX_PARECER_TECNICO_SUSCA {IdProcesso}, @PCO_NU = @PCO_NU OUTPUT, @PCO_ANO = @PCO_ANO OUTPUT 
														", 
						new SqlParameter("@PCO_NU", System.Data.SqlDbType.BigInt).Direction = System.Data.ParameterDirection.Output,
						new SqlParameter("@PCO_ANO", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output
			).ToList();

		}
	}
}