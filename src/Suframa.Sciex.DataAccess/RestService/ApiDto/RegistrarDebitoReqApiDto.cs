namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
    public class RegistrarDebitoReqApiDto
    {
        public int AnoSolicitacao { get; set; }
        public string CnpjCpf { get; set; }
        public int CobrarDebito { get; set; }
        public ExtratoCadastroApi ExtratoCadastro { get; set; }
        public int InscricaoSuframa { get; set; }
        public int Localidade { get; set; }
        public int NumeroSolicitacao { get; set; }
        public string RazaoSocial { get; set; }
        public ServicoApi Servico { get; set; }
        public double ValorDebito { get; set; }
        public decimal ValorReducao { get; set; }

        public class ExtratoCadastroApi
        {
            public string DataGeracao { get; set; }
        }

        public class ServicoApi
        {
            public int? Codigo { get; set; }
        }
    }
}