namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public partial class ParametroDistribuicaoAutomaticaVM : PagedOptions
    {
        public string DescricaoUnidadeCadastradora { get; set; }

        public int? IdParametroDistribuicaoAutomatica { get; set; }

        public int? IdUnidadeCadastradora { get; set; }

        public bool? IsAtivo { get; set; }
    }
}