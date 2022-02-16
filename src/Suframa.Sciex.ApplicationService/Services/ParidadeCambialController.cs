using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service paridade cambial</summary>
	public class ParidadeCambialController : ApiController
	{
		private readonly IParidadeCambialBll _paridadeCambialBll;

		/// <summary>Paridade Cambial injetar regras de negócio</summary>
		/// <param name="paridadeCambialBll"></param>
		public ParidadeCambialController(IParidadeCambialBll paridadeCambialBll)
		{
			_paridadeCambialBll = paridadeCambialBll;
		}

		/// <summary>Baixar/Copiar Paridade Cambial</summary>
		/// <param name="paridadeCambialGenerator">Objeto natureza jurídica a ser persistido</param>
		/// <returns></returns>
		[AllowAnonymous]
		public ParidadeCambialGenerator Put(ParidadeCambialGenerator paridadeCambialGenerator)
		{
			if (paridadeCambialGenerator.TipoGeracao != 3)
				return _paridadeCambialBll.Gerar(paridadeCambialGenerator);
			else
				return _paridadeCambialBll.BaixarArquivoParidadeRemoto(paridadeCambialGenerator);

		}
	}
}