using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ConsultaProtocoloVM : PagedOptions
    {
        public int? Ano { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime? DataInclusao { get; set; }
        public int? IdServico { get; set; }
        public int? IdSituacao { get; set; }
        public bool IsJuridica { get; set; }
        public int? NumeroSequencial { get; set; }
        public string Token { get; set; }
    }
}