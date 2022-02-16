using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class UnidadeCadastradoraDto : BaseDto
    {
        public int? Codigo { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string Descricao { get; set; }
        public int? IdMunicipio { get; set; }
        public int? IdUnidadeCadastradora { get; set; }
        public bool IsUnidadeCadastradoraManaus { get; set; }
        public int? TotalMunicipioSecundario { get; set; }
        public string UF { get; set; }
    }
}