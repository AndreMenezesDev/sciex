using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class ManterUnidadeCadastradoraDto : BaseDto
    {
        public IEnumerable<MunicipioDto> Municipios { get; set; } = Enumerable.Empty<MunicipioDto>();
        public int? TotalEncontradoRequerimento { get; set; }
        public int? TotalEncontradoUnidadeCadastradora { get; set; }

        /// <summary>
        /// Este campo irá receber a informação se existem duplicidades na Unidade Secundaria
        /// </summary>
        public int? TotalEncontradoUnidadeSecundaria { get; set; }

        public int? TotalUnidadeCadastradoraDescricao { get; set; }
        public UnidadeCadastradoraDto UnidadeCadastradora { get; set; }
    }
}