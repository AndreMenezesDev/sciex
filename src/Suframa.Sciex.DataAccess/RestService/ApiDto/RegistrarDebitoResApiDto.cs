using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
    public class RegistrarDebitoResApiDto
    {
        public int AnoDebito { get; set; }
        public string DataVencimento { get; set; }
        public string Mensagem { get; set; }
        public string MensagemErro { get; set; }
        public int Numero { get; set; }
        public string requestBody { get; set; }
        public EnumStatusTaxaServico StatusDePara { get; set; }
        public int TipoDebitoRetorno { get; set; }
    }
}