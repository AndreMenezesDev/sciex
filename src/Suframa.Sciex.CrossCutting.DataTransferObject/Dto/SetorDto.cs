using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class SetorDto : PagedOptions
    {
        public int? Codigo { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string Descricao { get; set; }
        public int? IdSetor { get; set; }
        public string Observacao { get; set; }
        public bool Status { get; set; }
        public EnumTipoSetorEmpresarial? Tipo { get; set; }
    }
}