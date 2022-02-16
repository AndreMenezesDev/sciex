namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class InscricaoCadastralAtividadeVM
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public bool IsExercida { get; set; }

        public string IsExercidaDescricao
        {
            get
            {
                return IsExercida ? "Sim" : "Não";
            }
        }
    }
}