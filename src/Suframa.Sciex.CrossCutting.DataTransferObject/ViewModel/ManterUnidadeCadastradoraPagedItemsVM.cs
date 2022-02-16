using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ManterUnidadeCadastradoraPagedItemsVM : PagedOptions
    {
        public string Descricao { get; set; }
        public int? IdMunicipio { get; set; }
        public int? IdUnidadeCadastradora { get; set; }
        public IEnumerable<MunicipioVM> MunicipiosSecundarios { get; set; }
        public string UF { get; set; }
    }
}