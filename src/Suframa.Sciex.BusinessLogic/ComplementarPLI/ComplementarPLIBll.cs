﻿using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Suframa.Sciex.BusinessLogic
{
	public class ComplementarPLIBll : IComplementarPLIBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private string complemento;
		//private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioPssBll _usuarioPssBll;



		public ComplementarPLIBll(IUnitOfWorkSciex uowSciex, IUsuarioLogado usuarioLogado, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
			//_IUsuarioLogado = usuarioLogado;
			_usuarioPssBll = usuarioPssBll;
		}


		public string ComplementarPLI(string idPli)
		{
			string retorno = "";

			if (idPli != "")
			{
				complemento = " AND PLI_ID = " + idPli;
			}
			else
			{
				complemento = "";
			}

			retorno = _uowSciex.CommandStackSciex.Salvar(sqlComplementar(), "");


			return retorno;
		}

		public string sqlComplementar()
		{
			string SQL = @"	
					
				DECLARE @PCA_ID BIGINT
				DECLARE @PVA_VALOR DECIMAL
				DECLARE @MOE_ID INT
				DECLARE @OBS VARCHAR(5000)
				DECLARE @PME_ID BIGINT
				DECLARE @PDM_VL_CONDICAO NUMERIC(18,7)
				DECLARE @PDM_VL_UNITARIO_CONDICAO NUMERIC(18,7)
				DECLARE @PDM_VL_UNITARIO_CONDICAO_DOLAR NUMERIC(18,7)
				DECLARE @PDM_VL_CONDICAO_VENDA_REAL  NUMERIC(18,7)
				DECLARE @PDM_VL_CONDICAO_VENDA_DOLAR  NUMERIC(18,7)
				DECLARE @PDM_VL_PARIDADE_DOLAR  DECIMAL(15,8)
				DECLARE @PDM_VL_PARIDADE DECIMAL(15,8)
				DECLARE @PDM_ID BIGINT 
				DECLARE @DATA_ENVIO_PLI VARCHAR(10)
				DECLARE @COBERTURACAMBIAL INT
				DECLARE @CPF_CNPJ VARCHAR(14)
				DECLARE @PME_CODIGO_PRODUTO SMALLINT
				DECLARE @PEM_CO_UTILIZACAO INT
				DECLARE @MOE_CO SMALLINT 
				DECLARE @PDM_VL_TOTAL_MERC_COND_VENDA NUMERIC(15,2)
				DECLARE @PDM_VL_TOTAL_MERC_VENDA_REAL  NUMERIC(15,2)
				DECLARE @PDM_VL_TOTAL_MERC_VENDA_DOLAR  NUMERIC(15,2)
				DECLARE @PLI_VL_TOTAL_PLI_COND_VENDA  NUMERIC(17,2)
				DECLARE @PLI_VL_TOTAL_PLI_VENDA_REAL  NUMERIC(17,2)
				DECLARE @PLI_VL_TOTAL_PLI_VENDA_DOLAR  NUMERIC(17,2)
				DECLARE @PLI_PROCESSADO BIT
				DECLARE @PDM_VL_CONDICAO_VENDA NUMERIC(18,7)
				DECLARE @PLI_ID BIGINT
				DECLARE @PLI_TP_DOCUMENTO INT
				DECLARE @MOE_CO_DOLAR INT
				DECLARE @IDFUNDAMENTOLEGAL INT
				DECLARE @TIPOAREABENEFICIO SMALLINT
				DECLARE @CODIGOCONTA INT
				DECLARE @PLIAPLICACAO INT
				DECLARE @CODIGOPLIAPLICACAO INT
				DECLARE @CODIGOUTILIZACAO INT 
				DECLARE @CODIGOCONTROLEEXECUCAO INT
				DECLARE @ERROCALCULO VARCHAR(1000)
				DECLARE @NOMEIMPORTADOR VARCHAR(100)
				DECLARE @PLI_NUM_CPFCNPJ_RESPONSAVEL VARCHAR(14)
				DECLARE @PLI_RESPONSAVEL_CADASTRO VARCHAR(100)
				DECLARE @SPL_ST_PLI_TECNOLOGIA_ASSISTIVA TINYINT
				

				CREATE TABLE #TPPARIDADECAMBIAL
				(
					PCA_ID INT,	
					PCA_DT_PARIDADE DATETIME, 
					PCA_DH_CADASTRO DATETIME, 
					PCA_DT_ARQUIVO DATETIME, 
					PCA_NU_USUARIO VARCHAR(14), 
					PCA_NO_USUARIO VARCHAR(120)
				)

				CREATE TABLE #TPPARIDADEVALOR
				(
					PVA_ID INT, 
					PCA_ID BIGINT, 
					MOE_ID INT, 
					PVA_VL_PARIDADE DECIMAL(15,8)
				)


				CREATE TABLE #TPPLIMERCADORIA
				(
					PME_ID BIGINT, 
					PLI_ID BIGINT, 
					MOE_ID INT,
					FLE_ID INT, 
					PME_VL_TOTAL_CONDICAO_VENDA NUMERIC(15,2), 
					PME_TP_COBCAMBIAL INT,
					PME_CO_PRODUTO SMALLINT,
					PME_VL_TOTAL_CONDICAO_VENDA_REAL NUMERIC(15,2), 
					PME_VL_TOTAL_CONDICAO_VENDA_DOLAR NUMERIC(15,2), 
					SITUACAO INT
				)
	
				CREATE TABLE #TPPLI
				( 
					PLI_ID BIGINT,
					PLI_ST_PLI TINYINT,
					PAP_ID INT,
					PLI_NU_CNPJ VARCHAR(14),
					PLI_ST_ANALISE_VISUAL SMALLINT,
					PLI_DH_ENVIO DATE,
					PLI_VL_TOTAL_CONDICAO_VENDA NUMERIC(17,2),
					PLI_VL_TOTAL_CONDICAO_VENDA_REAL NUMERIC(17,2),
					PLI_VL_TOTAL_CONDICAO_VENDA_DOLAR NUMERIC(17,2),
					IMP_DS_RAZAO_SOCIAL VARCHAR(100),
					PLI_NU_RESPONSAVEL_CADASTRO VARCHAR(14),
					PLI_NO_RESPONSAVEL_CADASTRO VARCHAR(100),
					SITUACAO INT,
					SPL_ST_PLI_TECNOLOGIA_ASSISTIVA TINYINT
				)

				CREATE TABLE #TPDETALHEMERCADORIA
				(
					PDM_ID BIGINT, 
					PME_ID BIGINT, 
					PDM_VL_UNITARIO_COND_VENDA NUMERIC(18,7),
					PDM_VL_UNITARIO_COND_VENDA_DOLAR NUMERIC(18,7),
					PDM_VL_CONDICAO_VENDA NUMERIC(18,7),
					PDM_VL_CONDICAO_VENDA_REAL  NUMERIC(18,7),
					PDM_VL_CONDICAO_VENDA_DOLAR  NUMERIC(18,7),
					SITUACAO INT
				)


				BEGIN TRY  
					BEGIN TRANSACTION; 

					SET @PDM_VL_CONDICAO = 0
					SET @PDM_VL_CONDICAO_VENDA_REAL = 0
					SET @PDM_VL_CONDICAO_VENDA_DOLAR = 0
					SET @PDM_VL_TOTAL_MERC_VENDA_REAL = 0
					SET @PDM_VL_TOTAL_MERC_VENDA_DOLAR = 0
					SET @PLI_VL_TOTAL_PLI_VENDA_REAL = 0
					SET @PLI_VL_TOTAL_PLI_VENDA_DOLAR = 0
					SET @MOE_CO_DOLAR = 220
					SET @PDM_VL_TOTAL_MERC_COND_VENDA = 0
					SET @PLI_VL_TOTAL_PLI_COND_VENDA = 0


					-- INSERE NA TABELA TEMPORÁRIA SOMENTE O PLI COM O STATUS 21 (ENTREGUE) -- RN05
					INSERT INTO #TPPLI
					SELECT 
						PLI_ID,
						PLI_ST_PLI,
						PAP_ID,
						PLI_NU_CNPJ,
						PLI_ST_ANALISE_VISUAL,
						PLI_DH_ENVIO,
						PLI_VL_TOTAL_CONDICAO_VENDA,
						PLI_VL_TOTAL_CONDICAO_VENDA_REAL,
						PLI_VL_TOTAL_CONDICAO_VENDA_DOLAR,
						IMP_DS_RAZAO_SOCIAL,
						PLI_NU_RESPONSAVEL_CADASTRO,
						PLI_NO_RESPONSAVEL_CADASTRO,
						1,
						SPL_ST_PLI_TECNOLOGIA_ASSISTIVA

					FROM SCIEX_PLI
					WHERE PLI_ST_PLI = 21 
		
					-- VARRE A TABELA TEMPORÁRIA PLI TODA
					WHILE EXISTS(SELECT * FROM #TPPLI WHERE SITUACAO = 1)
					BEGIN 
						-- INICIA O LOOP DO PLI
		
						-- BUSCO PELO: PLI_ID, DATA_ENVIO_PLI, PLIAPLICACAO E CPF E CNPJ
						SELECT TOP 1 
							@PLI_ID = PLI_ID, 
							@DATA_ENVIO_PLI = CONVERT(DATE, PLI_DH_ENVIO, 103), 
							@PLIAPLICACAO = PAP_ID,
							@CPF_CNPJ = PLI_NU_CNPJ,
							@NOMEIMPORTADOR = IMP_DS_RAZAO_SOCIAL,
							@PLI_NUM_CPFCNPJ_RESPONSAVEL = PLI_NU_RESPONSAVEL_CADASTRO,
							@PLI_RESPONSAVEL_CADASTRO = PLI_NO_RESPONSAVEL_CADASTRO,
							@SPL_ST_PLI_TECNOLOGIA_ASSISTIVA = SPL_ST_PLI_TECNOLOGIA_ASSISTIVA

						FROM #TPPLI WHERE SITUACAO = 1 -- BUSCA O ID DO PLI

						INSERT INTO SCIEX_CONTROLE_EXEC_SERVICO
						(CES_DH_EXECUCAO_INICIO, CES_ST_EXECUCAO, CES_NU_CPF_CNPJ_INTERESSADO, CES_NO_CPF_CNPJ_INTERESSADO, LSE_ID, CES_ME_OBJETO_ENVIO)
						VALUES
						(CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), 0, @CPF_CNPJ, @NOMEIMPORTADOR, 7, 'PLI(ID) '+CAST(@PLI_ID AS VARCHAR))

						SET @CODIGOCONTROLEEXECUCAO = @@IDENTITY;
									
						SET @PLI_PROCESSADO = 1

						INSERT INTO #TPPARIDADECAMBIAL 
						SELECT 
							PCA_ID, 
							PCA_DT_PARIDADE, 
							PCA_DH_CADASTRO, 
							PCA_DT_ARQUIVO, 
							PCA_NU_USUARIO, 
							PCA_NO_USUARIO
						FROM SCIEX_PARIDADE_CAMBIAL
						WHERE CONVERT(DATE,PCA_DT_PARIDADE,103) = @DATA_ENVIO_PLI
			
						SELECT TOP 1 @PCA_ID = PCA_ID FROM #TPPARIDADECAMBIAL

						IF (ISNULL(@PCA_ID,0) = 0)
						BEGIN
						   SET @PLI_PROCESSADO = 0

						   SET @OBS = 'PLI_ID = '+ CAST(@PLI_ID AS VARCHAR) + ' COMPLEMENTADO SEM SUCESSO (SEM PARIDADE CAMBIAL CADASTRADA)';

						END
						ELSE
						BEGIN 

								SELECT @CODIGOPLIAPLICACAO = PAP_CO
								FROM SCIEX_PLI_APLICACAO WHERE PAP_ID = @PLIAPLICACAO

								SELECT @PLI_TP_DOCUMENTO = PLI_TP_DOCUMENTO
								FROM SCIEX_PLI WHERE PLI_ID = @PLI_ID

								-- REGRA RN03 - ANÁLISE VISUAL
								IF @PLI_TP_DOCUMENTO = 3
								BEGIN
									UPDATE SCIEX_PLI SET 
									PLI_ST_ANALISE_VISUAL = 1 WHERE PLI_ID = @PLI_ID
								END
								ELSE IF @CODIGOPLIAPLICACAO = 2 OR @CODIGOPLIAPLICACAO = 3
								BEGIN
									UPDATE SCIEX_PLI SET 
									PLI_ST_ANALISE_VISUAL = 1 WHERE PLI_ID = @PLI_ID
								END
								ELSE
								BEGIN

								-- REGRA NOVA
									IF @SPL_ST_PLI_TECNOLOGIA_ASSISTIVA = 1
									BEGIN
										UPDATE SCIEX_PLI SET 
										PLI_ST_ANALISE_VISUAL = 1 WHERE PLI_ID = @PLI_ID	
									END
									ELSE
									BEGIN
										UPDATE SCIEX_PLI SET 
										PLI_ST_ANALISE_VISUAL = 0 WHERE PLI_ID = @PLI_ID
									END	

								END -- FIM REGRA RN03


							INSERT INTO #TPPARIDADEVALOR 
							SELECT 
								A.PVA_ID, 
								A.PCA_ID, 
								A.MOE_ID, 
								A.PVA_VL_PARIDADE
							FROM SCIEX_PARIDADE_VALOR A			
							WHERE PCA_ID = @PCA_ID
				
							INSERT INTO #TPPLIMERCADORIA
							SELECT 
								PME_ID,
								PLI_ID,
								MOE_ID,
								FLE_ID,
								PME_VL_TOTAL_CONDICAO_VENDA,
								PME_TP_COBCAMBIAL,
								PME_CO_PRODUTO,
								PME_VL_TOTAL_CONDICAO_VENDA_REAL,
								PME_VL_TOTAL_CONDICAO_VENDA_DOLAR,
								1
							FROM SCIEX_PLI_MERCADORIA
							WHERE PLI_ID = @PLI_ID
				

							WHILE EXISTS(SELECT * FROM #TPPLIMERCADORIA WHERE SITUACAO = 1)
							BEGIN -- INICIO DO LOOP DA MERCADORIA

								SELECT 
									TOP 1 
									@MOE_ID = MOE_ID, 
									@PME_ID = PME_ID,
									@PME_CODIGO_PRODUTO = PME_CO_PRODUTO,
									@IDFUNDAMENTOLEGAL = FLE_ID,
									@COBERTURACAMBIAL = PME_TP_COBCAMBIAL
								FROM #TPPLIMERCADORIA WHERE SITUACAO = 1
					

								-- REGRA RN01
								SELECT 
									@TIPOAREABENEFICIO = FLE_TP_AREA_BENEFICIO 
								FROM SCIEX_FUNDAMENTO_LEGAL 
								WHERE FLE_ID = @IDFUNDAMENTOLEGAL
							

								IF @TIPOAREABENEFICIO = 3 -- AMAZÔNIA OCIDENTAL
								BEGIN
									SET @CODIGOCONTA = 12 -- DECRETO LEI 356/68
								END
								ELSE
								BEGIN
									SET @CODIGOCONTA = 24  -- COMÉRCIO
								END
			
								IF @CODIGOPLIAPLICACAO = 1 --INDUSTRIALIZAÇÃO
								BEGIN
									SET @CODIGOCONTA = 11 -- QUOTA INDIVIDUAL DE INSUMOS
								END

								IF @CODIGOPLIAPLICACAO = 6 --EXPORTAÇÃO
								BEGIN
									SET @CODIGOCONTA = 90  -- PEXPAM
								END

								IF @COBERTURACAMBIAL = 4 -- SEM COBERTURA 
								BEGIN
									SET @CODIGOCONTA = 20 -- SEM COBERTURA CAMBIAL
								END

								IF @CODIGOPLIAPLICACAO = 0 
								OR @CODIGOPLIAPLICACAO = 1 
								OR @CODIGOPLIAPLICACAO = 6 
								OR @COBERTURACAMBIAL = 4

								BEGIN
									UPDATE SCIEX_PLI_MERCADORIA SET
										CCO_ID = (SELECT CCO_ID FROM SCIEX_CODIGO_CONTA WHERE CCO_CO = @CODIGOCONTA)
									WHERE PME_ID = @PME_ID						
								END -- FIM DA REGRA RN03

								-- REGRA RN02
								-- CODIGO DE UTILIZAÇÃO
								IF @CODIGOPLIAPLICACAO = 0 -- COMERCIALIZAÇÃO
								BEGIN
									SET @CODIGOUTILIZACAO = 0 --O COMERCIALIZAÇÃO
								END

								IF @CODIGOPLIAPLICACAO = 1  -- INDUSTRIALIZAÇÃO
								BEGIN
									SELECT @PEM_CO_UTILIZACAO = PEM_CO_UTILIZACAO FROM VW_PRJ_PRODUTO_EMPRESA 
									WHERE PEM_NU_CNPJ = @CPF_CNPJ AND PEM_CO_PRODUTO = @PME_CODIGO_PRODUTO
						
								END

								IF @PEM_CO_UTILIZACAO = 1
								BEGIN
									SET @CODIGOUTILIZACAO = 1 --INSUMOS
								END

								IF @PEM_CO_UTILIZACAO = 3
								BEGIN
									SET @CODIGOUTILIZACAO = 3 -- BENS DE INFORMÁTCA
								END

								IF @PEM_CO_UTILIZACAO = 5
								BEGIN
									SET @CODIGOUTILIZACAO = 5 -- COMPONENTES
								END

								-- TOPICO 3

								IF @CODIGOPLIAPLICACAO = 6  -- EXPORTAÇÃO
								BEGIN
									SET @CODIGOUTILIZACAO = 1 -- INSUMOS
								END

								IF @CODIGOUTILIZACAO = 0
								OR @CODIGOUTILIZACAO = 1
								OR @CODIGOUTILIZACAO = 6
								OR @CODIGOUTILIZACAO = 3
								OR @CODIGOUTILIZACAO = 5

								BEGIN
									UPDATE SCIEX_PLI_MERCADORIA SET 
									CUT_ID = ( SELECT CUT_ID FROM SCIEX_CODIGO_UTILIZACAO WHERE CUT_CO = @CODIGOUTILIZACAO)
				
								END -- FIM

								--DESCOBRIR O CÓDIGO DA MOEDA
								-- RN04
								SELECT TOP 1 @MOE_CO = 	MOE_CO 
								FROM SCIEX_MOEDA	
								WHERE MOE_ID = @MOE_ID				

								IF(EXISTS (SELECT PVA_VL_PARIDADE  FROM #TPPARIDADEVALOR WHERE MOE_ID = @MOE_ID))
								BEGIN	

										--LISTA A PARIDADE VALOR COM MOEDA IGUAL DA MERCADORIA
										SELECT 
											@PDM_VL_PARIDADE = PVA_VL_PARIDADE
										FROM #TPPARIDADEVALOR
										WHERE MOE_ID = @MOE_ID  
		
										SELECT 
											@PDM_VL_PARIDADE_DOLAR = PVA_VL_PARIDADE
										FROM #TPPARIDADEVALOR
										WHERE MOE_ID = (SELECT MOE_ID FROM SCIEX_MOEDA	WHERE MOE_CO = @MOE_CO_DOLAR)	 
			 								
												
										INSERT INTO #TPDETALHEMERCADORIA
										SELECT
											PDM_ID, 
											PME_ID,
											PDM_VL_UNITARIO_COND_VENDA,
											PDM_VL_UNITARIO_COND_VENDA_DOLAR,
											PDM_VL_CONDICAO_VENDA, 
											PDM_VL_CONDICAO_VENDA_REAL, 
											PDM_VL_CONDICAO_VENDA_DOLAR, 
											1
										FROM SCIEX_PLI_DETALHE_MERCADORIA
										WHERE PME_ID = @PME_ID
					
										WHILE EXISTS(SELECT * FROM #TPDETALHEMERCADORIA WHERE SITUACAO = 1)
										BEGIN
												
											IF @MOE_CO = 790 -- CASO A MOEDA SEJA O REAL BRASILEIRO
											BEGIN

												SET @ERROCALCULO = 'ERRO AO CALCULAR CONDIÇÃO NO DETALHE MERCADORIA DO PLI ID '+CAST(@PLI_ID AS VARCHAR)

												SELECT TOP 1 
													@PDM_ID = PDM_ID,
													@PDM_VL_CONDICAO = PDM_VL_CONDICAO_VENDA,	
													@PDM_VL_UNITARIO_CONDICAO = PDM_VL_UNITARIO_COND_VENDA, 		
													@PDM_VL_UNITARIO_CONDICAO_DOLAR = (@PDM_VL_UNITARIO_CONDICAO / 	 @PDM_VL_PARIDADE_DOLAR),			
													@PDM_VL_CONDICAO_VENDA_REAL = PDM_VL_CONDICAO_VENDA,
													@PDM_VL_CONDICAO_VENDA_DOLAR = (@PDM_VL_CONDICAO_VENDA_REAL / @PDM_VL_PARIDADE_DOLAR)			
												FROM #TPDETALHEMERCADORIA
												WHERE SITUACAO = 1
																
												UPDATE SCIEX_PLI_DETALHE_MERCADORIA 
												SET
													PDM_VL_CONDICAO_VENDA_REAL = isnull(@PDM_VL_CONDICAO_VENDA_REAL,0),
													PDM_VL_CONDICAO_VENDA_DOLAR = isnull(@PDM_VL_CONDICAO_VENDA_DOLAR,0),
													PDM_VL_UNITARIO_COND_VENDA_DOLAR = isnull(@PDM_VL_UNITARIO_CONDICAO_DOLAR,0)
												WHERE PDM_ID = @PDM_ID


											END

											IF @MOE_CO <> 790 -- CASO A MOEDA SEJA DIFERENTE DO REAL BRASILEIRO
											BEGIN
												IF @MOE_CO = @MOE_CO_DOLAR -- CASO A MOEDA SEJA DOLAR
												BEGIN

													SET @ERROCALCULO = 'ERRO AO CALCULAR CONDIÇÃO NO DETALHE MERCADORIA DO PLI ID '+CAST(@PLI_ID AS VARCHAR)

													SELECT TOP 1 
														@PDM_ID = PDM_ID,
														@PDM_VL_CONDICAO = PDM_VL_CONDICAO_VENDA,
														@PDM_VL_UNITARIO_CONDICAO_DOLAR = PDM_VL_UNITARIO_COND_VENDA,
														@PDM_VL_CONDICAO_VENDA_REAL = (PDM_VL_CONDICAO_VENDA * @PDM_VL_PARIDADE_DOLAR),
														@PDM_VL_CONDICAO_VENDA_DOLAR = PDM_VL_CONDICAO_VENDA				
													FROM #TPDETALHEMERCADORIA
													WHERE SITUACAO = 1
																	
													UPDATE SCIEX_PLI_DETALHE_MERCADORIA 
													SET
														PDM_VL_CONDICAO_VENDA_REAL = @PDM_VL_CONDICAO_VENDA_REAL,
														PDM_VL_CONDICAO_VENDA_DOLAR = @PDM_VL_CONDICAO_VENDA_DOLAR,
														PDM_VL_UNITARIO_COND_VENDA_DOLAR = @PDM_VL_UNITARIO_CONDICAO_DOLAR
													WHERE PDM_ID = @PDM_ID
										
												END
												ELSE --CASO MOEDA NÃO SEJA NEM REAL NEM DOLAR
												BEGIN
														
													SET @ERROCALCULO = 'ERRO AO CALCULAR CONDIÇÃO NO DETALHE MERCADORIA DO PLI ID '+CAST(@PLI_ID AS VARCHAR)
															
													SELECT TOP 1 
														@PDM_ID = PDM_ID,
														@PDM_VL_CONDICAO = PDM_VL_CONDICAO_VENDA,
														@PDM_VL_UNITARIO_CONDICAO_DOLAR = ( (PDM_VL_UNITARIO_COND_VENDA * @PDM_VL_PARIDADE) / @PDM_VL_PARIDADE_DOLAR),
														@PDM_VL_CONDICAO_VENDA_REAL = (PDM_VL_CONDICAO_VENDA * @PDM_VL_PARIDADE),
														@PDM_VL_CONDICAO_VENDA_DOLAR = (@PDM_VL_CONDICAO_VENDA_REAL / @PDM_VL_PARIDADE_DOLAR)				
													FROM #TPDETALHEMERCADORIA
													WHERE SITUACAO = 1
											
													UPDATE SCIEX_PLI_DETALHE_MERCADORIA 
													SET
														PDM_VL_CONDICAO_VENDA_REAL = @PDM_VL_CONDICAO_VENDA_REAL,
														PDM_VL_CONDICAO_VENDA_DOLAR = @PDM_VL_CONDICAO_VENDA_DOLAR,
														PDM_VL_UNITARIO_COND_VENDA_DOLAR = @PDM_VL_UNITARIO_CONDICAO_DOLAR
													WHERE PDM_ID = @PDM_ID
										
												END
											END								
				
											UPDATE #TPDETALHEMERCADORIA
											SET
												SITUACAO = 2 
											WHERE PDM_ID = @PDM_ID	

											SET @PDM_VL_TOTAL_MERC_COND_VENDA = (@PDM_VL_TOTAL_MERC_COND_VENDA + ISNULL(@PDM_VL_CONDICAO_VENDA,0))
											SET @PDM_VL_TOTAL_MERC_VENDA_REAL = (@PDM_VL_TOTAL_MERC_VENDA_REAL + ISNULL(@PDM_VL_CONDICAO_VENDA_REAL,0))
											SET @PDM_VL_TOTAL_MERC_VENDA_DOLAR = (@PDM_VL_TOTAL_MERC_VENDA_DOLAR + ISNULL(@PDM_VL_CONDICAO_VENDA_DOLAR,0))

											SET @PDM_VL_UNITARIO_CONDICAO_DOLAR = 0
									
										END

										DELETE FROM #TPDETALHEMERCADORIA

								END 
								ELSE IF @MOE_CO = 790 -- SENDO REAL
								BEGIN
									SELECT 
										@PDM_VL_PARIDADE_DOLAR = PVA_VL_PARIDADE
									FROM #TPPARIDADEVALOR
									WHERE MOE_ID = (SELECT MOE_ID FROM SCIEX_MOEDA	WHERE MOE_CO = @MOE_CO_DOLAR)	 
			 								
									INSERT INTO #TPDETALHEMERCADORIA
									SELECT
										PDM_ID, 
										PME_ID,
										PDM_VL_UNITARIO_COND_VENDA,
										PDM_VL_UNITARIO_COND_VENDA_DOLAR,
										PDM_VL_CONDICAO_VENDA, 
										PDM_VL_CONDICAO_VENDA_REAL, 
										PDM_VL_CONDICAO_VENDA_DOLAR, 
										1
									FROM SCIEX_PLI_DETALHE_MERCADORIA
									WHERE PME_ID = @PME_ID
						
									WHILE EXISTS(SELECT * FROM #TPDETALHEMERCADORIA WHERE SITUACAO = 1)
									BEGIN
												
										IF @MOE_CO = 790 -- CASO A MOEDA SEJA O REAL BRASILEIRO
										BEGIN
											SET @ERROCALCULO = 'ERRO AO CALCULAR CONDIÇÃO NO DETALHE MERCADORIA DO PLI ID '+CAST(@PLI_ID AS VARCHAR)

											SELECT TOP 1 
												@PDM_ID = PDM_ID,
												@PDM_VL_CONDICAO = PDM_VL_CONDICAO_VENDA,
												@PDM_VL_UNITARIO_CONDICAO = PDM_VL_UNITARIO_COND_VENDA, 		
												@PDM_VL_UNITARIO_CONDICAO_DOLAR = (@PDM_VL_UNITARIO_CONDICAO / 	 @PDM_VL_PARIDADE_DOLAR),		
												@PDM_VL_CONDICAO_VENDA_REAL = PDM_VL_CONDICAO_VENDA,
												@PDM_VL_CONDICAO_VENDA_DOLAR = (@PDM_VL_CONDICAO_VENDA_REAL / @PDM_VL_PARIDADE_DOLAR)				
											FROM #TPDETALHEMERCADORIA
											WHERE SITUACAO = 1
						
											UPDATE SCIEX_PLI_DETALHE_MERCADORIA 
											SET
												PDM_VL_CONDICAO_VENDA_REAL = @PDM_VL_CONDICAO_VENDA_REAL,
												PDM_VL_CONDICAO_VENDA_DOLAR = @PDM_VL_CONDICAO_VENDA_DOLAR,
												PDM_VL_UNITARIO_COND_VENDA_DOLAR = @PDM_VL_UNITARIO_CONDICAO_DOLAR
											WHERE PDM_ID = @PDM_ID
		
										END
										
										UPDATE #TPDETALHEMERCADORIA
										SET
											SITUACAO = 2 
										WHERE PDM_ID = @PDM_ID
					
										SET @PDM_VL_TOTAL_MERC_COND_VENDA = (@PDM_VL_TOTAL_MERC_COND_VENDA + ISNULL(@PDM_VL_CONDICAO_VENDA,0))
										SET @PDM_VL_TOTAL_MERC_VENDA_REAL = (@PDM_VL_TOTAL_MERC_VENDA_REAL + ISNULL(@PDM_VL_CONDICAO_VENDA_REAL,0))
										SET @PDM_VL_TOTAL_MERC_VENDA_DOLAR = (@PDM_VL_TOTAL_MERC_VENDA_DOLAR + @PDM_VL_CONDICAO_VENDA_DOLAR)

										SET @PDM_VL_UNITARIO_CONDICAO_DOLAR = 0
									
									END

									DELETE FROM #TPDETALHEMERCADORIA

								END
								ELSE IF @MOE_CO <> 790
								BEGIN

									SET @OBS = ' PLI_ID = '+CAST(@PLI_ID AS VARCHAR) + ' COMPLEMENTADO COM PROBLEMA (MERCADORIA SEM PARIDADE DE VALOR PARA A MOEDA)'	
									SET @PLI_PROCESSADO = 0

									UPDATE  SCIEX_CONTROLE_EXEC_SERVICO
									SET CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')),
										CES_ME_OBJETO_RETORNO = @OBS,
										CES_ST_EXECUCAO = 2
									WHERE CES_ID = @CODIGOCONTROLEEXECUCAO
													
									--BREAK
								END 

								--ATUALIZA VALORES DA MERCADORIA
								IF @PLI_PROCESSADO <> 0
								BEGIN
									UPDATE SCIEX_PLI_MERCADORIA
									SET
										--PME_VL_TOTAL_CONDICAO_VENDA = (SELECT SUM(PDM_VL_CONDICAO_VENDA) FROM SCIEX_PLI_DETALHE_MERCADORIA WHERE PME_ID = @PME_ID) ,
										PME_VL_TOTAL_CONDICAO_VENDA_REAL = @PDM_VL_TOTAL_MERC_VENDA_REAL,
										PME_VL_TOTAL_CONDICAO_VENDA_DOLAR = @PDM_VL_TOTAL_MERC_VENDA_DOLAR
									WHERE PME_ID = @PME_ID
								
									UPDATE #TPPLIMERCADORIA
									SET
										SITUACAO = 2 
									WHERE PME_ID = @PME_ID	
							
									SET @PLI_VL_TOTAL_PLI_VENDA_REAL = (@PLI_VL_TOTAL_PLI_VENDA_REAL + @PDM_VL_TOTAL_MERC_VENDA_REAL)
									SET @PLI_VL_TOTAL_PLI_VENDA_DOLAR = (@PLI_VL_TOTAL_PLI_VENDA_DOLAR + @PDM_VL_TOTAL_MERC_VENDA_DOLAR)
									SET @PLI_VL_TOTAL_PLI_COND_VENDA = (@PLI_VL_TOTAL_PLI_COND_VENDA + @PDM_VL_TOTAL_MERC_COND_VENDA)
							
									SET @PDM_VL_TOTAL_MERC_VENDA_DOLAR = 0
									SET @PDM_VL_TOTAL_MERC_VENDA_REAL = 0
									SET @PDM_VL_TOTAL_MERC_COND_VENDA = 0
								END
								ELSE BREAK;
							END --FIM DO LOOP DA MERCADORIA


							IF @PLI_PROCESSADO = 1 -- CASO O PLI NÃO TENHA PROBLEMAS, ENTÃO SERÁ PROCESSADO
							BEGIN			
								UPDATE SCIEX_PLI SET
									PLI_VL_TOTAL_CONDICAO_VENDA = (SELECT SUM(PME_VL_TOTAL_CONDICAO_VENDA) FROM SCIEX_PLI_MERCADORIA WHERE PLI_ID = @PLI_ID),
									PLI_VL_TOTAL_CONDICAO_VENDA_REAL = @PLI_VL_TOTAL_PLI_VENDA_REAL,
									PLI_VL_TOTAL_CONDICAO_VENDA_DOLAR = @PLI_VL_TOTAL_PLI_VENDA_DOLAR
								WHERE PLI_ID = @PLI_ID

					
								--RN06 
								-- ATUALIZA O STATUS DO PLI PARA AGUARDANDO GERAÇÃO DE DÉBITO
								UPDATE SCIEX_PLI SET PLI_ST_PLI = 22 WHERE PLI_ID = @PLI_ID

								-- GRAVA NO HISTÓRICO 
								INSERT INTO SCIEX_PLI_HISTORICO (PLI_ST_PLI,PLI_ID, PHI_DH_EVENTO, PHI_DS_OBSERVACAO, PLI_ST_PLI_DESCRICAO, PHI_NO_RESPONSAVEL, PHI_NU_CPFCNPJ_RESPONSAVEL)
								VALUES(22,@PLI_ID, CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), 'AGUARDANDO GERAÇÃO DE DÉBITO', 'AGUARDANDO GERAÇÃO DE DÉBITO', @PLI_RESPONSAVEL_CADASTRO, @PLI_NUM_CPFCNPJ_RESPONSAVEL)

								-- GERA A INFORMAÇÃO
								SET @OBS = 'PLI_ID = '+ CAST(@PLI_ID AS VARCHAR) + ' COMPLEMENTADO COM SUCESSO';

								UPDATE  SCIEX_CONTROLE_EXEC_SERVICO
								SET CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')),
									CES_ME_OBJETO_RETORNO = @OBS,
									CES_ST_EXECUCAO = 1
								WHERE CES_ID = @CODIGOCONTROLEEXECUCAO
							END -- FIM DA RN04

							DELETE FROM #TPPLIMERCADORIA

						END

						IF (@PLI_PROCESSADO = 0)
						BEGIN

							UPDATE  SCIEX_CONTROLE_EXEC_SERVICO
								SET CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')),
									CES_ME_OBJETO_RETORNO = @OBS,
									CES_ST_EXECUCAO = 2
								WHERE CES_ID = @CODIGOCONTROLEEXECUCAO

						END

						UPDATE #TPPLI SET SITUACAO = 2 
						WHERE PLI_ID = @PLI_ID -- INCREMENTO DA CONDICAO DO WHILE

						DELETE FROM #TPPARIDADECAMBIAL
						DELETE FROM #TPPARIDADEVALOR

						SET @PCA_ID = 0
						SET @MOE_ID = 0
						SET @MOE_CO = 0

						--DETALHE_MERCADORIA
						SET @PDM_VL_TOTAL_MERC_COND_VENDA = 0
						SET @PDM_VL_TOTAL_MERC_VENDA_REAL = 0
						SET @PDM_VL_TOTAL_MERC_VENDA_DOLAR = 0

						--PLI
						SET @PLI_VL_TOTAL_PLI_VENDA_REAL = 0
						SET @PLI_VL_TOTAL_PLI_VENDA_DOLAR = 0
						SET @PLI_VL_TOTAL_PLI_COND_VENDA = 0

	
					END -- FINALIZA O LOOP DO PLI

					SELECT @OBS
					COMMIT 

				END TRY
				BEGIN CATCH  

					IF @@TRANCOUNT > 0
					BEGIN
						ROLLBACK

						BEGIN TRANSACTION
	
						INSERT INTO SCIEX_CONTROLE_EXEC_SERVICO
						(CES_DH_EXECUCAO_INICIO, CES_ST_EXECUCAO, CES_NU_CPF_CNPJ_INTERESSADO, CES_NO_CPF_CNPJ_INTERESSADO, LSE_ID, CES_ME_OBJETO_RETORNO)
						VALUES
						(CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), 2, @CPF_CNPJ, @NOMEIMPORTADOR, 7, @ERROCALCULO)

						COMMIT
					END
									
				END CATCH;

				DROP TABLE #TPPLI
				DROP TABLE #TPDETALHEMERCADORIA
				DROP TABLE #TPPLIMERCADORIA
				DROP TABLE #TPPARIDADEVALOR
				DROP TABLE #TPPARIDADECAMBIAL			
			";

			string CPFCNPJ = "";
			string NOMEUSUARIO = "";

			var usuarioLogado = _usuarioPssBll.PossuiUsuarioLogado();
			if (usuarioLogado != null)
			{
				CPFCNPJ = usuarioLogado.usuarioLogadoCpfCnpj;
				NOMEUSUARIO = usuarioLogado.usuarioLogadoNome;
			}
			else
			{
				CPFCNPJ = "ADM";
				NOMEUSUARIO = "ADM";
			}

			return SQL.Replace("NUMCPFCNPJ", CPFCNPJ).Replace("NOMEUSUARIO", NOMEUSUARIO).Replace("CODIGO_ID_PLI", this.complemento);
		}
	}
}