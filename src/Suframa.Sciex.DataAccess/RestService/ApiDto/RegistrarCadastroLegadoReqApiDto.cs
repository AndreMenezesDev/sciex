using System;
using System.Collections.Generic;
using System.Linq;

/*
*  de:	willy kocher <willykocherr@gmail.com>
*	para: Fernando Supi <fernando.supi@gmail.com>
*	data: 2 de janeiro de 2018 20:36
*	assunto: json empresa
{
"tipoOperacao": 1,
	Empresa: {
		"operacao": 1,
		"empCnpj": "44452757000191",
		"empRazaoSocial": "RAZAO SOCIAL",
		"empNomeFantasia": "NOME FANTASIA",
		"empTelefone1": "33333
		3333",
		"empTpImportador": 4",
		empTpEstabelec": 1,
		"empPorte": 4,
		"empOptSimples": 0,
		"empCep": "69068000",
		"empLogradouro": "LOGRADOURO",
		"empNumero": "NUMERO",
		"empBairro": "BAIRRO",
		"empComplemento": "COMPLEMENTO",
		"munCd": 1302603,
		"natCd": 2143,
		"locCd": 1,
		"empDtCadastro": "07/12/2017 19:06:44"
	},
	EmpresaInscricao: {
		"operacao": 1,
		"empCnpj": "44452757000191",
		"inscSuf": 200112341,
		"sit": 2,
		"setCd": "18",
		"dtVal": "31/12/2100"
	},
	EmpresaSetores: {
		"insert": [
			{
				"empCnpj": "44452757000191",
				"setCd": "2"
			},
			{
				"empCnpj": "44452757000191",
				"setCd": "19"
			}
		],
		"update": [],
		"delete": []
},
EmpresaPolo: {
	"operacao": 1,
	"empCnpj
	": "44452757000191",
	"inscSuf": 200112341,
	"setCd": "19",
	"sSetCd": "0"
},
EmpresaAtividades : {
		"insert": [
			{
				"empCnpj": "44452757000191",
				"ativEmpSequencial": 1,
				"ativCodigo": "4637102",
				"ativEmpTipo": "0",
				"inscSuf": 200112341
			},
			{
				"empCnpj": "44452757000191",
				"ativEmpSequencial": 2,
				"ativCodigo": "2612300",
				"ativEmpTipo": "1",
				"inscSuf": 200112341
			}
		],
		"update": [],
		"delete": []
	}
}
*/

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
    public class RegistrarCadastroLegadoReqApiDto
    {
        //public long? TipoOperacao { get; set; } //"tipoOperacao": 1
        public EmpresaApi empresa { get; set; } //"Empresa"

        //public EmpresaPoloApi empresaPolo { get; set; } //"EmpresaPolo"
        public EmpresaAtividadesApi empresaAtividadeVO { get; set; }

        public EmpresaInscricaoApi empresaInscricao { get; set; } //"EmpresaInscricao"
        public EmpresaSetoresApi empresaSetorVO { get; set; } //"EmpresaSetores"
                                                              //"EmpresaAtividades"

        public class EmpresaApi
        {
            /// <summary>
            /// "operacao": 1
            /// </summary>
            //public int? Operacao { get; set; }

            /// <summary>
            /// "empComplemento": "COMPLEMENTO"
            /// CEP_DS_BAIRRO – CADSUF_CEP
            /// </summary>
            public string empBairro { get; set; }

            /// <summary>
            /// "empCep": "69068000"
            /// CEP_CO – CADSUF_CEP
            /// </summary>
            public long empCep { get; set; }

            /// <summary>
            /// "empCnpj": "44452757000191"
            /// PJU_CO_CNPJ – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empCnpj { get; set; }

            /// <summary>
            /// "COMPLEMENTO"
            /// PJU_DS_COMPLEMENTO – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empComplemento { get; set; }

            /// <summary>
            /// "empDtCadastro": "07/12/2017 19:06:44"
            /// INS_DT_ABERTURA – CADSUF_INSCRICAO_CADASTRAL
            /// MODIFICADO PARA: PJU_DT_REGISTRO - CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empDtCadastro { get; set; }

            /// <summary>
            /// "empLogradouro": "LOGRADOURO"
            /// CEP_TP_LOGRADOURO + CEP_DS_ENDERECO – CADSUF_CEP
            /// </summary>
            public string empLogradouro { get; set; }

            /// <summary>
            /// "empNomeFantasia": "NOME FANTASIA"
            /// PJU_DS_NOME_FANTASIA – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empNomeFantasia { get; set; }

            /// <summary>
            /// "empNumero": "NUMERO"
            /// PJU_NU_ENDERECO – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empNumero { get; set; }

            /// <summary>
            /// empOptSimples": 0
            /// PJU_ST_OP_SIMPLES – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public long? empOptSimples { get; set; }

            /// <summary>
            /// "empPorte": 4
            /// PEM_ID – CADSUF_PORTE_empRESA
            /// </summary>
            public long? empPorte { get; set; }

            /// <summary>
            /// "empRazaoSocial": "RAZAO SOCIAL"
            /// PJU_DS_RAZAO_SOCIAL – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empRazaoSocial { get; set; }

            /// <summary>
            /// "empTelefone1": "333333333"
            /// PJU_NU_TELEFONE – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public long? empTelefone1 { get; set; }

            /// <summary>
            /// "empTpEstabelec": 1
            /// PJU_TP_ESTABELECIMENTO – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public long? empTpEstabelec { get; set; }

            /// <summary>
            /// "empTpImportador": 4
            /// Valor default fixo = “4”
            /// </summary>
            public long? empTpImportador { get; set; }

            /// <summary>
            /// "locCd": 1
            /// UND_DS – CADSUF_UNIDADE_CADASTRADORA
            /// MODIFICADO PARA: UND_CO
            /// </summary>
            public long? locCd { get; set; }

            /// <summary>
            /// "munCd": 1302603
            /// MUN_DS_MUNICIPIO – CADSUF_MUNICIPIO
            /// </summary>
            public long? munCd { get; set; }

            /// <summary>
            /// "natCd": 2143
            /// NJU_DS_JUR – CADSUF_NATUREZA_JURIDICA (fonte desatualizado ainda não tem o atributo em PessoaJuridicaEntity)
            /// </summary>
            public long? natCd { get; set; }
        }

        public class EmpresaAtividadeApi
        {
            /// <summary>
            /// "ativCodigo": "4637102"
            /// SBC_CO – CADSUF_SUBCLASSE_ATIVIDADE
            /// </summary>
            public string ativCodigo { get; set; }

            /// <summary>
            /// "ativEmpSequencial": 1
            /// </summary>
            //public long ativEmpSequencial { get; set; }
            /// <summary>
            /// "ativEmpTipo": "0"
            /// ATV_TP – CADSUF_SUBCLASSE_ATIVIDADE
            /// 1 – Principal
            /// 2 – Secundária
            /// </summary>
            public string ativEmpTipo { get; set; }

            /// <summary>
            /// "empCnpj": "44452757000191"
            /// PJU_CO_CNPJ – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empCnpj { get; set; }

            /// <summary>
            /// "inscSuf": 200112341
            /// INS_NU – CADSUF_PESSOA_JURIDICA_INSCRICAO_ESTADUAL
            /// </summary>
            public string inscSuf { get; set; }
        }

        public class EmpresaAtividadesApi
        {
            public IEnumerable<EmpresaAtividadeApi> empresaAtividades { get; set; } = Enumerable.Empty<EmpresaAtividadeApi>();

            /// <summary>
            /// 0 – Nenhuma Ação
            /// 1 – INSERT
            /// 2 – UPDATE
            /// 3 – DELETE
            /// </summary>
            //public EnumIntegracaoTipoOperacao? TipoOperacao { get; set; }
        }

        public class EmpresaInscricaoApi
        {
            /// <summary>
            /// "operacao": 1
            /// </summary>
            //public int? Operacao { get; set; }

            /// <summary>
            /// "dtVal": "31/12/2100"
            /// Valor default fixo ="31/12/2100"
            /// </summary>
            public string dtVal { get; set; }

            /// <summary>
            /// "empCnpj": "44452757000191"
            /// PJU_CO_CNPJ – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empCnpj { get; set; }

            /// <summary>
            /// "inscSuf": 200112341
            /// Nulo
            /// </summary>
            public int? inscSuf { get; set; }

            /// <summary>
            /// "setCd": "18"
            /// Valor default fixo ="10”
            /// </summary>
            public long? setCd { get; set; }

            /// <summary>
            /// "sit": 2
            /// STI_CO – CADSUF_SITUACAO_INSCRICAO
            /// 1 - NÃO HABILITADA – Quando o CADSUF for [2 – BLOQUEADA,  3 – INATIVA, 4 - CANCELADA)
            /// 2 - HABILITADA – Quando o CADSUF for [1 - ATIVA]
            /// </summary>
            public long? sit { get; set; }
        }

        public class EmpresaSetorApi
        {
            /// <summary>
            /// "empCnpj": "44452757000191"
            /// PJU_CO_CNPJ – CADSUF_PESSOA_JURIDICA
            /// </summary>
            public string empCnpj { get; set; }

            /// <summary>
            /// "setCd": "2"
            /// SET_CO – CADSUF_SETOR
            /// </summary>
            public long setCd { get; set; }
        }

        public class EmpresaSetoresApi
        {
            public IEnumerable<EmpresaSetorApi> empresaSetores { get; set; } = Enumerable.Empty<EmpresaSetorApi>();

            /// <summary>
            /// 0 – Nenhuma Ação
            /// 1 – INSERT
            /// 2 – UPDATE
            /// 3 – DELETE
            /// </summary>
            //public int? TipoOperacao { get; set; }
        }

        /// <summary>
        /// informado pela Analista Monique que deverá ser NULL
        /// </summary>
        //public class EmpresaPoloApi
        //{
        //	/// <summary>
        //	/// "operacao": 1
        //	/// </summary>
        //	//public long? Operacao { get; set; }

        //	/// <summary>
        //	/// "empCnpj": "44452757000191"
        //	/// </summary>
        //	public string empCnpj { get; set; }

        //	/// <summary>
        //	/// "inscSuf": 200112341
        //	/// </summary>
        //	public string inscSuf { get; set; }

        //	/// <summary>
        //	/// "setCd": "19"
        //	/// </summary>
        //	public string setCd { get; set; }

        //	/// <summary>
        //	/// "sSetCd": "0"
        //	/// </summary>
        //	//public string SSetCd { get; set; }
        //}
    }
}