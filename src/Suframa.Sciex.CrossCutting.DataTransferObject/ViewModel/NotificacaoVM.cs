using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class NotificacaoVM
	{
		public int? Ano { get; set; }

		public string Cnpj { get; set; }

		public string Cpf { get; set; }

		public string Data
		{
			get
			{
				return DataInclusao.HasValue ? DataInclusao.Value.ToString("dd/MM/yyyy") : string.Empty;
			}
		}

		public DateTime? DataInclusao { get; set; }

		public IEnumerable<string> Documentos { get; set; }

		public string Email { get; set; }

		public string Hora
		{
			get
			{
				return DataInclusao.HasValue ? DataInclusao.Value.ToString("HH:mm:ss") : string.Empty;
			}
		}

		public int? IdDadosSolicitante { get; set; }

		public int? IdPessoaFisica { get; set; }

		public int? IdPessoaJuridica { get; set; }

		public int? IdRequerimento { get; set; }

		public int? IdWorkflowProtocolo { get; set; }

		public bool IsAtualizacaoDadosCadastraisCredenciamento
		{
			get
			{
				return TipoRequerimento == EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaTransportador ||
						TipoRequerimento == EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaRemetente ||
						TipoRequerimento == EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaTransportador;
			}
		}

		public bool IsCancelamento
		{
			get
			{
				return StatusProtocolo == EnumStatusProtocolo.Cancelado;
			}
		}

		public bool IsCancelamentoInscricao
		{
			get
			{
				return TipoRequerimento == EnumTipoRequerimento.CancelamentoInscricaoCadastralPessoaFisica || TipoRequerimento == EnumTipoRequerimento.CancelamentoInscricaoCadastralPessoaJuridica;
			}
		}

		public bool IsCancelamentoInscricaoCadastral { get; set; }

		public bool IsCredenciamento
		{
			get
			{
				return TipoRequerimento == EnumTipoRequerimento.CredenciamentoPessoaJuridicaTransportador ||
						TipoRequerimento == EnumTipoRequerimento.CredenciamentoPessoaFisicaTransportador ||
						TipoRequerimento == EnumTipoRequerimento.CredenciamentoPessoaJuridicaRemetente;
			}
		}

		public string Justificativa { get; set; }

		public IEnumerable<string> MensagemPadrao { get; set; }

		public string Nome { get; set; }

		public string NumeroProtocolo
		{
			get
			{
				return (Ano.HasValue && NumeroSequencial.HasValue) ? NumeroSequencial.Value + "/" + Ano : string.Empty;
			}
		}

		public int? NumeroSequencial { get; set; }

		public string Orientacao { get; set; }

		public string RazaoSocial { get; set; }

		public string Senha { get; set; }

		public string Servico { get; set; }

		public EnumStatusProtocolo StatusProtocolo { get; set; }

		public EnumTipoRequerimento TipoRequerimento { get; set; }

		public string UnidadeCadastradora { get; set; }

		public string Usuario { get; set; }
	}
}