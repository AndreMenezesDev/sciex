using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class PedidoCorrecaoVM
    {
        public EnumAcao Acao { get; set; }
        public string Campo { get; set; }
        public string CampoDe { get; set; }
        public string CampoPara { get; set; }
        public DateTime? DataCorrecao { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public int? IdCampoSistema { get; set; }
        public int? IdMensagemPadrao { get; set; }
        public int? IdPedidoCorrecao { get; set; }
        public int? IdProtocolo { get; set; }
        public int? IdTabela { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string Justificativa { get; set; }
    }
}