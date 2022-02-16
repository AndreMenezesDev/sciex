using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
    public enum EnumTipoEstabelecimento
    {
        Nenhum = 0,

        [Description("Matríz")]
        Matriz = 1,

        [Description("Filial")]
        Filial = 2,
    }
}