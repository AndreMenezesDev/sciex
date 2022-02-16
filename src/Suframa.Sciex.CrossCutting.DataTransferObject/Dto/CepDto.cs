using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class CepDto : BaseDto
    {
        public string Bairro { get; set; }
        public bool CepEncontrado { get; set; }
        public string Codigo { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string Endereco { get; set; }
        public int? IdCep { get; set; }
        public int? IdMunicipio { get; set; }
        public string Logradouro { get; set; }
        public MunicipioDto Municipio { get; set; }
        public int TotalEncontradoCep { get; set; }
        public string UF { get; set; }
    }
}