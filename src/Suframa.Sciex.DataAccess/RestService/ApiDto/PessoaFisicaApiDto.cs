using System;

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
    public class PessoaFisicaApiDto
    {
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string munNome { get; set; }
        public string pfBairro { get; set; }
        public string pfCep { get; set; }
        public string pfComplemento { get; set; }
        public string pfCpf { get; set; }
        public string pfLogradouro { get; set; }
        public string pfNome { get; set; }
        public string pfNumero { get; set; }
        public string ufSigla { get; set; }
    }
}