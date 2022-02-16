using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class MensagemPadraoVM
    {
        public string Descricao { get; set; }
        public int? IdMensagemPadrao { get; set; }
        public bool IsSelecionado { get; set; }
        public EnumTipoGrupoMensagemPadrao? TipoGrupo { get; set; }
    }
}