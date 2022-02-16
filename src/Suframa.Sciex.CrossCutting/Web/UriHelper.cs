namespace Suframa.Sciex.CrossCutting.Web
{
    public static class UriHelper
    {
        public static string Slashfy(string url)
        {
            return url.EndsWith("/") ? url : url + "/";
        }
    }
}