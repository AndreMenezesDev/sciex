using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class IdentificacaoPessoaFisicaVM
	{
		public CepVM Cep { get; set; }
		public string Codigo { get; set; }
		public string Complemento { get; set; }
		public string Cpf { get; set; }
		public DateTime? DataAlteracao { get; set; }
		public DateTime? DataInclusao { get; set; }
		public IEnumerable<DocumentoComprobatorioVM> DocumentosComprobatorios { get; set; }
		public string Email { get; set; }
		public ManterEnderecoVM Endereco { get; set; }
		public int IdCep { get; set; }
		public int? IdPessoaFisica { get; set; }
		public int? IdProtocolo { get; set; }
		public int? IdRequerimento { get; set; }
		public int? IdTipoRequerimento { get; set; }
		public int? IdUnidadeCadastradora { get; set; }
		public bool IsCredenciamentoTransportador { get; set; }
		public bool IsGerarProtocolo { get; set; }
		public EnumTipoTransportador? ModalidadeTransportador { get; set; }
		public string Nome { get; set; }
		public string NomeSocial { get; set; }
		public string NumeroEndereco { get; set; }
		public string PontoReferencia { get; set; }
		public int? Ramal { get; set; }
		public int? StatusComunicacao { get; set; }
		public decimal? Telefone { get; set; }
		public EnumTipoOrigemRequisicao TipoOrigem { get; set; }
	}
}