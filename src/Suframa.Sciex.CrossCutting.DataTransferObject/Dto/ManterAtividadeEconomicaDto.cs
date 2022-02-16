namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class ManterAtividadeEconomicaDto : BaseDto
    {
        public int? IdDivisaoAtividade { get; set; }
        public int? IdGrupoAtividade { get; set; }
        public int? IdSecaoAtividade { get; set; }
        public SubClasseAtividadeDto SubClasseAtividadeDto { get; set; }
    }
}