using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum

{
    public enum EnumTipoSetorEmpresarial
    {
        Nenhum = 0,

        [Description("Primário")]
        Primario = 1,

        [Description("Secundário")]
        Secundario = 2,

        [Description("Terciário")]
        Terciario = 3
    }
}