using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class TipoEmbalagemController : ApiController
	{
		ITipoEmbalagemBll _tipoEmbalagem;
		public TipoEmbalagemController(ITipoEmbalagemBll tipoEmbalagem)
		{
			_tipoEmbalagem = tipoEmbalagem;
		}

		public TipoEmbalagemVM Get([FromUri]TipoEmbalagemVM obejto)
		{
			return _tipoEmbalagem.VerificaCodigoCadastrado(obejto);
		}

		public TipoEmbalagemVM Get(int id)
		{
			return _tipoEmbalagem.SelecionarEmbalagem(id);
		}

		public int Delete(int id)
		{
			return _tipoEmbalagem.Deletar(id);
		}

		public int Post([FromBody]TipoEmbalagemVM objeto)
		{
			return _tipoEmbalagem.Salvar(objeto);
		}

		public int Put([FromBody]TipoEmbalagemVM objeto)
		{
			return _tipoEmbalagem.AtualizarStatus(objeto);
		}

	}
}