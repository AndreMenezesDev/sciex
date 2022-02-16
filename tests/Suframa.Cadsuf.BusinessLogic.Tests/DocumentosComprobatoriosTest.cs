using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
	[TestClass]
	public class DocumentosComprobatoriosTest
	{
		private readonly IDocumentosComprobatoriosBll _documentosComprobatorios;

		public DocumentosComprobatoriosTest()
		{
			CrossCutting.Mapping.Initialize.Init();
			CrossCutting.DependenceInjetion.Initialize.InitSingleton();

			_documentosComprobatorios = CrossCutting.DependenceInjetion.Initialize.Instance<DocumentosComprobatoriosBll>(typeof(DocumentosComprobatoriosBll));
		}

		[TestMethod]
		public void ListarVigentes()
		{
			var documentos = _documentosComprobatorios.ListarVigentes();
		}

		[TestMethod]
		public void SalvarDocumentosComprobatorios()
		{
			var documentos = new DocumentosComprobatoriosVM
			{
				IdRequerimento = 11,
				DocumentosComprobatorios = new List<DocumentoComprobatorioVM>
				{
					new DocumentoComprobatorioVM
					{
						IdRequerimento = 11,
						IdTipoDocumento = 1,
						NumeroCertidao = "111111",
						DataVencimento = DateTime.Now,
						DataExpedicao = DateTime.Now,
						Status = EnumStatus.Ativo,
						TipoOrigem = EnumTipoOrigemDocumento.Anexo,
						IdArquivo = 7
					}
				}
			};

			_documentosComprobatorios.Salvar(documentos);
		}
	}
}