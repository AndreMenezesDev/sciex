using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class AuditorDto
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
}