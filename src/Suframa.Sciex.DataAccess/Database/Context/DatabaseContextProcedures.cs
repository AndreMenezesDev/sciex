using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext, IDatabaseContext
    {
        public virtual IList<ProtocoloProcedure> ListarProtocolos(ProtocoloProcedure protocolo)
        {
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            var result = Database.SqlQuery<ProtocoloProcedure>("exec ST_SEL_LISTA_PROTOCOLO @PRT_ID, @DATA_PROTOCOLO_INICIAL, @DATA_PROTOCOLO_FINAL, @DATA_DESIGNACAO_INICIAL, @DATA_DESIGNACAO_FINAL, @NUMERO_PROTOCOLO, @ANO_PROTOCOLO, @ID_SITUACAO_PROTOCOLO, @ID_TIPO_SERVICO, @CPF_CNPJ, @NOME_RAZAOSOCIAL, @ID_RESPONSAVEL, @IS_COCAD, @IS_GRUPOPROTOCOLOANALISE, @ID_UNIDADE, @SORT, @REVERSE, @PAGE, @SIZE, @USI_NO",
                new SqlParameter("@PRT_ID", protocolo.PRT_ID ?? (object)DBNull.Value),
                new SqlParameter("@DATA_PROTOCOLO_INICIAL", protocolo.DATA_PROTOCOLO_INICIAL ?? (object)DBNull.Value),
                new SqlParameter("@DATA_PROTOCOLO_FINAL", protocolo.DATA_PROTOCOLO_FINAL ?? (object)DBNull.Value),
                new SqlParameter("@DATA_DESIGNACAO_INICIAL", protocolo.DATA_DESIGNACAO_INICIAL ?? (object)DBNull.Value),
                new SqlParameter("@DATA_DESIGNACAO_FINAL", protocolo.DATA_DESIGNACAO_FINAL ?? (object)DBNull.Value),
                new SqlParameter("@NUMERO_PROTOCOLO", protocolo.NUMERO_PROTOCOLO ?? (object)DBNull.Value),
                new SqlParameter("@ANO_PROTOCOLO", protocolo.ANO_PROTOCOLO ?? (object)DBNull.Value),
                new SqlParameter("@ID_SITUACAO_PROTOCOLO", protocolo.ID_SITUACAO_PROTOCOLO ?? (object)DBNull.Value),
                new SqlParameter("@ID_TIPO_SERVICO", protocolo.ID_TIPO_SERVICO ?? (object)DBNull.Value),
                new SqlParameter("@CPF_CNPJ", protocolo.CPF_CNPJ ?? (object)DBNull.Value),
                new SqlParameter("@NOME_RAZAOSOCIAL", protocolo.NOME_RAZAOSOCIAL ?? (object)DBNull.Value),
                new SqlParameter("@ID_RESPONSAVEL", protocolo.ID_RESPONSAVEL ?? (object)DBNull.Value),
                new SqlParameter("@IS_COCAD", protocolo.IS_COCAD ?? (object)DBNull.Value),
                new SqlParameter("@IS_GRUPOPROTOCOLOANALISE", protocolo.IS_GRUPOPROTOCOLOANALISE ?? (object)DBNull.Value),
                new SqlParameter("@ID_UNIDADE", protocolo.ID_UNIDADE ?? (object)DBNull.Value),
                new SqlParameter("@SORT", protocolo.SORT ?? (object)DBNull.Value),
                new SqlParameter("@REVERSE", protocolo.REVERSE ?? (object)DBNull.Value),
                new SqlParameter("@PAGE", protocolo.PAGE ?? (object)DBNull.Value),
                new SqlParameter("@SIZE", protocolo.SIZE ?? (object)DBNull.Value),
                new SqlParameter("@USI_NO", protocolo.USI_NO ?? (object)DBNull.Value)
            );

            return result.ToList();
        }

        public virtual int SelecionarNumeroInscricaoUnico(int tipoPessoa, int unidadeCadastradora)
        {
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            var result = Database.SqlQuery<int>("exec ST_SEL_NUMERO_INSCRICAO @idTipoPessoa, @idUnidadeCadastradora",
                new SqlParameter("@idTipoPessoa", tipoPessoa),
                new SqlParameter("@idUnidadeCadastradora", unidadeCadastradora)
            );

            return result.First<int>();
        }
    }
}