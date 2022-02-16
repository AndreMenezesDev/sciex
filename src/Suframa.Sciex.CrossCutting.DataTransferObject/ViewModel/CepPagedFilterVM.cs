namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class CepPagedFilterVM : PagedOptions
    {
        public string Codigo { get; set; }
        public string Endereco { get; set; }
        public int? IdMunicipio { get; set; }
        public string TipoLogradouro { get; set; }
        public string Uf { get; set; }
    }
}