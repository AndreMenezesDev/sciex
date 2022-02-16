using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class ManterSetorEmpresarialDto : BaseDto
    {
        public IEnumerable<SubClasseAtividadeDto> ListaSubClasseAtividade { get; set; }
        public SetorDto SetorDto { get; set; }

        public int? TotalEncontradoCodigoSetor { get; set; }
    }
}