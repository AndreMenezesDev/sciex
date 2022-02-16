using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum

{
    public enum EnumTipoAtividade
    {
        Nenhum = 0,

        [Description("Principal")]
        Principal = 1,

        [Description("Secundária")]
        Secundaria = 2
    }
}