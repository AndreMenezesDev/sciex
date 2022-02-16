using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class CredenciadoConsultorDto : BaseDto
    {
        public int locCd { get; set; }
        public string munCd { get; set; }
        public string pfBairro { get; set; }
        public string pfCep { get; set; }
        public string pfComplemento { get; set; }
        public string pfCpf { get; set; }
        public string pfDtCadastro { get; set; }
        public string pfEmail { get; set; }
        public string pfInscSuf { get; set; }
        public string pfLogradouro { get; set; }
        public string pfNome { get; set; }
        public string pfNumero { get; set; }
        public string pfStatus { get; set; }
        public string pfTelefone1 { get; set; }
        public string pfTipoPesFis { get; set; }
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
}