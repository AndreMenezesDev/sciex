using Newtonsoft.Json;
using Suframa.Sciex.CrossCutting.SuperStructs;
using System;
using System.Globalization;

public static class FormatExtension
{
    public static string CepFormat(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { return null; }
        value = value.ExtractNumbers().PadLeft(8, '0');
        return !string.IsNullOrWhiteSpace(value) ? Convert.ToUInt64(value).ToString(@"00000\-000") : string.Empty;
    }

    public static string CepUnformat(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { return null; }
        return value.ExtractNumbers().PadLeft(8, '0');
    }

    public static string CnpjCpfFormat(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { return null; }
        var numbers = value.ExtractNumbers();
        return numbers.Length == 11 ? Cpf.Mask(numbers) : numbers.CnpjFormat();
    }

    public static string CnpjCpfUnformat(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { return null; }
        var numbers = value.ExtractNumbers();
        return numbers.Length == 11 ? Cpf.Unmask(numbers) : numbers.CnpjUnformat();
    }

    public static string CnpjFormat(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { return null; }
        value = value.ExtractNumbers();
        return !string.IsNullOrWhiteSpace(value) ? Convert.ToUInt64(value).ToString(@"00\.000\.000\/0000\-00") : string.Empty;
    }

	public static string CodigoNcmOcidentalFormat(this string value)
	{
		if (string.IsNullOrWhiteSpace(value)) { return null; }
		value = value.ExtractNumbers();
		return !string.IsNullOrWhiteSpace(value) ? Convert.ToUInt64(value).ToString(@"0000\.00\.00") : string.Empty;
	}

	public static string CnpjUnformat(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { return null; }
        return value.ExtractNumbers().PadLeft(14, '0');
    }

	public static string ConvertendoData(this DateTime value)
	{
		string ano = value.Year.ToString();
		string mes = value.Month.ToString();
		string dia = value.Day.ToString();

		string data = dia + "/" + mes + "/" + ano;
		return data;
	}

	public static string RamalUnformat(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { return null; }
        return string.Join(null, System.Text.RegularExpressions.Regex.Split(value, "[^\\d]"));
    }

    public static string TelefoneFormat(this decimal? value)
    {
        if (value == null) { return null; }
        return value.HasValue ? Convert.ToUInt64(value).ToString(@"\(00\)\ \0000\-\0000") : string.Empty;
    }

	public static string ToJson(this object value)
    {
        if (value == null) return "{ null }";
        try
        {
            return JsonConvert.SerializeObject(value);
        }
        catch { return "Erro ao serializar entidade"; }
    }

	public static string Slice(this string s,
		int beginIndex, int endIndex)
	{
		if (s == null) return null;

		var b = "";
		if (beginIndex < 0)
			b = s.Substring(0, Math.Abs(beginIndex));
		else
			b = s.Substring(s.Length - beginIndex );

		//var a = beginIndex >= 0 ?
		//	s.Substring(beginIndex, endIndex) :
		//	s.Substring(s.Length - beginIndex);

		return b;
	}

	public static string Slice(this string s,
		int beginIndex = 0)
	{
		if (s == null) return null;

		return Slice(s, beginIndex, s.Length);
	}

	public static string FormatDecimalOrZero(decimal? value)
	{
		if (value == null || value == 0)
		{
			return string.Format("{0:0,000.0000000}", 0);
		}
		else
		{
			var a = string.Format("{0:0,0.00000000}", value)
								.TrimStart(new Char[] { '0' }).TrimStart(new Char[] { '.' });
			return a;
		}
	}

}