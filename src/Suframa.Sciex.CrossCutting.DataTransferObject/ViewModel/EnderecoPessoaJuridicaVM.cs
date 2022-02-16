namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class EnderecoPessoaJuridicaVM
    {
        public string Complemento { get; set; }

        public string Email { get; set; }

        //CEP
        public ManterEnderecoVM Endereco { get; set; }

        public int? IdCep { get; set; }

        // Pessoa Juridica
        public int? IdPessoaJuridica { get; set; }

        public int IdUnidadeCadastradora { get; set; }

        public string NumeroEndereco { get; set; }
        public string PontoReferencia { get; set; }

        public string Ramal { get; set; }
        public string Telefone { get; set; }
    }
}