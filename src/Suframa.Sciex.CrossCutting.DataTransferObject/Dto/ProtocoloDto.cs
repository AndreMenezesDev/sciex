namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class ProtocoloDto : BaseDto
    {
        public bool IsCaptchaValido { get; set; }
        public bool IsExigeAnalise { get; set; }

        public bool IsProtocoloExistente { get; set; }
        public bool IsTipoConsultaPermitido { get; set; }
    }
}