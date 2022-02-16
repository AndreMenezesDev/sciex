using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class MunicipioVM
    {
        public string Descricao { get; set; }
        public int? IdMunicipio { get; set; }
        public IEnumerable<UnidadeCadastradoraVM> UnidadeCadastradora { get; set; }
		public int? Id { get; set; }
	}
}