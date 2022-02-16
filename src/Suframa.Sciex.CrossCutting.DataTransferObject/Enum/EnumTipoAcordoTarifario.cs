using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
    public enum EnumTipoAcordoTarifario
    {
        [Description("Não Selecionado")]
        NÃO_SELECIONADO = 0,

        [Description("ALADI")]
        ALADI = 2,

		[Description("OMC")]
		OMC = 3,

		[Description("SGPC")]
		SGPC = 4,
	}
}