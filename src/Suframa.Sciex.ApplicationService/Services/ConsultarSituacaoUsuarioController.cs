using System.Web.Http;
using Suframa.Sciex.BusinessLogic;

namespace Suframa.Sciex.ApplicationService.Grid
{
	/// <summary>InscricaoCadastralCredenciamentoCancelamentoController</summary>
	public class ConsultarSituacaoUsuarioController : ApiController
	{
		private readonly IUsuarioBll _usuarioBll;

		/// <summary>
		/// Construtor consulta situação usuário
		/// </summary>
		/// <param name="usuarioBll"></param>
		public ConsultarSituacaoUsuarioController(IUsuarioBll usuarioBll)
		{
			_usuarioBll = usuarioBll;
		}

		/// <summary>
		/// Consultar se o usuario está bloqueado
		/// </summary>
		/// <param name="cpfCnpj"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public bool Get(string cpfCnpj)
		{
			// Variável criada para fazer bypass ao debugar essa action, pois acessa o legado.
			var retorno = _usuarioBll.ConsultarSituacaoUsuarioComValidacao(cpfCnpj);
			return retorno;
		}
	}
}