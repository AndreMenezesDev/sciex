using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
    public enum EnumPapel
    {
        [Description("Técnico")]
        Tecnico = 1,

        [Description("Superintendente Adjunto")]
        SuperintendenteAdjunto = 2,

        [Description("Coordenador Geral COCAD")]
        CoordenadorGeralCocad = 3,

        [Description("Coordenador Descentralizada")]
        CoordenadorDescentralizada = 4,

        [Description("Coordenador de Outras Áreas")]
        CoordenadorOutrasAreas = 5
    }
}