using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ConsultaPublicaResponsaveisVM
    {
        public string CpfCnpj { get; set; }
        public int? IdPessoaFisica { get; set; }
        public int? IdPessoaJuridica { get; set; }
        public int? IdPessoaJuridicaAdministrador { get; set; }
        public int? IdPessoaJuridicaSocio { get; set; }
        public int? IdProtocolo { get; set; }
        public int? IdResponsavel { get; set; }
        public bool IsRestricao { get; set; }
        public bool IsSituacao { get; set; }
        public string NomeRazaoSocial { get; set; }
        public EnumTipoPessoaDropList TipoResponsavel { get; set; }
    }
}