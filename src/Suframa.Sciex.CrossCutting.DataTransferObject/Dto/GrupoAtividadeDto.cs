namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class GrupoAtividadeDto : BaseDto
    {
        public int? Codigo { get; set; }
        public int? CodigoDivisaoAtividade { get; set; }
        public string Descricao { get; set; }
        public int? IdDivisaoAtividade { get; set; }
        public int? IdGrupoAtividade { get; set; }
    }
}