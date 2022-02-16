namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ManterAtividadeEconomicaPagedFilterVM : PagedOptions
    {
        public int? Codigo { get; set; }
        public string Descricao { get; set; }
        public int? IdClasseAtividade { get; set; }
        public int? IdDivisaoAtividade { get; set; }
        public int? IdGrupoAtividade { get; set; }
        public int? IdSecaoAtividade { get; set; }
        public int? IdSubClasseAtividade { get; set; }
        public bool Status { get; set; }
    }
}