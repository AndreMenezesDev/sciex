using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class CepVM
    {
        public string Bairro { get; set; }
        public string Codigo { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataInclusao { get; set; }
        public string Endereco { get; set; }
        public int IdCep { get; set; }
        public int IdMunicipio { get; set; }
        public string Logradouro { get; set; }
        public MunicipioVM Municipio { get; set; }
    }
}