using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ManterNaturezaJuridicaVM
    {
        public int? Codigo { get; set; }
        public string Descricao { get; set; }
        public int? IdNaturezaGrupo { get; set; }
        public int? IdNaturezaJuridica { get; set; }
        public IEnumerable<QualificacaoVM> Qualificacoes { get; set; }
        public bool Status { get; set; }
        public bool StatusQuadroSocial { get; set; }
    }
}