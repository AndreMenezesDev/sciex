using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
	public class InscricaoCadastralDto
	{
		private Regex slice = new Regex(@"^(?<tipoPessoa>[0-9]{2})(?<unidadeCadastradora>[0-9]{2})(?<sequencial>[0-9]{4})(?<dv>[0-9]{1})$");

		public int CodigoUnidadeCadastradora { get; private set; }

		public int DV { get; private set; }

		public int Sequencial { get; private set; }

		public EnumTipoPessoa TipoPessoa { get; private set; }

		public InscricaoCadastralDto(string unidadeCadastradora)
		{
			Match match = this.slice.Match(unidadeCadastradora);

			// Validar formato
			if (!match.Success)
				throw new Exception("O código da unidade cadastradora informada não contem um formato válido");

			// Validar tipo pessoa
			switch (match.Groups["tipoPessoa"].Value)
			{
				case "10":
					this.TipoPessoa = EnumTipoPessoa.PessoaFisica;
					break;

				case "20":
					this.TipoPessoa = EnumTipoPessoa.PessoaJuridica;
					break;

				default:
					throw new Exception("Tipo da pessoa inválido");
			}

			this.DV = Convert.ToInt32(match.Groups["dv"].Value);
			this.CodigoUnidadeCadastradora = Convert.ToInt32(match.Groups["unidadeCadastradora"].Value);
			this.Sequencial = Convert.ToInt32(match.Groups["sequencial"].Value);

			// Validar dv
			if (!this.ChecarDV())
			{
				throw new Exception("Dígito verificador inválido");
			}
		}

		public InscricaoCadastralDto(EnumTipoPessoa tipoPessoa, int codigoUnidadeCadastradora, int sequencial)
		{
			this.TipoPessoa = tipoPessoa;
			this.CodigoUnidadeCadastradora = codigoUnidadeCadastradora;
			this.Sequencial = sequencial;
			this.DV = this.GerarDV();
		}

		private bool ChecarDV(int dv)
		{
			return this.GerarDV() == dv;
		}

		private bool ChecarDV()
		{
			return this.GerarDV() == this.DV;
		}

		private int GerarDV()
		{
			var unidadeCadastradoraSemDV = this.ToString().Substring(0, this.ToString().Length - 1);

			int sum = 0;
			int weight = 9;
			foreach (var c in unidadeCadastradoraSemDV)
			{
				sum += Convert.ToInt32(c.ToString()) * weight;
				weight--;
			}

			var mod = sum % 11;
			var dv = new int[] { 0, 1 }.Contains(mod) ? 0 : (11 - mod);

			return dv;
		}

		public override string ToString()
		{
			return $"{(this.TipoPessoa == EnumTipoPessoa.PessoaFisica ? "10" : "20")}{this.CodigoUnidadeCadastradora:D2}{this.Sequencial:D4}{this.DV}";
		}
	}
}