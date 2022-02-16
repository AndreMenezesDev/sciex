using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class AliArquivoBll : IAliArquivoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;		
		private readonly IUsuarioLogado _usuarioLogado;			

		public AliArquivoBll(IUnitOfWorkSciex uowSciex,
			IUsuarioLogado usuarioLogadoBll)
		{
			_uowSciex = uowSciex;
			_usuarioLogado = usuarioLogadoBll;				
		}

		public IEnumerable<AliArquivoVM> Listar(AliArquivoVM AliArquivoVM)
		{
			var AliArquivo = _uowSciex.QueryStackSciex.AliArquivo.Listar<AliArquivoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<AliArquivoVM>>(AliArquivo);
		}
			
		public AliArquivoVM RegrasSalvar(AliArquivoVM AliArquivoVM)
		{
			var entityAliArquivoEnvio = AutoMapper.Mapper.Map<AliArquivoEntity>(AliArquivoVM);
			_uowSciex.CommandStackSciex.AliArquivo.Salvar(entityAliArquivoEnvio);
			_uowSciex.CommandStackSciex.Save();

			var _AliArquivoVM = AutoMapper.Mapper.Map<AliArquivoVM>(entityAliArquivoEnvio);
			return _AliArquivoVM;
		}		

		public AliArquivoVM Selecionar(long? idAliArquivo)
		{
			var AliArquivoVM = new AliArquivoVM();
			if (!idAliArquivo.HasValue) { return AliArquivoVM; }

			var AliArquivo = _uowSciex.QueryStackSciex.AliArquivo.Selecionar(x => x.IdAliArquivoEnvio == idAliArquivo);
			if (AliArquivo == null) { return AliArquivoVM; }

			AliArquivoVM = AutoMapper.Mapper.Map<AliArquivoVM>(AliArquivo);
			return AliArquivoVM;
		}

		public void Deletar(long id)
		{
			var AliArquivo = _uowSciex.QueryStackSciex.AliArquivo.Selecionar(s => s.IdAliArquivoEnvio == id);
			if (AliArquivo != null)
			{
				_uowSciex.CommandStackSciex.AliArquivo.Apagar(AliArquivo.IdAliArquivoEnvio);
			}
		}

		//Download file
		public HttpResponseMessage GetAliArquivo(long idAliArquivo)
		{
			var AliArquivo = _uowSciex.QueryStackSciex.AliArquivo.Selecionar(o => o.IdAliArquivoEnvio == idAliArquivo);

			HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

			response.Content = new StreamContent(new MemoryStream(AliArquivo.Arquivo));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
			response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
			response.Content.Headers.ContentDisposition.FileName = "FileName";
			return response;
		}

	}
}
