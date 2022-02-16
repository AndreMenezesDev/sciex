using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ProtocoloHtmlVM
	{
		public int? Ano { get; set; }
		public string CnpjSolicitacao { get; set; }

		public string CnpjSolicitacaoFormatado
		{
			get
			{
				if (!string.IsNullOrEmpty(CnpjSolicitacao))
				{
					return Convert.ToUInt64(CnpjSolicitacao).ToString(@"00\.000\.000\/0000\-00");
				}

				return "";
			}
		}

		public string CpfSolicitacao { get; set; }

		public string CpfSolicitacaoFormatado
		{
			get
			{
				if (!string.IsNullOrEmpty(CpfSolicitacao))
				{
					return Convert.ToUInt64(CpfSolicitacao).ToString(@"000\.000\.000\-00");
				}

				return "";
			}
		}

		public string CpfSolicitante { get; set; }

		public string CpfSolicitanteFormatado
		{
			get
			{
				if (!string.IsNullOrEmpty(CpfSolicitante))
				{
					return Convert.ToUInt64(CpfSolicitante).ToString(@"000\.000\.000\-00");
				}

				return "";
			}
		}

		public string Data
		{
			get
			{
				if (DataInclusao.HasValue)
				{
					return DataInclusao.Value.ToString("dd/MM/yyyy");
				}

				return "";
			}
		}

		public DateTime? DataInclusao { get; set; }
		public string DescricaoServico { get; set; }
		public string DescricaoUnidadeCadastradora { get; set; }

		public string Hora
		{
			get
			{
				if (DataInclusao.HasValue)
				{
					return DataInclusao.Value.ToString("HH:mm");
				}

				return "";
			}
		}

		public string NomeSolicitacao { get; set; }
		public string NomeSolicitante { get; set; }

		public string NumeroProtocolo
		{
			get
			{
				return (NumeroSequencial.HasValue && Ano.HasValue) ? (NumeroSequencial.Value + "/" + Ano.Value).ToString() : null;
			}
		}

		public int? NumeroSequencial { get; set; }
		public string RazaoSocialSolicitacao { get; set; }
	}
}