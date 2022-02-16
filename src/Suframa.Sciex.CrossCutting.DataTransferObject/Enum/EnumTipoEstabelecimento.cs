using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
    public enum EnumTipoEntidadeRegistro
    {
        Nenhum = 0,

        [Description("Cartório")]
        Cartorio = 1,

        [Description("Junta Comercial")]
        JuntaComercial = 2,

        [Description("OAB")]
        Oab = 3,

        [Description("RFB")]
        Rfb = 4
    }
}