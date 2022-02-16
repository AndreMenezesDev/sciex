using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.Security;
using System;
using System.Web.Http.Filters;

namespace Suframa.Sciex.ApplicationService
{
    /// <summary>
    /// CustomHeaderFilter
    /// </summary>
    public class CustomHeaderFilter : ActionFilterAttribute
    {
        private UsuarioLogado _usuarioLogado;
		// aqui
        /// <summary>
        /// OnActionExecuted
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null && actionExecutedContext.Response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                this._usuarioLogado = CrossCutting.DependenceInjetion.Initialize.Instance<UsuarioLogado>(typeof(UsuarioLogado));
				//this._usuarioLogado.Load();

                
            }
        }
    }
}