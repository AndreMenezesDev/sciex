using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class NaturezaJuridicaDto : BaseDto
    {
        public int? Codigo { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string Descricao { get; set; }
        public int? IdNaturezaGrupo { get; set; }
        public int? IdNaturezaJuridica { get; set; }
        public NaturezaGrupoDto NaturezaGrupo { get; set; }
        public bool Status { get; set; }
        public bool StatusQuadroSocial { get; set; }
    }
}