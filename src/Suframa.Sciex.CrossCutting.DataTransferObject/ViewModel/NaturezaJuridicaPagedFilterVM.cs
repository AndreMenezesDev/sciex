namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class NaturezaJuridicaPagedFilterVM : PagedOptions
    {
        public int? Codigo { get; set; }
        public string Descricao { get; set; }
        public int? IdNaturezaGrupo { get; set; }
    }
}