namespace Suframa.Sciex.CrossCutting.Configuration
{
    public class PublicSettings
    {
        public bool BYPASS_CAPTCHA = AppSettings.Get<bool>(nameof(BYPASS_CAPTCHA));
        public string CAPTCHA_SITE_KEY = AppSettings.Get<string>(nameof(CAPTCHA_SITE_KEY));
        public int ID_MUNICIPIO_UNIDADE_CADASTRADORA = AppSettings.Get<int>(nameof(ID_MUNICIPIO_UNIDADE_CADASTRADORA));
		public string URL_BACKEND = AppSettings.Get<string>(nameof(URL_BACKEND));
		public string URL_FRONTEND = AppSettings.Get<string>(nameof(URL_FRONTEND));
		public string URL_CAS = AppSettings.Get<string>(nameof(URL_CAS));
		public string URL_BASE_PSS = AppSettings.Get<string>(nameof(URL_BASE_PSS));
		public string URL_MENU = AppSettings.Get<string>(nameof(URL_MENU));
		public string UrlAnalistaDesignado = AppSettings.Get<string>(nameof(UrlAnalistaDesignado));
	}
}