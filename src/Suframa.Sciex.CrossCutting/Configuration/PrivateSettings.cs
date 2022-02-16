using System;

namespace Suframa.Sciex.CrossCutting.Configuration
{
	public static class PrivateSettings
	{
		public static bool BYPASS_AUTHENTICATION = AppSettings.Get<bool>(nameof(BYPASS_AUTHENTICATION));
		public static bool DEVELOPMENT_MODE = AppSettings.Get<bool>(nameof(DEVELOPMENT_MODE));
		public static string CAPTCHA_SECRET_KEY = AppSettings.Get<string>(nameof(CAPTCHA_SECRET_KEY));
		public static string CAPTCHA_URL = AppSettings.Get<string>(nameof(CAPTCHA_URL));
		public static string FROM_EMAIL = AppSettings.Get<string>(nameof(FROM_EMAIL));
		public static string HEADERS_CORS = AppSettings.Get<string>(nameof(HEADERS_CORS));
		public static string HOST_EMAIL = AppSettings.Get<string>(nameof(HOST_EMAIL));
		public static int ID_DECRETO_NOME_SOCIAL = AppSettings.Get<int>(nameof(ID_DECRETO_NOME_SOCIAL));
		public static string IP_LDAP = AppSettings.Get<string>(nameof(IP_LDAP));
		public static string MAIL_CREDENTIALS_EMAIL = AppSettings.Get<string>(nameof(MAIL_CREDENTIALS_EMAIL));
		public static string METHODS_CORS = AppSettings.Get<string>(nameof(METHODS_CORS));
		public static int MINUTES_EXPIRE_TOKEN = AppSettings.Get<int>(nameof(MINUTES_EXPIRE_TOKEN));
		public static string ORIGINS_CORS = AppSettings.Get<string>(nameof(ORIGINS_CORS));
		public static string PASS_ARRECADACAO = AppSettings.Get<string>(nameof(PASS_ARRECADACAO));
		public static string PASS_CREDENTIALS_EMAIL = AppSettings.Get<string>(nameof(PASS_CREDENTIALS_EMAIL));
		public static int PORT_EMAIL = AppSettings.Get<int>(nameof(PORT_EMAIL));
		public static bool SSL_HOST_EMAIL = AppSettings.Get<bool>(nameof(SSL_HOST_EMAIL));
		public static string TOKEN_SECRET = AppSettings.Get<string>(nameof(TOKEN_SECRET));
		public static string URL_ARRECADACAO = AppSettings.Get<string>(nameof(URL_ARRECADACAO));
		public static string URL_AUTENTICACAO = AppSettings.Get<string>(nameof(URL_AUTENTICACAO));
		public static string URL_CADASTRO = AppSettings.Get<string>(nameof(URL_CADASTRO));
		public static string URL_INTEGRACAO_LEGADO = AppSettings.Get<string>(nameof(URL_INTEGRACAO_LEGADO));
		public static string USER_ARRECADACAO = AppSettings.Get<string>(nameof(USER_ARRECADACAO));
		public static string USER_LDAP = AppSettings.Get<string>(nameof(USER_LDAP));
	}
}