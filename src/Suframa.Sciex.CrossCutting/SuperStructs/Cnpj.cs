namespace Suframa.Sciex.CrossCutting.SuperStructs
{
	public struct Cnpj
	{
		private string _value;

		public bool IsValid { get; private set; }
		public string Masked { get; private set; }
		public string Unmasked { get; private set; }

		public Cnpj(string value)
		{
			value = value ?? string.Empty;
			this._value = value.ExtractNumbers();

			this.Unmasked = value.CnpjUnformat();
			this.Masked = value.CnpjUnformat();
			this.IsValid = Validate(this.Unmasked);
		}

		private static bool Validate(string cnpj)
		{
			if (string.IsNullOrEmpty(cnpj))
				return false;

			int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int soma;
			int resto;
			string digito;
			string tempCnpj;
			cnpj = cnpj.Trim();
			cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
			if (cnpj.Length != 14)
				return false;
			tempCnpj = cnpj.Substring(0, 12);
			soma = 0;
			for (int i = 0; i < 12; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = resto.ToString();
			tempCnpj = tempCnpj + digito;
			soma = 0;
			for (int i = 0; i < 13; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = digito + resto.ToString();
			return cnpj.EndsWith(digito);
		}

		public static implicit operator Cnpj(string d)
		{
			return new Cnpj(d);
		}

		public static implicit operator string(Cnpj d)
		{
			return d.ToString();
		}

		public override string ToString()
		{
			return this.Unmasked;
		}
	}
}