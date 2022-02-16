namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class DivisaoAtividadeDto : BaseDto
    {
        public int? Codigo { get; set; }
        public string Descricao { get; set; }
        public int? IdDivisaoAtividade { get; set; }
        public int? IdSecaoAtividade { get; set; }
    }
}