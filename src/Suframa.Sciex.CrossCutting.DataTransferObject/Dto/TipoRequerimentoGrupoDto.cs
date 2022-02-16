using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
	public static class TipoRequerimentoGrupoDto
	{
		public static List<int> AcompanhamentoProtocolo
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaPreposto,
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaConsultor
				};
			}
		}

		public static List<int> Atualizacao
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaConsultor,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaPreposto,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaRemetente,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica
				};
			}
		}

		public static List<int> ConsultaPublica
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaFisica
				};
			}
		}

		public static List<int> Credenciamento
		{
			get
			{
				return new List<int>
				{
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaConsultor,
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaPreposto,
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaTransportador,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaRemetente,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaConsultor,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaPreposto,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaRemetente,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaTransportador
				};
			}
		}

		public static List<int> CredenciamentoAuditor
		{
			get
			{
				return new List<int>
				{
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaAuditor,
				};
			}
		}

		public static List<int> CredenciamentoPessoaFisica
		{
			get
			{
				return new List<int>
				{
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaConsultor,
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaPreposto,
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaTransportador,
				};
			}
		}

		public static List<int> CredenciamentoTransportador
		{
			get
			{
				return new List<int>
				{
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaTransportador,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaTransportador,
				};
			}
		}

		public static List<int> DistribuicaoManual
		{
			get
			{
				return new List<int>
				{
					(int)EnumTipoRequerimento.AtualizacaoDocumentosPessoaJuridica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.CancelamentoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.EnviarRecursoPessoaJuridica,
					(int)EnumTipoRequerimento.DesbloqueioPessoaJuridica,
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.AtualizacaoDocumentosPessoaFisica,
					(int)EnumTipoRequerimento.DesbloqueioPessoaFisica,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaAuditor,
				};
			}
		}

		public static List<int> Externo
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaConsultor,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaPreposto,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.CancelamentoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaPreposto,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.DesbloqueioPessoaJuridica,
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica
				};
			}
		}

		public static List<int> ExternoExistente
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaConsultor,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaPreposto,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.CancelamentoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.DesbloqueioPessoaJuridica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica
				};
			}
		}

		public static List<int> IniciaRequerimentoFechado
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaRemetente,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaRemetente,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaTransportador
				};
			}
		}

		public static List<int> InscricaoCadastral
		{
			get
			{
				return new List<int>
				{
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica,
				};
			}
		}

		public static List<int> InscricaoCadastralJuridica
		{
			get
			{
				return new List<int>
				{
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica
				};
			}
		}

		public static List<int> NaoPossuiDocumentoComprobatorio
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaRemetente,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaRemetente,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaTransportador,
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaTransportador,
					(int)EnumTipoRequerimento.CredenciamentoPessoaFisicaTransportador
				};
			}
		}

		public static List<int> NaoPossuiStatusComunicacao
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.CredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaAuditor,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaJuridica
				};
			}
		}

		public static List<int> Reativacao
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaFisica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica
				};
			}
		}

		public static List<int> StatusAtivarPessoa
		{
			get
			{
				return new List<int> {
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaTransportador,
					(int)EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaRemetente
				};
			}
		}

		public static List<int> TiposRequerimentoLegado
		{
			get
			{
				return new List<int>
				{
					(int)EnumTipoRequerimento.InscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica,
					(int)EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica
				};
			}
		}
	}
}