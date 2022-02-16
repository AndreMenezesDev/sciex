using System;
using System.IO;
using System.IO.Compression;

namespace Suframa.Sciex.CrossCutting.Compressor
{
	public class Compressor
	{
		public string MensagemErro { get; set; }

		public bool UnZIP(byte[] arquivo, string caminho)
		{
			try
			{
				string path = caminho;
				Stream stream = new MemoryStream(arquivo);
				ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read);

				archive.ExtractToDirectory(path);
				
				return true;
			}
			catch (Exception ex)
			{
				MensagemErro = ex.Message.ToString();
				return false;
			}									
		}
	}

}
