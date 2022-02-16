using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.Validation
{
	public class Funcoes
	{
		public string FormatarMascaraValor(string valor, int mascara, int tamanhoMascara)
		{
			if (valor.Trim() == "")
			{
				switch (mascara)
				{
					case 5: { return "0,00000"; }
					case 7: { return "0,0000000"; }
					default:
						break;
				}
			}

			try
			{
				Convert.ToDecimal(valor);
			}
			catch
			{
				return "0";
			}

			int contadorPonto = 0;
			int contadorVirgula = 0;

			string valorFormatado = "";
			string valorformatar = "";

			for (int i = 0; i < valor.Length; i++)
			{
				if (valor[i] == '.')
				{
					contadorPonto = contadorPonto + 1;
				}

				if (valor[i] == ',')
				{
					contadorVirgula = contadorVirgula + 1;
				}
			}


			if (contadorPonto == 1 && contadorVirgula == 0)
			{
				valorformatar = valor.Replace(".", ",");
			}

			if (contadorPonto >= 1 && contadorVirgula == 1)
			{
				valorformatar = valor.Replace(".", "");
			}

			if (contadorPonto == 0 && contadorVirgula == 1)
			{
				valorformatar = valor;
			}

			string[] dados = valorformatar.Split(',');

			if (dados.Length > 1)
			{
				switch (mascara)
				{
					case 5:
						{
							if (dados[0].Length > tamanhoMascara || dados[1].Length > mascara)
							{
								return "Valor não permitido, excede o limite de  " + tamanhoMascara.ToString() + " inteiros e 5 decimais ";
							}
							else
							{
								dados[1] = dados[1].PadRight(mascara, '0').Substring(0, mascara);
								valorFormatado = Convert.ToDecimal(dados[0] + "," + dados[1]).ToString("N5");
							}
							break;
						}
					case 7:
						{
							if (dados[0].Length > tamanhoMascara || dados[1].Length > mascara)
							{
								return "Valor não permitido, excede o limite de  " + tamanhoMascara.ToString() + " inteiros e 7 decimais ";
							}

							dados[1] = dados[1].PadRight(mascara, '0').Substring(0, mascara);
							valorFormatado = Convert.ToDecimal(dados[0] + "," + dados[1]).ToString("N7");
							break;
						}
					default:
						break;
				}
			}
			else
			{
				switch (mascara)
				{
					case 5:
						{
							if (valor.Length > tamanhoMascara)
							{
								return "Valor não permitido, excede o limite de  " + tamanhoMascara.ToString() + " inteiros e 5 decimais ";
							}
							else
							{
								valorFormatado = Convert.ToDecimal(valor).ToString("N5");
							}
							break;
						}
					case 7:
						{
							if (valor.Length > tamanhoMascara)
							{
								return "Valor não permitido, excede o limite de  " + tamanhoMascara.ToString() + " inteiros e 7 decimais ";
							}

							valorFormatado = Convert.ToDecimal(valor).ToString("N7");
							break;
						}
					default:
						break;
				}
			}

			return valorFormatado;
		}
		public ValoresVM Valor(string valor1, string valor2)
		{
			ValoresVM obj = new ValoresVM();

			decimal valorR1 = 0;
			decimal valorR2 = 0;

			if (valor1.Trim().Length > 0)
				valorR1 = Convert.ToDecimal(valor1.Replace(".", ""));
			if (valor2.Trim().Length > 0)
				valorR2 = Convert.ToDecimal(valor2.Replace(".", ""));

			obj.ValorQuantidade = valorR1;
			obj.ValorCondicao = valorR2;
			obj.ValorTotalFormatado = (valorR1 * valorR2).ToString("N7");
			obj.ValorTotalDecimal = (valorR1 * valorR2);

			return obj;
		}
	}
}
