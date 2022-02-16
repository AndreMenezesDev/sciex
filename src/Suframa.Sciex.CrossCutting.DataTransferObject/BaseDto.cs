namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public abstract class BaseDto : IBaseDto
    {
		public int TipoOperacao { get; set; }
		public string Mensagem { get; set; }
		public bool AtualizarTela { get; set; }
	}
}