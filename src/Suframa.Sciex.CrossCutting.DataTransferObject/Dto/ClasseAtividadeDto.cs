namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class ClasseAtividadeDto : BaseDto
    {
        public int? Codigo { get; set; }
        public int? CodigoDivisao { get; set; }
        public int? CodigoGrupo { get; set; }
        public string Descricao { get; set; }
        public int? IdClasseAtividade { get; set; }
        public int? IdGrupoAtividade { get; set; }
    }
}