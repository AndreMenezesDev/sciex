namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class UsuarioVM
    {
        public string id { get; set; }

        public string locCd { get; set; }

        public string setCd { get; set; }

        public string usuCpf { get; set; }
        public string usuDtUltimoLogin { get; set; }
        public string usuDtValidCadastro { get; set; }
        public string usuDtValidSenha { get; set; }
        public string usuEmail { get; set; }
        public string usuEstagiaro { get; set; }
        public string usuLoginAnt { get; set; }
        public string usuNm { get; set; }

        public string usuPssStTpAcesso { get; set; }
        public string usuPssTpAcesso { get; set; }
        public string usuSenha { get; set; }
        public string usuSenhaAnt1 { get; set; }
        public string usuSenhaAnt2 { get; set; }
        public string usuSenhaAnt3 { get; set; }
        public string usuSenhaAnt4 { get; set; }
        public string usuSenhaAnt5 { get; set; }
        public string usuSt { get; set; }
        public string usuTipo { get; set; }
		public int status { get; set; }
	}
}