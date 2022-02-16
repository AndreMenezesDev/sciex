namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class MunicipioDto : BaseDto
    {
        public decimal? Codigo { get; set; }
        public string Descricao { get; set; }
        public int? IdMunicipio { get; set; }
        public string SiglaUF { get; set; }
        public string UF { get; set; }
    }
}