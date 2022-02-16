using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ConsultaPublicaVM
    {
        public string CpfCnpj { get; set; }
        public DateTime? DataRestricao { get; set; }
        public int? IdArquivo { get; set; }
        public int? IdConsultaPublica { get; set; }
        public int? IdPessoaFisica { get; set; }
        public int? IdPessoaJuridica { get; set; }
        public int? IdPessoaJuridicaAdministrador { get; set; }
        public int? IdPessoaJuridicaSocio { get; set; }
        public int? IdProtocolo { get; set; }
        public int? IdTipoConsultaPublica { get; set; }
        public string NomeArquivo { get; set; }
        public string NomeConsulta { get; set; }
        public string NomeRazaoSocial { get; set; }
        public bool? StatusRestricao { get; set; }
        public EnumOrigemConsultaPublica TipoOrigem { get; set; }
    }
}