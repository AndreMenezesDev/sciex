using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
    public enum EnumTipoCoberturaCambial
	{
        [Description("ATÉ 180 DIAS")]
		[DefaultValue("ATÉ 180 DIAS")]
		ATÉ_180_DIAS = 1,

        [Description("DE 180 ATÉ 360 DIAS")]
		DE_180_ATÉ_360_DIAS = 2,

		[Description("ACIMA DE 360 DIAS")]
		ACIMA_DE_360_DIAS = 3,

		[Description("SEM COBERTURA")]
		SEM_COBERTURA = 4,
	}
}