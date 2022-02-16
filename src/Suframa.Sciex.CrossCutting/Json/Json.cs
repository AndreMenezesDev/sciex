using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Suframa.Sciex.CrossCutting.Json
{
	public static class Json
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		private static string CleanJson(string sourceJson)
		{
			sourceJson = "{" + sourceJson.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("true", "Sim").Replace("false", "Não").Replace(".", "").Replace("-", "") + "}";
			return sourceJson;
		}

		private static JToken FormatPropertyValue(string key, JToken value)
		{
			if (value == null || value.ToString().Equals("null")) { return null; }

			// Formatar datas
			if (value.ToString().Length >= 8 && (key.Contains("data") || key.Contains("_DT_")))
			{
				value = value.ToString().Substring(0, 8);

				value = string.Format("{0}/{1}/{2}",
					value.ToString().Substring(6, 2),
					value.ToString().Substring(4, 2),
					value.ToString().Substring(0, 4)
				);
			}

			//Formatar Cep
			if (key.Contains("CEP") && value.ToString().Length == 8)
			{
				value = value.ToString().Substring(0, 8);

				value = string.Format("{0}-{1}",
					value.ToString().Substring(0, 5),
					value.ToString().Substring(5, 3)
				);
			}

			//Formatar Dinheiro
			if (key.Contains("CAPITAL_SOCIAL"))
			{
				var capital = (decimal)value;
				value = string.Format("{0:C}", capital);
			}

			//Formatar Porcentagem
			if (key.Contains("valorParticipacao"))
			{
				var valor = 0M;
				decimal.TryParse(value.ToString(), out valor);

				if (valor.ToString().Length >= 3 && valor != 100)
				{
					if (valor.ToString().Length >= 4)
					{
						value = string.Format("{0},{1}%",
							value.ToString().Substring(0, 2),
							value.ToString().Substring(2, 2)
							);
					}
					else
					{
						value = string.Format("{0},{1}%",
							value.ToString().Substring(0, 2),
							value.ToString().Substring(2, 1)
							);
					}
				}
				else
				{
					value = string.Format("{0}%", value);
				}
			}

			return value;
		}

		public static byte[] ConverterJsonToByte(List<string> lista)
		{
			return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(lista));
		}

		public static byte[] ConverterJsonToByte(string value)
		{
			return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(value));
		}


		/// <summary>
		/// Retorna o valor do primeiro campo do json.
		/// </summary>
		/// <param name="json">json</param>
		/// <returns>Primeiro valor do primeiro campo ou string.empty se mais de um campo.</returns>
		public static string ExtrairPrimeiroValorPrimeiroCampo(string json)
		{
			if (string.IsNullOrWhiteSpace(json)) { return string.Empty; }

			JEnumerable<JToken> jsonObject;

			try
			{
				jsonObject = JObject.Parse(json).Children();
			}
			catch
			{
				logger.Info("Erro ao realizar o parse no json: " + json);
			}

			if (jsonObject.Any() && jsonObject.Count() == 1)
			{
				return jsonObject.First().Values().First().ToString();
			}
			else
			{
				//Returns a collection of the child tokens of this token, in document order. https://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_Linq_JToken_Children.html
				var obj = JsonConvert.DeserializeObject<JEnumerable<JToken>>(json);

				StringBuilder str = new StringBuilder();

				var regex = new Regex("[{}_\"]");

				foreach (var item in obj)
				{
					str.Append(item);
				}

				return regex.Replace(str.ToString(), " ");
			}
		}

		/// <summary>Retorna a diferença entre dois Json's</summary>
		/// <param name="sourceJson"></param>
		/// <param name="targetJson"></param>
		/// <returns></returns>
		public static List<string> ObterDiferencas(string sourceJson, string targetJson)
		{
			if (!sourceJson.Contains("_EM") && !sourceJson.Contains("CAPITAL_SOCIAL"))
			{
				sourceJson = CleanJson(sourceJson);
				targetJson = CleanJson(targetJson);
			}

			JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(sourceJson);
			JObject targetJObject = JsonConvert.DeserializeObject<JObject>(targetJson);

			var list = new List<string>();

			if (!JToken.DeepEquals(sourceJObject, targetJObject))
			{
				foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
				{
					var sourceValue = FormatPropertyValue(sourceProperty.Key, sourceProperty.Value);

					var targetProperty = targetJObject.Property(sourceProperty.Key);

					if (targetProperty != null)
					{
						var targetValue = FormatPropertyValue(sourceProperty.Key, targetProperty.Value);

						if (!JToken.DeepEquals(sourceValue, targetValue))
						{
							var item = string.Format("{0};{1};{2}", sourceValue, targetValue, sourceProperty.Key);
							list.Add(item);
						}
					}
					else
					{
						var item = string.Format("{0};{1};{2}", sourceValue, "", sourceProperty.Key);
						list.Add(item);
					}
				}
			}

			return list;
		}
	}
}