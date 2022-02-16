namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ArquivoVM
    {
        public byte[] Arquivo { get; set; }
        public int IdArquivo { get; set; }
        public string Nome { get; set; }
        public decimal Tamanho { get; set; }
        public string Tipo { get; set; }
    }
}