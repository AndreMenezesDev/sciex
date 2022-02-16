using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class CredenciadoRemTranspDto : BaseDto
    {
        public int cidId { get; set; }
        public int estId { get; set; }
        public int modTranspId { get; set; }
        public string remTraBairro { get; set; }
        public string remTraCep { get; set; }
        public string remTraCnpjCpf { get; set; }
        public string remTraComplemento { get; set; }
        public string remTraDtCadastro { get; set; }
        public string remTraEmail { get; set; }
        public string remTraLogradouro { get; set; }
        public int remTraNatureza { get; set; }
        public string remTraNomeRazaoSocial { get; set; }
        public string remTraNumero { get; set; }
        public string remTraTelefone { get; set; }
        public int remTraTipo { get; set; }
        public int sitId { get; set; }
    }
}