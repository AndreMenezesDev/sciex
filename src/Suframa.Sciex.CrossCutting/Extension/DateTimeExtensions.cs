using System;

public static class DateTimeExtensions
{
	public static DateTime SetTime(this DateTime datetime, string time)
	{
		if (datetime == null || string.IsNullOrWhiteSpace(time)) { return datetime; }

		var parts = time.Split(':');

		if (parts.Length < 3) { return datetime; }

		var hours = 0;
		if (!int.TryParse(parts[0], out hours)) { return datetime; }

		var minutes = 0;
		if (!int.TryParse(parts[1], out minutes)) { return datetime; }

		var seconds = 0;
		if (!int.TryParse(parts[2], out seconds)) { return datetime; }

		var timeSpan = new TimeSpan(hours, minutes, seconds);

		return datetime.Date + timeSpan;
	}

	public static int RetornarNumeroMes(string nomeAno)
	{
		var retorno = "";

		switch (nomeAno.ToUpper())
		{
			case "JAN": retorno = "01"; break;
			case "FEV": retorno = "02"; break;
			case "MAR": retorno = "03"; break;
			case "ABR": retorno = "04"; break;
			case "MAI": retorno = "05"; break;
			case "JUN": retorno = "06"; break;
			case "JUL": retorno = "07"; break;
			case "AGO": retorno = "08"; break;
			case "SET": retorno = "09"; break;
			case "OUT": retorno = "10"; break;
			case "NOV": retorno = "11"; break;
			case "DEZ": retorno = "12"; break;
			default: retorno = "00"; break;
		}

		return int.Parse(retorno);
	}
}