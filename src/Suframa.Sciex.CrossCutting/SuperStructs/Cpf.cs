using System;

namespace Suframa.Sciex.CrossCutting.SuperStructs
{
	public struct Cpf
	{
		private string _value;

		public bool IsValid { get; private set; }
		public string Masked { get; private set; }
		public string Unmasked { get; private set; }

		public Cpf(string value)
		{
			value = value ?? string.Empty;
			this._value = value.ExtractNumbers();

			this.Unmasked = Unmask(value);
			this.Masked = Mask(value);
			this.IsValid = Validate(this.Unmasked);
		}

		public static implicit operator Cpf(string d)
		{
			return new Cpf(d);
		}

		public static implicit operator string(Cpf d)
		{
			return d.ToString();
		}

		public static string Mask(string value)
		{
			if (string.IsNullOrWhiteSpace(value)) { return null; }
			value = value.ExtractNumbers();
			return !string.IsNullOrWhiteSpace(value) ? Convert.ToUInt64(value).ToString(@"000\.000\.000\-00") : string.Empty;
		}

		public static string Unmask(string value)
		{
			if (string.IsNullOrWhiteSpace(value)) { return null; }
			return value.ExtractNumbers().PadLeft(11, '0');
		}

		public static bool Validate(string cpf)
		{
			if (string.IsNullOrEmpty(cpf))
				return false;

			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digito;

			int soma;
			int resto;

			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");

			if (cpf.Length != 11)
			{
				return false;
			}
			tempCpf = cpf.Substring(0, 9);

			soma = 0;

			for (int i = 0; i < 9; i++)
			{
				soma += int.Parse(tempCpf[i].ToString()) * (multiplicador1[i]);
			}
			resto = soma % 11;

			if (resto < 2)
			{
				resto = 0;
			}
			else
			{
				resto = 11 - resto;
			}

			digito = resto.ToString();
			tempCpf = tempCpf + digito;
			int soma2 = 0;

			for (int i = 0; i < 10; i++)
			{
				soma2 += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
			}

			resto = soma2 % 11;

			if (resto < 2)
			{
				resto = 0;
			}
			else
			{
				resto = 11 - resto;
			}

			digito = digito + resto.ToString();
			return cpf.EndsWith(digito);
		}

		public override string ToString()
		{
			return this.Unmasked;
		}
	}
}