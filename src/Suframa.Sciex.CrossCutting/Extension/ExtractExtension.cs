public static class StringExtension
{
	public static string ExtractNumbers(this string value)
	{
		if (string.IsNullOrEmpty(value))
			return string.Empty;
		else
			return string.Join(null, System.Text.RegularExpressions.Regex.Split(value, "[^\\d]"));
	}

	public static string Truncate(this string value, int length)
	{
		if (string.IsNullOrEmpty(value))
			return value;
		else
			return value.Length > length ? value.Substring(0, length) : value;
	}
}