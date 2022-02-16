using FluentValidation;
using NLog;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.BusinessLogic
{
	public class ArquivoBll : IArquivoBll
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public ArquivoBll(IUnitOfWork uow, IValidation validation)
		{
			_uow = uow;
			_validation = new Validation();
		}

		public void Apagar(int id)
		{
			_uow.CommandStack.Arquivo.Apagar(id);
			_uow.CommandStack.Save();
		}

		public ArquivoEntity RegrasSalvar(ArquivoEntity arquivoEntity)
		{
			_uow.CommandStack.Arquivo.Salvar(arquivoEntity);

			return arquivoEntity;
		}

		public ArquivoVM Salvar(ArquivoVM arquivoVM)
		{
			if (arquivoVM.Tamanho > 20 * 1024 * 1024)
			{
				var arquivoDto = new ArquivoDto { Tamanho = arquivoVM.Tamanho };
				_validation._arquivoSalvarValidation.ValidateAndThrow(arquivoDto);
			}
			try
			{
				logger.Info("Iniciando rotina de gravação de arquivo");
				var arquivoEntity = AutoMapper.Mapper.Map<ArquivoEntity>(arquivoVM);

				logger.Info("Iniciando regra de gravação de arquivo");
				RegrasSalvar(arquivoEntity);

				logger.Info("Iniciando persistencia de gravação de arquivo");
				_uow.CommandStack.Save();

				logger.Info("Iniciando mapamento reverso");
				arquivoVM = AutoMapper.Mapper.Map<ArquivoVM>(arquivoEntity);
			}
			catch (Exception ex)
			{
				logger.Debug(ex.ToString());
				logger.Error(ex, "Erro ao persistir arquivo");
			}

			return arquivoVM;
		}

		public ArquivoVM Selecionar(int idArquivo)
		{
			return _uow.QueryStack.Arquivo.Selecionar<ArquivoVM>(x => x.IdArquivo == idArquivo);
		}
	}
}