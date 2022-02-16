namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class SecaoAtividadeDto : BaseDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public int? IdSecaoAtividade { get; set; }
    }
}