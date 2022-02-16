using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class AgendaAtendimentoVM
    {
        public string CpfCnpj { get; set; }
        public DadosSolicitanteVM DadosSolicitante { get; set; }
        public DateTime? DataAtendimento { get; set; }
        public string DataAtual { get { return DateTime.Now.ToString("dd/MM/yyyy"); } }
        public string DescricaoDataAtendimento { get { return DataAtendimento.HasValue ? DataAtendimento.Value.ToString("dd/MM/yyyy") : string.Empty; } }
        public string DescricaoHorarioAtendimento { get { return HorarioAtendimento.HasValue ? HorarioAtendimento.Value.ToString("HH:mm") : string.Empty; } }
        public string DescricaoServico { get; set; }
        public string DescricaoUnidadeCadastradora { get; set; }
        public IEnumerable<string> Documentos { get; set; }
        public string Endereco { get; set; }
        public string Guiche { get; set; }
        public string HoraAtual { get { return DateTime.Now.ToString("HH:mm"); } }
        public DateTime? HorarioAtendimento { get; set; }
        public int? IdAgendaAtendimento { get; set; }
        public int? IdCalendarioHora { get; set; }
        public int? IdServico { get; set; }
        public int? IdUnidadeCadastradora { get; set; }
        public bool IsLiConcordo { get; set; }
        public string NomeRazaoSocial { get; set; }
        public string NumeroProtocolo { get; set; }
        public string NumeroSenha { get; set; }
        public EnumTipoAgendaAtendimento Tipo { get; set; }
        public string Token { get; set; }
    }
}