using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.Ping;
using Suframa.Sciex.CrossCutting.Reflection;
using System.Web.Http;
using System.Configuration;

namespace Suframa.Sciex.ApplicationService.Information
{
	/// <summary>
	/// Documentação:
	/// http://bitoftech.net/2014/08/25/asp-net-web-api-documentation-using-swagger/
	/// https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio
	/// </summary>
	public class InformacaoController : ApiController
	{
		// GET: api/Information
		/// <summary>Obter informações gerais do sistema</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public dynamic Get()
		{
			var result = new
			{
				versao = AssemblyHelper.GetCallingAssemblyVersion(),
				appSettings = new PublicSettings(),
				connectionSciexString = ConfigurationManager.ConnectionStrings["DatabaseContextSciex"].ConnectionString,
				connectionString = ConfigurationManager.ConnectionStrings["DatabaseContext"].ConnectionString
				//statusConexaoSAC_80 = PingCheck.CheckPort("10.75.34.46", 80) ? "Porta aberta" : "Porta fechada",
				//statusPortaSAC_8080 = PingCheck.CheckPort("10.75.34.46", 8080) ? "Porta aberta" : "Porta fechada"
			};

			return result;
		}
	}
}