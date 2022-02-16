namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ManterEnderecoVM
    {
        public string Bairro { get; set; }
        public bool CepEncontrado { get; set; }
        public string Codigo { get; set; }
        public string Endereco { get; set; }
        public int? IdCep { get; set; }
        public int IdMunicipio { get; set; }
        public string Logradouro { get; set; }
        public string UF { get; set; }
    }
}