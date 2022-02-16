using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class CredenciamentoVM
    {
        public int? IdCredenciamento { get; set; }
        public int? IdPessoaFisica { get; set; }
        public int? IdPessoaJuridica { get; set; }
        public int? IdTipoUsuario { get; set; }
        public EnumTipoTransportador? ModalidadeTransportador { get; set; }
    }
}