using FluentValidation;
using Suframa.Sciex.CrossCutting.Exceptions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace Suframa.Sciex.ApplicationService.Filters

{
    ///<Summary>
    /// Classe para validação de Exception
    ///</Summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private static void ValidationException(HttpActionExecutedContext context)
        {

			if (context.Exception is StackOverflowException)
			{
				if (context.Exception.Message.ToString() != "0")
				{
					context.Response = context.Request.CreateResponse(HttpStatusCode.Conflict, context.Exception.Message.ToString());
				}
				else
				{
					context.Response = context.Request.CreateResponse(HttpStatusCode.MethodNotAllowed, context.Exception.Message.ToString());
				}
			}
			else
            if (context.Exception is ValidationException)
            {
				var message = new StringBuilder();

				var validationException = (context.Exception as ValidationException);

                if (validationException.Errors != null && validationException.Errors.Any())
                {
                    foreach (var item in (context.Exception as ValidationException).Errors)
                    {
                        message.Append(item.ErrorMessage);
                    }
                }
                else
                {
                    message.Append(validationException.Message);
                }

                context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, message.ToString());
            }
            else if (context.Exception is RedirectValidationException)
            {
                var redirectValidationException = (context.Exception as RedirectValidationException);
                context.Response = context.Request.CreateResponse(HttpStatusCode.Redirect, new { message = redirectValidationException.Message, url = redirectValidationException.Url });
            }
			else if (context.Exception.Message.Contains("Sua sessão expirou. Faça login novamente no sistema."))
			{
				context.Response = context.Request.CreateResponse(HttpStatusCode.BadGateway, context.Exception.Message);
			}
            else
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, string.Format("Message: {0} StackTrace: {1}", context.Exception.Message, context.Exception.StackTrace));
            }
        }

        ///<Summary>
        /// Gets the answer
        ///</Summary>
        ///<param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            ValidationException(context);
        }
    }
}