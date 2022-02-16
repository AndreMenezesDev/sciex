using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class GridManterNaturezaJuridicaDto : BaseDto
    {
        public bool Ativo { get; set; }
        public string Codigo { get; set; }
        public string Grupo { get; set; }
        public string NaturezaJuridica { get; set; }
        public bool QuadroSocietario { get; set; }
    }

    public class ManterNaturezaJuridicaDto : BaseDto
    {
        /// <summary>
        /// Este campo irá receber a informação se existem duplicidades na qualificação
        /// </summary>
        public bool? IsQualificacoesDuplicadas { get; set; }

        public IEnumerable<NaturezaGrupoDto> NaturezaGrupos { get; set; } = Enumerable.Empty<NaturezaGrupoDto>();

        public NaturezaJuridicaDto NaturezaJuridica { get; set; }
        public IEnumerable<QualificacaoDto> Qualificacoes { get; set; } = Enumerable.Empty<QualificacaoDto>();

        public IEnumerable<GridManterNaturezaJuridicaDto> ResultadoConsulta { get; set; } = Enumerable.Empty<GridManterNaturezaJuridicaDto>();

        /// <summary>
        /// Registra o total de registros encontrados na tabela CADSUF_NATUREZA_JURIDICA para efeitos
        /// de validação
        /// </summary>
        public int? TotalEncontradoCodigoNaturezaJuridica { get; set; }
    }
}