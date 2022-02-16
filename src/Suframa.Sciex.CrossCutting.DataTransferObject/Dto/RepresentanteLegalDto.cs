using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class RepresentanteLegalDto : BaseDto
    {
        public int cidId { get; set; }
        public int estId { get; set; }
        public string repBairro { get; set; }
        public string repCep { get; set; }
        public string repCpfCnpj { get; set; }
        public string repDataCadastro { get; set; }
        public string repEmail { get; set; }
        public string repLogradouro { get; set; }
        public string repNomeRazaoSocial { get; set; }
        public string repNumero { get; set; }
        public string repTelefone { get; set; }
        public int sitId { get; set; }
    }
}