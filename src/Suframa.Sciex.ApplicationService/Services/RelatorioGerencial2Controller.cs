//using Fasterflect;
//using OfficeOpenXml;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
//using OfficeOpenXml.Style;
//using Suframa.Simnac.BusinessLogic;
//using Suframa.Simnac.BusinessLogic.RelatorioGerencial;
//using Suframa.Simnac.CrossCutting.DataTransferObject;
//using Suframa.Simnac.CrossCutting.DataTransferObject.Dto;
//using Suframa.Simnac.CrossCutting.DataTransferObject.Enum;
//using Suframa.Simnac.CrossCutting.DataTransferObject.ViewModel;
//using Suframa.Simnac.CrossCutting.DataTransferObject.ViewModel.Relatorios;
//using Suframa.Simnac.DataAccess.Database.Entities;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Reflection;
//using System.Web.Http;
//using System.Web.UI;
//using WebGrease.Css.Extensions;

//namespace Suframa.Sciex.ApplicationService.Services
//{
//	public class RelatorioGerencial2Controller : ApiController
//	{

//		private IRelatorioGerencialBll _relatorioBll;

//		private readonly string[] _colunas = new string[] { "C", "D", "E", "F", "G","H", "I", "J", "K", "L", "M","N", "O", "P", "Q", "R", "S","T", "U",
//															"V", "W", "X", "Y","Z", "AA", "AB", "AC", "AD","AE","AF", "AG", "AH", "AI", "AJ", "AK","AL",
//															"AM", "AN" ,"AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ","BA","BB",
//															"BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR",
//															"BS","BT","BU","BW","BX","BY","BZ","CA","CC","CD","CE","CF","CG"};

//		/*private readonly string[] _colunasVist2 = new string[] { "C", "D", "E", "F", "G","H", "I", "J", "K", "L", "M","N", "O", "P", "Q", "R", "S","T",
//															"V", "W", "X", "Y","Z", "AA", "AB", "AC", "AD","AE","AF", "AG", "AH", "AI", "AJ", "AK","AL",
//															 "AN" ,"AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ","BA","BB",
//															"BC","BD","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR",
//															"BS","BT","BU","BV","BX","BY","BZ","CA","CB","CC","CD","CE","CF","CG","CH","CI","CJ","CK","CL","CM","CN",
//		"CP","CQ","CR","CS","CU","CT","CV","CW","CX","CY","CZ","DA","DB","DC","DD","DE","DF","DH","DI","DJ","DK","DL","DM","DN","DO","DP","DQ","DR","DS","DT","DU","DV","DW","DX",
//		"DZ","EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","ER","ES","ET","EU","EV","EW","EX","EY","EZ","FA","FB","FC","FD","FE","FF","FG","FH","FJ","FK","FL",
//		"FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR"
//		,"GT","GU","GV","GW","GX","	GY","GZ","HA","HB","HC","HD","HE","HF","HG","HH","HI","HJ","HL","HM","HN","HO","HP","HQ","HR","HS","HT","HU","HV","HW","HX","HY","HZ","IA","IB","ID","IE","IF","IG","IH","II","IJ","IK","IL","IM","IN","IO","IP","IQ","IR","IS","IT","IV","IW","IX","IY","IZ","JA","JB","JC","JD","JE","JF","JG","JH","JI","JJ","JK","JL","JN","JO","JP","JQ","JR","JS","JT","JU","JV","JW","JX","JY","JZ","KA","KB","KC","KD","KF","KG","KH","KI","KJ","KK","KL","KM","KN","KO","KP","KQ","KR","KS","KT","KU","KV","KX","KY","KZ","LA","LB","LC","LD","LE","LF","LG","LH","LI","LJ","LK","LL","LM","LN","LP","LQ","LR","LS","LT","LU","LV","LW","LX","LY","LZ","MA","MB","MC","MD","ME","MF"};*/
//		private readonly int[] _colunasVist = new int[] {3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185,186,187,
//			188,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,232,233,234,235,236,237,238,239,240,241,242,243,244,245,246,247,248,249,250,251,252,253,254,255,256,257,258,259,260,261,262,263,264,265,266,267,268,269,270,271,272,273,274,275,276,277,278,279,280,281,282,283,284,285,286,287,288,289,290,291,292,293,294,295,296,297,298,299,300,301,302,303,304,305,306,307,308,309,310,311,312,313,314,315,316,317,318,319,320,321,322,323,324,325,326,327,328,329,330,331,332,333,334,335,336,337,338,339,340,341,342,343,344,
//		345,346,347,348,349,350,351,352,353,354,355,356,357,358,359,360,361,362,363,364,365,366,367,368,369,370,371,372,373,374,375,376,377,378,379,380,381,382,383,384,385,386,387,388,389,390,391,392,393,394,395,396,397,398,399,400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,419,420,421,422,423,424,425,426,427,428,429,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444,445,446,447,448,449,450,451,452,453,454,455,456,457,458,459,460,461,462,463,464,465,466,467,468,469,470,471,472,473,474,475,476,477,478,479,480,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,497,498,499,500,501,502,503,
//			504,505,506,507,508,509,510,511,512,513,514,515,516,517,518,519,520,521,522,523,524,525,526,527,528,529,530,531,532,533,534,535,536,537,538,539,540,541,542,543,544,545,546,547,548,549,550,551,552,553,554,555,556,557,558,559,560,561,562,563,564,565,566,567,568,569,570,571,572,573,574,575,576,577,578,579,580,581,582,583,584,585,586,587,588,589,590,591,592,593,594,595,596,597,598,599,600,601,602,603,604,605,606,607,608,609,610,611,612,613,614,615,616,617,618,619,620,621,622,623,624,625,626,627,628,629,630,631,632,633,634,635,636,637,638,639,640,641,642,643,644,645,646,647
//};
//	private string[] _meses = new string[] { "MES", "JANEIRO", "FEVEREIRO", "MARÇO", "ABRIL", "MAIO", "JUNHO", "JULHO", "AGOSTO", "SETEMBRO", "OUTUBRO", "NOVEMBRO", "DEZEMBRO" };
//		private  string[] dias = new string[] { "DIA", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
//		private string[] horas = new string[] { "HORA", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };

//		public RelatorioGerencial2Controller(IRelatorioGerencialBll relatorioGerencialBll)
//        {
//            _relatorioBll = relatorioGerencialBll;
//        }

//        private void AplicarFormatacaoCabecalho(ExcelWorksheet worksheet, RelatorioGerencialVM param)
//        {
//            var regional = "Selecionar a Regional: " + (param.FiltroPesquisa == 1 ? "Consolidado" : "Detalhado");
//            var periodo = "Selecionar o periodo: " + param.dataInicio.ToString("dd/MM/yyyy") + " à " + param.dataFim.ToString("dd/MM/yyyy");
//            var filtroPesquisa = "Selecionar a pesquisa: ";
//            if (param.nfeImportada == 1)
//            {
//                filtroPesquisa += "(A) ";
//            }

//            if (param.pinSolicitado == 1)
//            {
//                filtroPesquisa += "(B) ";
//            }
//            if (param.pinRegistrado == 1)
//            {

//                filtroPesquisa += "(C) ";
//            }
//            if (param.pinConfirmado == 1)
//            {

//                filtroPesquisa += "(D) ";
//            }
//            if (param.pinNecessidadeVistoria == 1)
//            {
//                filtroPesquisa += "(E) ";
//                /*
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinPendenteVistQtde;
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinPendenteVistValor;
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinPendenteVistPct;*/
//            }
//            if (param.pinVistoriado == 1)
//            {
//                filtroPesquisa += "(F) ";
//                /*
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinVistoriadoQtde;
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinVistoriadoValor;
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinVistoriadoPct;
//				*/
//            }
//            if (param.pinInternado == 1)
//            {
//                filtroPesquisa += "(G) ";
//            }
//            worksheet.Cells["A03"].Value = regional;
//            worksheet.Cells["D03"].Value = periodo;
//            worksheet.Cells["H03"].Value = filtroPesquisa;
//        }

//        private void AplicarFormatacaoConsolidado(ExcelWorksheet worksheet, int startRow, int rowCount, string columnPos, string color, string value, decimal numberValue = 0, bool bold = false, string fontColor = "#000000", ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Center)
//        {
//            //quando for moeda o tipo tem q se decimal para correta formatacao
//            if (String.IsNullOrEmpty(value))
//            {
//                worksheet.Cells[columnPos + (startRow + rowCount)].Style.Numberformat.Format = "###,###,##0.00";
//                worksheet.Cells[columnPos + (startRow + rowCount)].Value = numberValue;
//            }
//            else
//            {
//                worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//            }

//            //worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Merge = true;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Name = "Calibri";
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Size = 16;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Bold = bold;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(fontColor));

//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.HorizontalAlignment = align;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


//        }

//		private void AplicarFormatacaoConsolidado(ExcelWorksheet worksheet, int startRow, int rowCount, int columnPos, string color, string value, decimal numberValue = 0, bool bold = false, string fontColor = "#000000", ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Center)
//		{
//			//quando for moeda o tipo tem q se decimal para correta formatacao
//			if (String.IsNullOrEmpty(value))
//			{
//				worksheet.Cells[startRow,columnPos].Style.Numberformat.Format = "###,###,##0.00";
//				worksheet.Cells[startRow,columnPos].Value = numberValue;
//			}
//			else
//			{
//				worksheet.Cells[startRow,columnPos].Value = value;
//			}

//			//worksheet.Cells[startRow,columnPos + ":" + columnPos + (startRow + rowCount + 1)].Merge = true;
//			worksheet.Cells[startRow,columnPos].Style.Font.Name = "Calibri";
//			worksheet.Cells[startRow,columnPos].Style.Font.Size = 16;
//			worksheet.Cells[startRow,columnPos].Style.Font.Bold = bold;
//			worksheet.Cells[startRow,columnPos].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(fontColor));

//			worksheet.Cells[startRow,columnPos].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[startRow,columnPos].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[startRow,columnPos].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[startRow,columnPos].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[startRow,columnPos].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[startRow,columnPos].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[startRow,columnPos].Style.HorizontalAlignment = align;
//			worksheet.Cells[startRow,columnPos].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


//		}

//		private void AplicarFormatacaoConsolidadoRegiao(ExcelWorksheet worksheet, int startRow, int rowCount, int rowSize, string columnPos, string value)
//        {
//            //depois recalcular esse if
//            if (rowSize > 1)
//            {
//                rowSize = (rowSize * 2) - 1;
//            }
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Merge = true;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.Font.Name = "Calibri";
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.Font.Size = 14;

//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.Fill.PatternType = ExcelFillStyle.Solid;


//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//            worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//        }

//        private void AplicarFormatacaoConsolidadoCidade(ExcelWorksheet worksheet, int startRow, int rowCount, int rowSize, string columnPos, string color, string value, bool bold, string fontColor = "#000000", bool italic = false, bool centered = false)
//        {
//            //worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + rowSize + 1)].Merge = true;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Name = "Calibri";
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Size = 14;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Bold = bold;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Italic = italic;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(fontColor));

//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.PatternType = ExcelFillStyle.Solid;


//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//            if (centered)
//            {
//                worksheet.Cells[columnPos + (startRow + rowCount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//            } else
//            {
//                worksheet.Cells[columnPos + (startRow + rowCount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
//            }
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//            worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//        }

//        private RelatorioGerencialDto CriarMockRelatorio()
//        {
//            var relatorio = new RelatorioGerencialDto();
//            relatorio.TotalNfeValor = 999999;
//            relatorio.TotalNfeQtde = 999;
//            relatorio.TotalNfePct = "99%";

//            relatorio.TotalPinRegistradoPct = "99%";
//            relatorio.TotalPinRegistradoValor = 999999;
//            relatorio.TotalPinRegistradoQtde = 999;

//            relatorio.TotalPinSolicitadoQtde = 999;
//            relatorio.TotalPinSolicitadoValor = 999999;
//            relatorio.TotalPinSolicitadoPct = "99%";

//            relatorio.TotalPinConfirmadoQtde = 999;
//            relatorio.TotalPinConfirmadoValor = 999999;
//            relatorio.TotalPinConfirmadoPct = "99%";

//            relatorio.TotalPinPendenteVistQtde = 999;
//            relatorio.TotalPinPendenteVistValor = 999999;
//            relatorio.TotalPinPendenteVistPct = "99%";

//            relatorio.TotalPinVistoriadoQtde = 999;
//            relatorio.TotalPinVistoriadoValor = 999999;
//            relatorio.TotalPinVistoriadoPct = "99%";

//            relatorio.TotalPinInternadoQtde = 999;
//            relatorio.TotalPinInternadoValor = 999999;
//            relatorio.TotalPinInternadoPct = "99%";


//            EstadoDto estado = new EstadoDto();
//            estado.Nome = "Amazonas";

//            CidadeDto manaus = new CidadeDto();
//            manaus.Nome = "Manaus";
//            manaus.TotalNfeValor = 999999;
//            manaus.TotalNfeQtde = 999;
//            manaus.TotalNfePct = "99%";

//            manaus.TotalPinRegistradoPct = "99%";
//            manaus.TotalPinRegistradoValor = 999999;
//            manaus.TotalPinRegistradoQtde = 999;

//            manaus.TotalPinSolicitadoQtde = 999;
//            manaus.TotalPinSolicitadoValor = 999999;
//            manaus.TotalPinSolicitadoPct = "99%";

//            manaus.TotalPinConfirmadoQtde = 999;
//            manaus.TotalPinConfirmadoValor = 999999;
//            manaus.TotalPinConfirmadoPct = "99%";

//            manaus.TotalPinPendenteVistQtde = 999;
//            manaus.TotalPinPendenteVistValor = 999999;
//            manaus.TotalPinPendenteVistPct = "99%";

//            manaus.TotalPinVistoriadoQtde = 999;
//            manaus.TotalPinVistoriadoValor = 999999;
//            manaus.TotalPinVistoriadoPct = "99%";

//            manaus.TotalPinInternadoQtde = 999;
//            manaus.TotalPinInternadoValor = 999999;
//            manaus.TotalPinInternadoPct = "99%";

//            estado.Cidades.Add(manaus);

//            CidadeDto ita = new CidadeDto();
//            ita.Nome = "Itacoatiara";
//            ita.TotalNfeValor = 999999;
//            ita.TotalNfeQtde = 999;
//            ita.TotalNfePct = "99%";

//            ita.TotalPinRegistradoPct = "99%";
//            ita.TotalPinRegistradoValor = 999999;
//            ita.TotalPinRegistradoQtde = 999;

//            ita.TotalPinSolicitadoQtde = 999;
//            ita.TotalPinSolicitadoValor = 999999;
//            ita.TotalPinSolicitadoPct = "99%";

//            ita.TotalPinConfirmadoQtde = 999;
//            ita.TotalPinConfirmadoValor = 999999;
//            ita.TotalPinConfirmadoPct = "99%";

//            ita.TotalPinPendenteVistQtde = 999;
//            ita.TotalPinPendenteVistValor = 999999;
//            ita.TotalPinPendenteVistPct = "99%";

//            ita.TotalPinVistoriadoQtde = 999;
//            ita.TotalPinVistoriadoValor = 999999;
//            ita.TotalPinVistoriadoPct = "99%";

//            ita.TotalPinInternadoQtde = 999;
//            ita.TotalPinInternadoValor = 999999;
//            ita.TotalPinInternadoPct = "99%";

//            estado.Cidades.Add(ita);

//            var acre = CriarMockEstado("Acre");

//            relatorio.Estados.Add(estado);
//            relatorio.Estados.Add(acre);

//            return relatorio;
//        }

//        private EstadoDto CriarMockEstado(string nome)
//        {
//            EstadoDto estado = new EstadoDto();
//            estado.Nome = nome;

//            CidadeDto manaus = new CidadeDto();
//            manaus.Nome = "Manaus";
//            manaus.TotalNfeValor = 999999;
//            manaus.TotalNfeQtde = 999;
//            manaus.TotalNfePct = "99%";

//            manaus.TotalPinRegistradoPct = "99%";
//            manaus.TotalPinRegistradoValor = 999999;
//            manaus.TotalPinRegistradoQtde = 999;

//            manaus.TotalPinSolicitadoQtde = 999;
//            manaus.TotalPinSolicitadoValor = 999999;
//            manaus.TotalPinSolicitadoPct = "99%";

//            manaus.TotalPinConfirmadoQtde = 999;
//            manaus.TotalPinConfirmadoValor = 999999;
//            manaus.TotalPinConfirmadoPct = "99%";

//            manaus.TotalPinPendenteVistQtde = 999;
//            manaus.TotalPinPendenteVistValor = 999999;
//            manaus.TotalPinPendenteVistPct = "99%";

//            manaus.TotalPinVistoriadoQtde = 999;
//            manaus.TotalPinVistoriadoValor = 999999;
//            manaus.TotalPinVistoriadoPct = "99%";

//            manaus.TotalPinInternadoQtde = 999;
//            manaus.TotalPinInternadoValor = 999999;
//            manaus.TotalPinInternadoPct = "99%";

//            estado.Cidades.Add(manaus);

//            CidadeDto ita = new CidadeDto();
//            ita.Nome = "Itacoatiara";
//            ita.TotalNfeValor = 999999;
//            ita.TotalNfeQtde = 999;
//            ita.TotalNfePct = "99%";

//            ita.TotalPinRegistradoPct = "99%";
//            ita.TotalPinRegistradoValor = 999999;
//            ita.TotalPinRegistradoQtde = 999;

//            ita.TotalPinSolicitadoQtde = 999;
//            ita.TotalPinSolicitadoValor = 999999;
//            ita.TotalPinSolicitadoPct = "99%";

//            ita.TotalPinConfirmadoQtde = 999;
//            ita.TotalPinConfirmadoValor = 999999;
//            ita.TotalPinConfirmadoPct = "99%";

//            ita.TotalPinPendenteVistQtde = 999;
//            ita.TotalPinPendenteVistValor = 999999;
//            ita.TotalPinPendenteVistPct = "99%";

//            ita.TotalPinVistoriadoQtde = 999;
//            ita.TotalPinVistoriadoValor = 999999;
//            ita.TotalPinVistoriadoPct = "99%";

//            ita.TotalPinInternadoQtde = 999;
//            ita.TotalPinInternadoValor = 999999;
//            ita.TotalPinInternadoPct = "99%";

//            estado.Cidades.Add(ita);

//            return estado;
//        }

//        private void AplicarFormatacaoTituloConsolidadoTopo(ExcelWorksheet worksheet, string columnPos, string color, string value)
//        {
//            worksheet.Cells[columnPos].Value = value;
//            worksheet.Cells[columnPos].Style.Font.Name = "Calibri";
//            worksheet.Cells[columnPos].Style.Font.Size = 24;

//            worksheet.Cells[columnPos].Style.Fill.PatternType = ExcelFillStyle.Solid;
//            worksheet.Cells[columnPos].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//            worksheet.Cells[columnPos].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//            worksheet.Cells[columnPos].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
//        }

//		private void AplicarFormatacaoTituloConsolidadoTopo2(ExcelWorksheet worksheet, int row,int column, string color, string value)
//		{
//			worksheet.Cells[row, column].Value = value;
//			worksheet.Cells[row, column].Style.Font.Name = "Calibri";
//			worksheet.Cells[row, column].Style.Font.Size = 24;
//			worksheet.Cells[row, column].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
//			worksheet.Cells[row, column].Style.WrapText = true;

//			worksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[row, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[row, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[row, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[row, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//			worksheet.Cells[row, column].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
//		}

//		private void AplicarFormatacaoSumarioColuna(ExcelWorksheet worksheet, int startRow, int rowCount, string columnPos, string color, string value)
//        {
//            worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Merge = true;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Font.Name = "Calibri";
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Font.Size = 14;

//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//            worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//        }

//        private void AplicarFormatacaoTituloConsolidado(ExcelWorksheet worksheet, int startRow, int rowCount, int columnPos, string color, string value,bool mergeNextRow = false)
//        {
//            worksheet.Cells[startRow,columnPos].Value = value;
//            worksheet.Cells[startRow, columnPos].Style.Font.Name = "Calibri";
//            worksheet.Cells[startRow, columnPos].Style.Font.Size = 14;

//			if (mergeNextRow)
//			{
//				worksheet.Cells[startRow, columnPos,startRow+1,columnPos].Merge = true;
//				worksheet.Cells[startRow, columnPos, startRow + 1, columnPos].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
//				worksheet.Cells[startRow, columnPos, startRow + 1, columnPos].Style.WrapText = true;

//			}
//			worksheet.Cells[startRow, columnPos].Style.Fill.PatternType = ExcelFillStyle.Solid;
//            worksheet.Cells[startRow, columnPos].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//            worksheet.Cells[startRow, columnPos].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[startRow, columnPos].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[startRow, columnPos].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[startRow, columnPos].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[startRow, columnPos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//            worksheet.Cells[startRow, columnPos].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
		

//        }

//		private void AplicarFormatacaoTituloConsolidado(ExcelWorksheet worksheet, int startRow, int rowCount, string columnPos, string color, string value)
//		{
//			worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Font.Name = "Calibri";
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Font.Size = 14;

//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//			worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//		}

//		private string FormatarQuantidade(int numero)
//        {
//            if (numero > 0)
//            {
//                return numero.ToString("#,##");
//            }
//            return numero.ToString();
//        }

//		private string FormatarQuantidade(long numero)
//		{
//			if (numero > 0)
//			{
//				return numero.ToString("#,##");
//			}
//			return numero.ToString();
//		}

//		private void AplicarCorGeralSuframa(ExcelWorksheet worksheet, int startRow, int rowCount, string columnPos, string color, string value, decimal numberValue = 0, string fontColor = "#FFFFFF", ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Center)
//        {

//            if (String.IsNullOrEmpty(value))
//            {
//                worksheet.Cells[columnPos + (startRow + rowCount)].Style.Numberformat.Format = "###,###,##0.00";
//                worksheet.Cells[columnPos + (startRow + rowCount)].Value = numberValue;
//            }
//            else
//            {
//                worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//            }
//            //worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Merge = true;

//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Name = "Calibri";
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Size = 22;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(fontColor));

//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.HorizontalAlignment = align;
//            worksheet.Cells[columnPos + (startRow + rowCount)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//        }

//		private void AplicarCorGeralSuframa(ExcelWorksheet worksheet, int startRow, int rowCount, int columnPos, string color, string value, decimal numberValue = 0, string fontColor = "#FFFFFF", ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Center)
//		{

//			if (String.IsNullOrEmpty(value))
//			{
//				worksheet.Cells[startRow,columnPos].Style.Numberformat.Format = "###,###,##0.00";
//				worksheet.Cells[startRow, columnPos].Value = numberValue;
//			}
//			else
//			{
//				worksheet.Cells[startRow, columnPos].Value = value;
//			}
//			//worksheet.Cells[columnPos + (startRow + rowCount) + ":" + columnPos + (startRow + rowCount + 1)].Merge = true;

//			worksheet.Cells[startRow, columnPos].Style.Font.Name = "Calibri";
//			worksheet.Cells[startRow, columnPos].Style.Font.Size = 22;
//			worksheet.Cells[startRow, columnPos].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(fontColor));

//			worksheet.Cells[startRow, columnPos].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[startRow, columnPos].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[startRow, columnPos].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[startRow, columnPos].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[startRow, columnPos].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[startRow, columnPos].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[startRow, columnPos].Style.HorizontalAlignment = align;
//			worksheet.Cells[startRow, columnPos].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//		}
//		public HttpResponseMessage Get([FromUri] RelatorioGerencialVM param)
//        {
//            var pagedFilter = new RelatorioGerencialVM();
//            pagedFilter.dataInicio = new DateTime(2019, 1, 1);
//            pagedFilter.dataFim = new DateTime(2020, 1, 1);


//            //var relatorio = CriarMockRelatorio();

//            var relatorio = _relatorioBll.PesquisarDadosRelatorio(param);
//            var templatePath = AppDomain.CurrentDomain.BaseDirectory + "template3.xlsx";
//            var templateFile = new FileInfo(templatePath);

//            ExcelPackage pck = new OfficeOpenXml.ExcelPackage(templateFile, true);
//            ExcelWorksheet worksheet = pck.Workbook.Worksheets[1];
//			//var _colunas = new string[] { "C", "D", "E", "F", "H", "I", "J", "K", "L", "N", "O", "P", "Q", "R", "T", "U", "V", "W", "X", "Z", "AA", "AB", "AC", "AD", "AF", "AG", "AH", "AI", "AJ", "AL", "AM", "AN" };
//			var _colunas = new string[] { "C", "D", "E", "F", "H", "I", "J", "K", "L", "N", "O", "P", "Q", "R", "T", "U", "V", "W", "X", "Z", "AA", "AB", "AC", "AD", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BB", "BC", "BD"  };

//			//var _colunas = new string[] { "C", "E", "F", "H", "I", "J", "L", "M", "N", "P", "Q", "R", "T", "U", "V", "X", "Y", "Z", "AB", "AC", "AD" };
//			var _colunasIndex = 0;
//            AplicarFormatacaoCabecalho(worksheet, param);
//            int startRow = 6;
//            int columnMergeCount = 3;
//            //Consolidado geral suframa
//            if (param.nfeImportada == 1)
//            {
//                worksheet.Cells["C6:F6"].Merge = true;
//                AplicarFormatacaoTituloConsolidadoTopo(worksheet, "C6:F6", "#375623", "NFE IMPORTADA (A)");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "Quant");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalNfeQtde), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "R$");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalNfeValor, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% PIN");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% VALORES");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");
//                /*
//				worksheet.Cells[_colunas[_colunasIndex++]+"09"].Value = relatorio.TotalNfeQtde;
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalNfeValor;
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalNfePct;
//				*/
//            } else
//            {
//                _colunas = new string[] { "C", "D", "E", "F", "G", "I", "J", "K", "L", "M", "O", "P", "Q", "R", "S", "U", "V", "W", "X", "Y", "AA", "AB", "AC", "AD", "AE", "AG", "AH", "AI", "AJ", "AL", "AM", "AN" };
//            }

//            if (param.pinSolicitado == 1)
//            {

//                decimal total = 0M;
//                if (relatorio.TotalPinRegistradoQtde > 0 && relatorio.TotalPinSolicitadoQtde > 0)
//                {
//                    total = (DivisaoSegura((decimal)relatorio.TotalPinSolicitadoQtde, (decimal)relatorio.TotalNfeQtde)) * 100;

//                }

//                var columnIndex = _colunas[_colunasIndex] + "6:" + _colunas[_colunasIndex + columnMergeCount] + "6";
//                worksheet.Cells[columnIndex].Merge = true;
//                AplicarFormatacaoTituloConsolidadoTopo(worksheet, columnIndex, "#375623", "PIN SOLICITADO (B)");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "Quant");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalPinSolicitadoQtde), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "R$");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalPinSolicitadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% PIN");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinSolicitadoPct.ToString(), 0, "#FFC000");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% VALORES");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinSolicitadoPct.ToString(), 0, "#FFC000");

//                //if (param.nfeImportada == 1)
//                //{
//                AplicarFormatacaoSumarioColuna(worksheet, 6, 0, _colunas[_colunasIndex], "#262626", "% IMPORTAÇÃO EFETIVADA");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", total.ToString("0.##") + "%", 0, "#FFC000");
//                //}




//                /*
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinSolicitadoQtde;
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinSolicitadoValor;
//				worksheet.Cells[_colunas[_colunasIndex++] + "09"].Value = relatorio.TotalPinSolicitadoPct;
//				*/
//            }
//            if (param.pinRegistrado == 1)
//            {

//                decimal total = 0M;
//                if (relatorio.TotalPinRegistradoQtde > 0 && relatorio.TotalPinSolicitadoQtde > 0)
//                {
//                    total = (DivisaoSegura((decimal)relatorio.TotalPinRegistradoQtde, (decimal)relatorio.TotalPinSolicitadoQtde)) * 100;

//                }

//                var columnIndex = _colunas[_colunasIndex] + "6:" + _colunas[_colunasIndex + columnMergeCount] + "6";

//                worksheet.Cells[columnIndex].Merge = true;
//                AplicarFormatacaoTituloConsolidadoTopo(worksheet, columnIndex, "#375623", "PIN REGISTRADO (C)");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "Quant");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalPinRegistradoQtde), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "R$");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalPinRegistradoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% PIN");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinRegistradoPct.ToString(), 0, "#FFC000");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% VALORES");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinRegistradoPct.ToString(), 0, "#FFC000");

//                //if (param.pinSolicitado == 1)
//                //{
//                AplicarFormatacaoSumarioColuna(worksheet, 6, 0, _colunas[_colunasIndex], "#262626", "% PIN EFETIVADO");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", total.ToString("0.##") + "%", 0, "#FFC000");
//                //}


//            }
//            if (param.pinConfirmado == 1)
//            {

//                decimal total = 0M;
//                if (relatorio.TotalPinRegistradoQtde > 0 && relatorio.TotalPinRegistradoQtde > 0)
//                {
//                    total = (DivisaoSegura((decimal)relatorio.TotalPinConfirmadoQtde, (decimal)relatorio.TotalPinRegistradoQtde)) * 100;

//                }

//                var columnIndex = _colunas[_colunasIndex] + "6:" + _colunas[_colunasIndex + columnMergeCount] + "6";
//                worksheet.Cells[columnIndex].Merge = true;
//                AplicarFormatacaoTituloConsolidadoTopo(worksheet, columnIndex, "#375623", "PIN CONF. RECEBIMENTO MERCADORIA (D)");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "Quant");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalPinConfirmadoQtde), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "R$");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalPinConfirmadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% PIN");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinConfirmadoPct.ToString(), 0, "#FFC000");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% VALORES");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinConfirmadoPct.ToString(), 0, "#FFC000");

//                //if (param.pinRegistrado == 1)
//                //{
//                AplicarFormatacaoSumarioColuna(worksheet, 6, 0, _colunas[_colunasIndex], "#262626", "% PIN DISPONIBILIZADO PARA VISTORIA");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", total.ToString("0.##") + "%", 0, "#FFC000");
//                //}


//            }
//            if (param.pinNecessidadeVistoria == 1)
//            {



//                decimal total = 0M;
//                if (relatorio.TotalPinPendenteVistQtde > 0 && relatorio.TotalPinConfirmadoQtde > 0)
//                {
//                    total = (DivisaoSegura((decimal)relatorio.TotalPinPendenteVistQtde, (decimal)relatorio.TotalPinConfirmadoQtde)) * 100;
//                }

//                var columnIndex = _colunas[_colunasIndex] + "6:" + _colunas[_colunasIndex + columnMergeCount] + "6";
//                worksheet.Cells[columnIndex].Merge = true;
//                AplicarFormatacaoTituloConsolidadoTopo(worksheet, columnIndex, "#375623", "PIN NECESSIDADE VISTORIA (E)");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "Quant");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalPinPendenteVistQtde), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "R$");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalPinPendenteVistValor, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% PIN");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinPendenteVistPct.ToString(), 0, "#FFC000");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% VALORES");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinPendenteVistPct.ToString(), 0, "#FFC000");

//                //if (param.pinConfirmado == 1)
//                //	{
//                AplicarFormatacaoSumarioColuna(worksheet, 6, 0, _colunas[_colunasIndex], "#262626", "% PIN SELECIONADO PARA VISTORIA");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", total.ToString("0.##") + "%", 0, "#FFC000");
//                //}
//            }
//            if (param.pinVistoriado == 1)
//            {

//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalCanalAzulQuant), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalCanalAzulValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				//AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalAzulVistoriado, relatorio.CanalAzulVistoriado)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", ((DivisaoSegura(cidade.CanalAzulVistoriado, relatorio.CanalAzulVistoriado)) * 100).ToString("0.##") + "%", 0, "#FFC000");
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");

//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalCanalVerdeQuant), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalCanalVerdeValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalVerdePct, 0, "#FFC000");
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalVerdePct, 0, "#FFC000");

//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalCanalVermelhoQuant), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalCanalVermelhoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalVermelhoPct, 0, "#FFC000");
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalVermelhoPct, 0, "#FFC000");

//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalCanalCinzaQuant), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalCanalCinzaValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalCinzaPct, 0, "#FFC000");
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalCinzaPct, 0, "#FFC000");

//				//total

//				int totalCanais = relatorio.CanalCinzaVistoriado + relatorio.CanalVermelhoVistoriado + relatorio.CanalVerdeVistoriado + relatorio.CanalAzulVistoriado;
//				decimal totalValor = relatorio.CanalCinzaVistoriadoValor + relatorio.CanalVermelhoVistoriadoValor + relatorio.CanalVerdeVistoriadoValor + relatorio.CanalAzulVistoriadoValor;

//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(totalCanais), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", Convert.ToDecimal(totalValor), "#FFFFFF", ExcelHorizontalAlignment.Right);
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");
//				AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "100%", 0, "#FFC000");

//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalCinzaPct, 0, "#FFC000");
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalCinzaPct, 0, "#FFC000");
//				//AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalCanalCinzaPct, 0, "#FFC000");

//				/*
//				AplicarFormatacaoTituloConsolidado(worksheet, 8, 0, _colunas[_colunasIndex], "#00B050", "PIN VISTORIADO (F)");
//				AplicarCorGeralSuframa(worksheet, 9, 0, _colunas[_colunasIndex++], "#00B050", relatorio.TotalPinVistoriadoQtde.ToString());

//				AplicarFormatacaoTituloConsolidado(worksheet, 8, 0, _colunas[_colunasIndex], "#00B050", "R$");
//				AplicarCorGeralSuframa(worksheet, 9, 0, _colunas[_colunasIndex++], "#00B050", "", relatorio.TotalPinVistoriadoValor);

//				AplicarFormatacaoTituloConsolidado(worksheet, 8, 0, _colunas[_colunasIndex], "#00B050", "% (MERCADORIA RECEBIDA)");
//				*/
//				decimal total = 0M;
//                if (relatorio.TotalPinVistoriadoQtde > 0 && relatorio.TotalPinConfirmadoQtde > 0)
//                {
//                    total = (DivisaoSegura((decimal)relatorio.TotalPinVistoriadoQtde, (decimal)relatorio.TotalPinConfirmadoQtde)) * 100;
//                }
//                //AplicarCorGeralSuframa(worksheet, 9, 0, _colunas[_colunasIndex++], "#00B050", total.ToString("0.##")+"%");

//				/*

//                var columnIndex = _colunas[_colunasIndex] + "6:" + _colunas[_colunasIndex + columnMergeCount] + "6";
//                worksheet.Cells[columnIndex].Merge = true;
//                AplicarFormatacaoTituloConsolidadoTopo(worksheet, columnIndex, "#375623", "PIN VISTORIADO (F)");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "Quant");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalPinVistoriadoQtde), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "R$");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalPinVistoriadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% PIN");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinVistoriadoPct.ToString(), 0, "#FFC000");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "% VALORES");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", relatorio.TotalPinVistoriadoPct.ToString(), 0, "#FFC000");
//				*/
//                //if (param.pinConfirmado == 1)
//                //{
//               // AplicarFormatacaoSumarioColuna(worksheet, 6, 0, _colunas[_colunasIndex], "#262626", "% PIN VISTORIADO X PIN RECEBIDO");
//                //AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", total.ToString("0.##") + "%", 0, "#FFC000");
//                //}
//            }
//            if (param.pinInternado == 1)
//            {
//                /*
//				AplicarFormatacaoTituloConsolidado(worksheet, 8, 0, _colunas[_colunasIndex], "#8064A2", "PIN INTERNADO (G)");
//				AplicarCorGeralSuframa(worksheet, 9, 0, _colunas[_colunasIndex++], "#8064A2", relatorio.TotalPinInternadoQtde.ToString());

//				AplicarFormatacaoTituloConsolidado(worksheet, 8, 0, _colunas[_colunasIndex], "#8064A2", "R$");
//				AplicarCorGeralSuframa(worksheet, 9, 0, _colunas[_colunasIndex++], "#8064A2", "", relatorio.TotalPinInternadoValor);

//				AplicarFormatacaoTituloConsolidado(worksheet, 8, 0, _colunas[_colunasIndex], "#8064A2", "% (PIN VISTORIADO)");*/
//                decimal total = 0M;
//                if (relatorio.TotalPinInternadoQtde > 0 && relatorio.TotalPinVistoriadoQtde > 0)
//                {
//                    total = (DivisaoSegura((decimal)relatorio.TotalPinInternadoQtde, (decimal)relatorio.TotalPinVistoriadoQtde)) * 100;
//                }

//                //AplicarCorGeralSuframa(worksheet, 9, 0, _colunas[_colunasIndex++], "#8064A2", total.ToString("0.##") + "%");




//                var columnIndex = _colunas[_colunasIndex] + "6:" + _colunas[_colunasIndex + 1] + "6";
//                worksheet.Cells[columnIndex].Merge = true;
//                AplicarFormatacaoTituloConsolidadoTopo(worksheet, columnIndex, "#375623", "PIN INTERNADO (G)");

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "Quant");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", FormatarQuantidade(relatorio.TotalPinInternadoQtde), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                AplicarFormatacaoTituloConsolidado(worksheet, 7, 0, _colunas[_colunasIndex], "#CCCCCC", "R$");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex++], "#262626", "", relatorio.TotalPinInternadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);

//                //if (param.pinVistoriado == 1)
//                //{
//                AplicarFormatacaoSumarioColuna(worksheet, 6, 0, _colunas[_colunasIndex], "#262626", "% PIN INTERNADO X PIN VISTORIADO");
//                AplicarCorGeralSuframa(worksheet, 8, 0, _colunas[_colunasIndex], "#262626", total.ToString("0.##") + "%", 0, "#FFC000");
//                //}
//            }

//            //Consolidado regionais

//            startRow = 9;
//            var rowSize = 1;

//            foreach (CrossCutting.DataTransferObject.Dto.UnidadeCadastradoraDto unidade in relatorio.Unidades)
//            {
//				// var colunas = new string[] { "C", "D", "E", "F", "H", "I", "J", "K", "L", "N", "O", "P", "Q", "R", "T", "U", "V", "W", "X", "Z", "AA", "AB", "AC", "AD", "AF", "AG", "AH", "AI", "AJ", "AL", "AM", "AN" };
//				var colunas = new string[] { "C", "D", "E", "F", "H", "I", "J", "K", "L", "N", "O", "P", "Q", "R", "T", "U", "V", "W", "X", "Z", "AA", "AB", "AC", "AD", "AF", "AG", "AH", "AI", "AJ","AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BB", "BC", "BD" };
//				if (param.nfeImportada == 0)
//                {
//                    colunas = new string[] { "C", "D", "E", "F", "G", "I", "J", "K", "L", "M", "O", "P", "Q", "R", "S", "U", "V", "W", "X", "Y", "AA", "AB", "AC", "AD", "AE", "AG", "AH", "AI", "AJ", "AL", "AM", "AN" };
//                }
//                //AplicarFormatacaoConsolidadoRegiao(worksheet, startRow, 0, estado.Cidades.Count, "A", estado.Nome);
//                int rowCount = 0;
//                var ucCount = 0;
//                foreach (CidadeDto cidade in unidade.Cidades)
//                {
//                    int colunaIndex = 0;
//                    string color = cidade.destaque == true ? "#C6E0B4" : "#FFFFFF";
//                    string cidadeFontColor = "#000000";
//                    string cidadePinsValoresFontColor = "#000000";
//                    string cidadePinsValoresFontColorBack = color;
//                    if (cidade.destaque && cidade.totalizadorEstado && color == "#C6E0B4")
//                    {
//                        color = "#595959";
//                        cidadeFontColor = "#FFFFFF";
//                        cidadePinsValoresFontColor = "#F4B084";
//                        cidadePinsValoresFontColorBack = "#404040";
//                    } else
//                    {
//                        ucCount++;
//                        if (ucCount == 1)
//                        {
//                            rowCount = rowCount + 1;
//                        }
//                    }

//                    if (cidade.MunicipioUfUc)
//                    {
//                        color = "#DDEBF7";
//                        cidadeFontColor = "#7030A0";
//                        cidadePinsValoresFontColor = "#7030A0";
//                        cidadePinsValoresFontColorBack = "#DDEBF7";
//                    } else if (cidade.MunicipioOutraUf)
//                    {
//                        cidade.destaque = true;
//                        color = "#FFFFFF";
//                        cidadeFontColor = "#0070C0";
//                    }
//                    AplicarFormatacaoConsolidadoCidade(worksheet, startRow, rowCount, 0, "A", color, cidade.Nome, cidade.destaque, cidadeFontColor, cidade.MunicipioOutraUf, cidade.totalizadorEstado);

//                    if (param.nfeImportada == 1)
//                    {
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.TotalNfeQtde), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.TotalNfeValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalNfePct.ToString() + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.TotalNfeValor, relatorio.TotalNfeValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//                    }

//                    if (param.pinSolicitado == 1) {
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.TotalPinSolicitadoQtde), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.TotalPinSolicitadoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalPinSolicitadoPct.ToString() + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.TotalPinSolicitadoValor, relatorio.TotalPinSolicitadoValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//                        //if(param.nfeImportada == 1)
//                        //{
//                        if (cidade.TotalNfeQtde > 0)
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", ((DivisaoSegura(cidade.TotalPinSolicitadoQtde, (float)cidade.TotalNfeQtde)) * 100).ToString("0.##") + "%", 0, cidade.destaque, "#F4B084");

//                        }
//                        else
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", "0" + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        //}

//                    }

//                    if (param.pinRegistrado == 1)
//                    {
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.TotalPinRegistradoQtde), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.TotalPinRegistradoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalPinRegistradoPct.ToString() + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.TotalPinRegistradoValor, relatorio.TotalPinRegistradoValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//                        //if (param.pinSolicitado == 1)
//                        //{
//                        if (cidade.TotalPinSolicitadoQtde > 0)
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", ((DivisaoSegura(cidade.TotalPinRegistradoQtde, (float)cidade.TotalPinSolicitadoQtde)) * 100).ToString("0.##") + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        else
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", "0" + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        //}

//                    }

//                    if (param.pinConfirmado == 1)
//                    {
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.TotalPinConfirmadoQtde), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.TotalPinConfirmadoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalPinConfirmadoPct.ToString() + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.TotalPinConfirmadoValor, relatorio.TotalPinConfirmadoValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//                        //if (param.pinRegistrado == 1)
//                        //{
//                        if (cidade.TotalPinRegistradoQtde > 0)
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", ((DivisaoSegura(cidade.TotalPinConfirmadoQtde, (float)cidade.TotalPinRegistradoQtde)) * 100).ToString("0.##") + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        else
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", "0" + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        //}

//                    }

//                    if (param.pinNecessidadeVistoria == 1)
//                    {
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.TotalPinPendenteVistQtde), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.TotalPinPendenteVistValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalPinPendenteVistPct.ToString() + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.TotalPinPendenteVistValor, relatorio.TotalPinPendenteVistValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//                        // (param.pinConfirmado == 1)
//                        //{
//                        if (cidade.TotalPinConfirmadoQtde > 0)
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", ((DivisaoSegura(cidade.TotalPinPendenteVistQtde, (float)cidade.TotalPinConfirmadoQtde)) * 100).ToString("0.##") + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        else
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", "0" + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        //}

//                    }

//                    if (param.pinVistoriado == 1)
//                    {
//						//colunaIndex = colunaIndex + 21;
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.CanalAzulVistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.CanalAzulVistoriadoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalAzulPct + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.CanalAzulVistoriadoValor, relatorio.TotalCanalAzulValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.CanalVerdeVistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.CanalVerdeVistoriadoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalVerdePct + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.CanalVerdeVistoriadoValor, relatorio.TotalCanalVerdeValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.CanalVermelhoVistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.CanalVermelhoVistoriadoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalVermelhoPct + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.CanalVermelhoVistoriadoValor, relatorio.TotalCanalVermelhoValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.CanalCinzaVistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.CanalCinzaVistoriadoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalCinzaPct + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.CanalCinzaVistoriadoValor, relatorio.TotalCanalCinzaValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);


//						//willy //total
//						int totalCanaisEstado = cidade.CanalCinzaVistoriado + cidade.CanalVermelhoVistoriado + cidade.CanalVerdeVistoriado + cidade.CanalAzulVistoriado;
//						decimal totalValorEstado = cidade.CanalCinzaVistoriadoValor + cidade.CanalVermelhoVistoriadoValor + cidade.CanalVerdeVistoriadoValor + cidade.CanalAzulVistoriadoValor;


//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(totalCanaisEstado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", totalValorEstado, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalAzulPct + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalAzulPct + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.CanalAzulVistoriadoValor, relatorio.TotalCanalAzulValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);




//						/*
//						AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.TotalPinVistoriadoQtde), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.TotalPinVistoriadoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, cidade.TotalPinVistoriadoPct.ToString() + "%", 0, cidade.destaque, cidadePinsValoresFontColor);

//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], cidadePinsValoresFontColorBack, ((DivisaoSegura(cidade.TotalPinVistoriadoValor, relatorio.TotalPinVistoriadoValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadePinsValoresFontColor);
                       
//                        if (cidade.TotalPinConfirmadoQtde > 0)
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", ((DivisaoSegura(cidade.TotalPinVistoriadoQtde, (float)cidade.TotalPinConfirmadoQtde)) * 100).ToString("0.##") + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        else
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], "#404040", "0" + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        */
//					}

//					if (param.pinInternado == 1)
//                    {
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, FormatarQuantidade(cidade.TotalPinInternadoQtde), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex++], color, "", cidade.TotalPinInternadoValor, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//                        //AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex], "#404040", cidade.TotalPinInternadoPct.ToString() + "%", 0, cidade.destaque, "#F4B084");

//                        //if (param.pinVistoriado == 1)
//                        //{
//                        if (cidade.TotalPinVistoriadoQtde > 0)
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex], "#404040", ((DivisaoSegura(cidade.TotalPinInternadoQtde, (float)cidade.TotalPinVistoriadoQtde)) * 100).ToString("0.##") + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        else
//                        {
//                            AplicarFormatacaoConsolidado(worksheet, startRow, rowCount, colunas[colunaIndex], "#404040", "0" + "%", 0, cidade.destaque, "#F4B084");
//                        }
//                        //}

//                    }

//                    rowCount = rowCount + (1 * rowSize);
//                }

//                startRow = startRow + (unidade.Cidades.Count * rowSize);
//            }

//            #region Relatorio Detalhado

//            var _posstartRow = startRow;

//            string[] _nameColunas;
//            string _title;
//            string _corRel;

//			var inicio = param.dataInicio.Month;
//			var fim = param.dataFim.Month;

//			int resultadoTipoPeriodo = AplicarRegrasPeriodo(param.dataInicio, param.dataFim);
//			if(resultadoTipoPeriodo == (int)EnumRegrasFiltro.DIA)
//			{
//				inicio = param.dataInicio.Day;
//				fim = param.dataFim.Day;
//				_meses = dias;
//			}
//			else if (resultadoTipoPeriodo == (int)EnumRegrasFiltro.HORA)
//			{
//				inicio = 1;
//				fim = 23;
//				_meses = horas;
//			}

//			if (relatorio.DetalheNFEImportado.Count > 0)
//            {
//				var startColumn = 3;
//				_colunasIndex = 0;
//				startRow = startRow + 3;

//				for (int i = inicio; i <= fim; i++)
//				{
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, 1, "#375623", "NFE IMPORTADA");
//					//AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, 1, "#CCCCCC", "GERAL SUFRAMA");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, 1, "#262626", "GERAL-SUFRAMA", 0, "#FFFFFF", ExcelHorizontalAlignment.Center);

//					worksheet.Cells[startRow, startColumn, startRow, startColumn + 3].Merge = true;
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, startColumn, "#375623", _meses[i]);
//					//startRow = startRow + 1;
//					if(i == 1)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde01), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor01, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if(i == 2)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde02), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor02, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 3)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde03), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor03, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 4)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde04), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor04, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 5)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde05), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor05, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 6)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde06), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor06, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 7)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde07), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor07, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 8)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde08), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor08, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 9)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde09), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor09, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 10)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde10), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor10, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 11)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde11), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor11, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 12)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde12), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor12, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 13)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde13), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor13, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 14)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde14), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor14, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}

//					else if (i == 15)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde15), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor15, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 16)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde16), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor16, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 17)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde17), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor17, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 18)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde18), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor18, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 19)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde19), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor19, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 20)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde20), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor20, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 21)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde21), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor21, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 22)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde22), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor22, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 23)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde23), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor23, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 24)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde24), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor24, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 25)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde25), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor25, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 26)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde26), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor26, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 27)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde27), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor27, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 28)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde28), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor28, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 29)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde29), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor29, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 30)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde30), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor30, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 31)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNfeQtde31), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNfeValor31, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% PIN");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% VALORES");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					startColumn++;
//				}
				
//				startColumn = 3;
//				startRow = startRow + 2;
//				foreach (var und in relatorio.UnidadesDetalhamentoImportado)
//				{
//					startColumn = 3;

//					foreach (var cidade in und.CidadesDetalhamentoGenerico)
//					{
//						string color = cidade.destaque == true ? "#C6E0B4" : "#FFFFFF";
//						string cidadeFontColor = "#000000";
//						string cidadePinsValoresFontColor = "#000000";
//						string cidadePinsValoresFontColorBack = color;
//						if (cidade.destaque && cidade.totalizadorEstado && color == "#C6E0B4")
//						{
//							color = "#595959";
//							cidadeFontColor = "#FFFFFF";
//							cidadePinsValoresFontColor = "#F4B084";
//							cidadePinsValoresFontColorBack = "#404040";
//						}
//						else
//						{
							
//						}

//						if (cidade.MunicipioUfUc)
//						{
//							color = "#DDEBF7";
//							cidadeFontColor = "#7030A0";
//							cidadePinsValoresFontColor = "#7030A0";
//							cidadePinsValoresFontColorBack = "#DDEBF7";
//						}
//						else if (cidade.MunicipioOutraUf)
//						{
//							cidade.destaque = true;
//							color = "#FFFFFF";
//							cidadeFontColor = "#0070C0";
//						}

//						startRow = startRow + 1;
//						startColumn = 2;

//						AplicarFormatacaoConsolidadoCidade(worksheet, startRow, 0, 0, "A", color, cidade.Nome, cidade.destaque, cidadeFontColor, cidade.MunicipioOutraUf, cidade.totalizadorEstado);
//						if (1 >= inicio && 1 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde01), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor01), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (2 >= inicio && 2 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde02), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor02), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin02, 0,  cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (3 >= inicio && 3 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde03), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor03), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin03, 0,  cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (4 >= inicio && 4 <= fim)
//						{
//							startColumn++;
//						AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde04), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor04), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}

//						if (5 >= inicio && 5 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde05), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor05), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (6 >= inicio && 6 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde06), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor06), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (7 >= inicio && 7 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde07), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor07), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (8 >= inicio && 8 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde08), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor08), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (9 >= inicio && 9 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde09), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor09), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (10 >= inicio && 10 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde10), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor10), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (11 >= inicio && 11 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde11), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor11), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if(12 >= inicio && 12 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde12), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor12), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (13 >= inicio && 13 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde13), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor13), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (14 >= inicio && 14 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde14), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor14), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}

//						if (15 >= inicio && 15 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde15), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor15), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (16 >= inicio && 16 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde16), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor16), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (17 >= inicio && 17 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde17), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor17), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (18 >= inicio && 18 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde18), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor18), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (19 >= inicio && 19 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde19), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor19), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (20 >= inicio && 20 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde20), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor20), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (21 >= inicio && 21 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde21), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor21), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (22 >= inicio && 22 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde22), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor22), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (23 >= inicio && 23 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde23), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor23), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (24 >= inicio && 24 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde24), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor24), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (25 >= inicio && 25 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde25), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor25), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (26 >= inicio && 26 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde26), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor26), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (27 >= inicio && 27 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde27), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor27), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (28 >= inicio && 28 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde28), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor28), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (29 >= inicio && 29 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde29), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor29), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (30 >= inicio && 30 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde30), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor30), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (31 >= inicio && 31 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde31), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor31), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//					}
//				}
				

//				startRow = startRow + 1;
//				_posstartRow = startRow + 5;
		
//			}

//            if (relatorio.UnidadesDetalhamentoSolicitado.Count > 0)
//            {

//				var startColumn = 3;
//				_colunasIndex = 0;
//				startRow = startRow + 3;

//				for (int i = inicio; i <= fim; i++)
//				{
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, 1, "#375623", "PIN SOLICITADO");
//					//AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, 1, "#CCCCCC", "GERAL SUFRAMA");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, 1, "#262626", "GERAL-SUFRAMA", 0, "#FFFFFF", ExcelHorizontalAlignment.Center);

//					worksheet.Cells[startRow, startColumn, startRow, startColumn + 4].Merge = true;
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, startColumn, "#375623", _meses[i]);
//					//startRow = startRow + 1;
//					if (i == 1)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde01), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor01, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 2)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde02), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor02, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 3)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde03), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor03, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 4)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde04), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor04, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 5)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde05), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor05, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 6)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde06), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor06, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 7)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde07), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor07, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 8)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde08), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor08, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 9)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde09), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor09, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 10)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde10), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor10, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 11)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde11), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor11, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 12)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde12), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor12, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 13)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde13), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor13, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 14)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde14), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor14, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 15)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde15), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor15, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 16)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde16), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor16, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 17)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde17), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor17, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 18)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde18), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor18, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 19)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde19), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor19, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 20)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde20), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor20, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 21)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde21), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor21, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 22)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde22), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor22, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 23)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde23), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor23, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 24)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde24), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor24, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 25)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde25), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor25, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 26)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde26), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor26, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 27)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde27), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor27, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 28)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde28), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor28, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 29)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde29), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor29, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 30)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde30), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor30, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 31)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalSolicitadoQtde31), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalSolicitadoValor31, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% PIN");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% VALORES");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#262626", "% IMPORTAÇÃO EFETIVADA",true);
					

//					startColumn++;
//					startColumn++;
//				}

//				startColumn = 3;
//				startRow = startRow + 2;
//				foreach (var und in relatorio.UnidadesDetalhamentoSolicitado)
//				{
//					startColumn = 3;

//					foreach (var cidade in und.CidadesDetalhamentoGenerico)
//					{
//						string color = cidade.destaque == true ? "#C6E0B4" : "#FFFFFF";
//						string cidadeFontColor = "#000000";
//						string cidadePinsValoresFontColor = "#000000";
//						string cidadePinsValoresFontColorBack = color;
//						if (cidade.destaque && cidade.totalizadorEstado && color == "#C6E0B4")
//						{
//							color = "#595959";
//							cidadeFontColor = "#FFFFFF";
//							cidadePinsValoresFontColor = "#F4B084";
//							cidadePinsValoresFontColorBack = "#404040";
//						}
//						else
//						{

//						}

//						if (cidade.MunicipioUfUc)
//						{
//							color = "#DDEBF7";
//							cidadeFontColor = "#7030A0";
//							cidadePinsValoresFontColor = "#7030A0";
//							cidadePinsValoresFontColorBack = "#DDEBF7";
//						}
//						else if (cidade.MunicipioOutraUf)
//						{
//							cidade.destaque = true;
//							color = "#FFFFFF";
//							cidadeFontColor = "#0070C0";
//						}

//						startRow = startRow + 1;
//						startColumn = 2;

//						AplicarFormatacaoConsolidadoCidade(worksheet, startRow, 0, 0, "A", color, cidade.Nome, cidade.destaque, cidadeFontColor, cidade.MunicipioOutraUf, cidade.totalizadorEstado);
//						if (1 >= inicio && 1 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde01), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor01), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (2 >= inicio && 2 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde02), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor02), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (3 >= inicio && 3 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde03), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor03), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (4 >= inicio && 4 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde04), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor04), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//						if (5 >= inicio && 5 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde05), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor05), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (6 >= inicio && 6 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde06), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor06), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (7 >= inicio && 7 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde07), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor07), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (8 >= inicio && 8 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde08), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor08), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (9 >= inicio && 9 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde09), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor09), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (10 >= inicio && 10 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde10), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor10), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (11 >= inicio && 11 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde11), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor11), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (12 >= inicio && 12 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde12), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor12), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (13 >= inicio && 13 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde13), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor13), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (14 >= inicio && 14 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde14), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor14), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (15 >= inicio && 15 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde15), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor15), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (16 >= inicio && 16 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde16), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor16), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (17 >= inicio && 17 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde17), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor17), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (18 >= inicio && 18 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde18), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor18), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (19 >= inicio && 19 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde19), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor19), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (20 >= inicio && 20 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde20), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor20), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (21 >= inicio && 21 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde21), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor21), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (22 >= inicio && 22 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde22), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor22), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (23 >= inicio && 23 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde23), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor23), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (24 >= inicio && 24 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde24), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor24), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (25 >= inicio && 25 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde25), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor25), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (26 >= inicio && 26 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde26), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor26), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (27 >= inicio && 27 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde27), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor27), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (28 >= inicio && 28 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde28), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor28), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (29 >= inicio && 29 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde29), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor29), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (30 >= inicio && 30 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde30), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor30), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (31 >= inicio && 31 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde31), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor31), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//					}
//				}

//			}
//			_posstartRow = startRow + 5;
//			if (relatorio.UnidadesDetalhamentoRegistrado.Count > 0)
//            {
//				var startColumn = 3;
//				_colunasIndex = 0;
//				startRow = startRow + 3;

//				for (int i = inicio; i <= fim; i++)
//				{
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, 1, "#375623", "PIN REGISTRADO");
//					//AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, 1, "#CCCCCC", "GERAL SUFRAMA");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, 1, "#262626", "GERAL-SUFRAMA", 0, "#FFFFFF", ExcelHorizontalAlignment.Center);

//					worksheet.Cells[startRow, startColumn, startRow, startColumn + 4].Merge = true;
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, startColumn, "#375623", _meses[i]);
//					//startRow = startRow + 1;
//					if (i == 1)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde01), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor01, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 2)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde02), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor02, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 3)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde03), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor03, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 4)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde04), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor04, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 5)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde05), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor05, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 6)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde06), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor06, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 7)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde07), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor07, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 8)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde08), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor08, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 9)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde09), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor09, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 10)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde10), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor10, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 11)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde11), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor11, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 12)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde12), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor12, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 13)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde13), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor13, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 14)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde14), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor14, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 15)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde15), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor15, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 16)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde16), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor16, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 17)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde17), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor17, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 18)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde18), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor18, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 19)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde19), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor19, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 20)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde20), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor20, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 21)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde21), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor21, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 22)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde22), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor22, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 23)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde23), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor23, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 24)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde24), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor24, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 25)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde25), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor25, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 26)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde26), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor26, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 27)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde27), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor27, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 28)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde28), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor28, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 29)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde29), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor29, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 30)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde30), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor30, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 31)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalRegistradoQtde31), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalRegistradoValor31, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% PIN");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% VALORES");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#262626", "% PIN EFETIVADO", true);


//					startColumn++;
//					startColumn++;
//				}
//				startColumn = 3;
//				startRow = startRow + 2;

//				foreach (var und in relatorio.UnidadesDetalhamentoRegistrado)
//				{
//					startColumn = 3;

//					foreach (var cidade in und.CidadesDetalhamentoGenerico)
//					{
//						string color = cidade.destaque == true ? "#C6E0B4" : "#FFFFFF";
//						string cidadeFontColor = "#000000";
//						string cidadePinsValoresFontColor = "#000000";
//						string cidadePinsValoresFontColorBack = color;
//						if (cidade.destaque && cidade.totalizadorEstado && color == "#C6E0B4")
//						{
//							color = "#595959";
//							cidadeFontColor = "#FFFFFF";
//							cidadePinsValoresFontColor = "#F4B084";
//							cidadePinsValoresFontColorBack = "#404040";
//						}
//						else
//						{

//						}

//						if (cidade.MunicipioUfUc)
//						{
//							color = "#DDEBF7";
//							cidadeFontColor = "#7030A0";
//							cidadePinsValoresFontColor = "#7030A0";
//							cidadePinsValoresFontColorBack = "#DDEBF7";
//						}
//						else if (cidade.MunicipioOutraUf)
//						{
//							cidade.destaque = true;
//							color = "#FFFFFF";
//							cidadeFontColor = "#0070C0";
//						}

//						startRow = startRow + 1;
//						startColumn = 2;

//						AplicarFormatacaoConsolidadoCidade(worksheet, startRow, 0, 0, "A", color, cidade.Nome, cidade.destaque, cidadeFontColor, cidade.MunicipioOutraUf, cidade.totalizadorEstado);
//						if (1 >= inicio && 1 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde01), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor01), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (2 >= inicio && 2 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde02), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor02), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (3 >= inicio && 3 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde03), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor03), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (4 >= inicio && 4 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde04), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor04), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//						if (5 >= inicio && 5 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde05), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor05), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (6 >= inicio && 6 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde06), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor06), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (7 >= inicio && 7 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde07), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor07), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (8 >= inicio && 8 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde08), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor08), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (9 >= inicio && 9 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde09), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor09), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (10 >= inicio && 10 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde10), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor10), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (11 >= inicio && 11 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde11), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor11), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (12 >= inicio && 12 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde12), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor12), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (13 >= inicio && 13 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde13), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor13), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (14 >= inicio && 14 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde14), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor14), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (15 >= inicio && 15 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde15), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor15), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (16 >= inicio && 16 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde16), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor16), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (17 >= inicio && 17 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde17), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor17), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (18 >= inicio && 18 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde18), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor18), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (19 >= inicio && 19 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde19), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor19), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (20 >= inicio && 20 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde20), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor20), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (21 >= inicio && 21 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde21), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor21), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (22 >= inicio && 22 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde22), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor22), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (23 >= inicio && 23 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde23), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor23), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (24 >= inicio && 24 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde24), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor24), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (25 >= inicio && 25 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde25), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor25), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (26 >= inicio && 26 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde26), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor26), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (27 >= inicio && 27 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde27), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor27), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (28 >= inicio && 28 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde28), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor28), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (29 >= inicio && 29 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde29), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor29), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (30 >= inicio && 30 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde30), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor30), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (31 >= inicio && 31 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde31), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor31), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//					}
//				}
//			}

//            if (relatorio.UnidadesDetalhamentoConfirmado.Count > 0)
//            {
//				var startColumn = 3;
//				_colunasIndex = 0;
//				startRow = startRow + 3;

//				for (int i = inicio; i <= fim; i++)
//				{
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, 1, "#375623", "PIN CONF. RECEBIMENTO MERCADORIA");
//					//AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, 1, "#CCCCCC", "GERAL SUFRAMA");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, 1, "#262626", "GERAL-SUFRAMA", 0, "#FFFFFF", ExcelHorizontalAlignment.Center);

//					worksheet.Cells[startRow, startColumn, startRow, startColumn + 4].Merge = true;
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, startColumn, "#375623", _meses[i]);
//					//startRow = startRow + 1;
//					if (i == 1)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde01), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor01, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 2)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde02), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor02, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 3)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde03), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor03, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 4)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde04), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor04, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 5)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde05), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor05, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 6)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde06), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor06, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 7)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde07), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor07, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 8)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde08), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor08, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 9)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde09), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor09, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 10)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde10), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor10, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 11)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde11), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor11, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 12)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde12), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor12, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 13)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde13), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor13, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 14)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde14), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor14, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 15)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde15), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor15, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 16)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde16), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor16, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 17)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde17), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor17, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 18)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde18), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor18, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 19)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde19), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor19, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 20)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde20), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor20, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 21)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde21), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor21, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 22)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde22), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor22, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 23)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde23), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor23, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 24)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde24), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor24, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 25)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde25), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor25, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 26)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde26), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor26, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 27)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde27), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor27, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 28)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde28), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor28, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 29)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde29), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor29, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 30)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde30), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor30, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 31)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalConfirmadoQtde31), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalConfirmadoValor31, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% PIN");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% VALORES");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#262626", "% PIN DISPONIBILIZADO PARA VISTORIA", true);


//					startColumn++;
//					startColumn++;
//				}
//				startColumn = 3;
//				startRow = startRow + 2;

//				foreach (var und in relatorio.UnidadesDetalhamentoConfirmado)
//				{
//					startColumn = 3;

//					foreach (var cidade in und.CidadesDetalhamentoGenerico)
//					{
//						string color = cidade.destaque == true ? "#C6E0B4" : "#FFFFFF";
//						string cidadeFontColor = "#000000";
//						string cidadePinsValoresFontColor = "#000000";
//						string cidadePinsValoresFontColorBack = color;
//						if (cidade.destaque && cidade.totalizadorEstado && color == "#C6E0B4")
//						{
//							color = "#595959";
//							cidadeFontColor = "#FFFFFF";
//							cidadePinsValoresFontColor = "#F4B084";
//							cidadePinsValoresFontColorBack = "#404040";
//						}
//						else
//						{

//						}

//						if (cidade.MunicipioUfUc)
//						{
//							color = "#DDEBF7";
//							cidadeFontColor = "#7030A0";
//							cidadePinsValoresFontColor = "#7030A0";
//							cidadePinsValoresFontColorBack = "#DDEBF7";
//						}
//						else if (cidade.MunicipioOutraUf)
//						{
//							cidade.destaque = true;
//							color = "#FFFFFF";
//							cidadeFontColor = "#0070C0";
//						}

//						startRow = startRow + 1;
//						startColumn = 2;

//						AplicarFormatacaoConsolidadoCidade(worksheet, startRow, 0, 0, "A", color, cidade.Nome, cidade.destaque, cidadeFontColor, cidade.MunicipioOutraUf, cidade.totalizadorEstado);
//						if (1 >= inicio && 1 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde01), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor01), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (2 >= inicio && 2 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde02), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor02), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (3 >= inicio && 3 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde03), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor03), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (4 >= inicio && 4 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde04), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor04), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//						if (5 >= inicio && 5 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde05), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor05), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (6 >= inicio && 6 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde06), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor06), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (7 >= inicio && 7 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde07), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor07), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (8 >= inicio && 8 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde08), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor08), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (9 >= inicio && 9 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde09), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor09), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (10 >= inicio && 10 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde10), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor10), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (11 >= inicio && 11 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde11), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor11), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (12 >= inicio && 12 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde12), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor12), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (13 >= inicio && 13 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde13), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor13), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (14 >= inicio && 14 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde14), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor14), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						//
//						if (15 >= inicio && 15 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde15), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor15), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (16 >= inicio && 16 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde16), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor16), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (17 >= inicio && 17 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde17), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor17), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (18 >= inicio && 18 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde18), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor18), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (19 >= inicio && 19 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde19), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor19), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (20 >= inicio && 20 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde20), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor20), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (21 >= inicio && 21 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde21), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor21), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (22 >= inicio && 22 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde22), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor22), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (23 >= inicio && 23 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde23), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor23), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (24 >= inicio && 24 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde24), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor24), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (25 >= inicio && 25 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde25), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor25), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (26 >= inicio && 26 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde26), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor26), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (27 >= inicio && 27 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde27), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor27), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (28 >= inicio && 28 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde28), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor28), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (29 >= inicio && 29 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde29), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor29), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (30 >= inicio && 30 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde30), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor30), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (31 >= inicio && 31 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde31), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor31), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//					}
//				}
//			}

//            if (relatorio.UnidadesDetalhamentoNecessidadeVistoria.Count > 0)
//            {
//				var startColumn = 3;
//				_colunasIndex = 0;
//				startRow = startRow + 3;

//				for (int i = inicio; i <= fim; i++)
//				{
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, 1, "#375623", "PIN NECESSIDADE DE VISTORIA");
//					//AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, 1, "#CCCCCC", "GERAL SUFRAMA");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, 1, "#262626", "GERAL-SUFRAMA", 0, "#FFFFFF", ExcelHorizontalAlignment.Center);

//					worksheet.Cells[startRow, startColumn, startRow, startColumn + 4].Merge = true;
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, startColumn, "#375623", _meses[i]);
//					//startRow = startRow + 1;
//					if (i == 1)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde01), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor01, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 2)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde02), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor02, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 3)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde03), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor03, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 4)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde04), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor04, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 5)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde05), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor05, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 6)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde06), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor06, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 7)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde07), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor07, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 8)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde08), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor08, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 9)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde09), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor09, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 10)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde10), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor10, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 11)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde11), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor11, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 12)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde12), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor12, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 13)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde13), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor13, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 14)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde14), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor14, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 15)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde15), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor15, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 16)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde16), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor16, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 17)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde17), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor17, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 18)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde18), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor18, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 19)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde19), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor19, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 20)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde20), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor20, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 21)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde21), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor21, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 22)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde22), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor22, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 23)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde23), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor23, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 24)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde24), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor24, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 25)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde25), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor25, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 26)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde26), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor26, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 27)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde27), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor27, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 28)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde28), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor28, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 29)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde29), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor29, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 30)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde30), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor30, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 31)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalNecessidadeVistoriaQtde31), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalNecessidadeVistoriaValor31, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% PIN");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% VALORES");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#262626", "% PIN SELECIONADO DE VISTORIA", true);


//					startColumn++;
//					startColumn++;
//				}
//				startColumn = 3;
//				startRow = startRow + 2;

//				foreach (var und in relatorio.UnidadesDetalhamentoNecessidadeVistoria)
//				{
//					startColumn = 3;

//					foreach (var cidade in und.CidadesDetalhamentoGenerico)
//					{
//						string color = cidade.destaque == true ? "#C6E0B4" : "#FFFFFF";
//						string cidadeFontColor = "#000000";
//						string cidadePinsValoresFontColor = "#000000";
//						string cidadePinsValoresFontColorBack = color;
//						if (cidade.destaque && cidade.totalizadorEstado && color == "#C6E0B4")
//						{
//							color = "#595959";
//							cidadeFontColor = "#FFFFFF";
//							cidadePinsValoresFontColor = "#F4B084";
//							cidadePinsValoresFontColorBack = "#404040";
//						}
//						else
//						{

//						}

//						if (cidade.MunicipioUfUc)
//						{
//							color = "#DDEBF7";
//							cidadeFontColor = "#7030A0";
//							cidadePinsValoresFontColor = "#7030A0";
//							cidadePinsValoresFontColorBack = "#DDEBF7";
//						}
//						else if (cidade.MunicipioOutraUf)
//						{
//							cidade.destaque = true;
//							color = "#FFFFFF";
//							cidadeFontColor = "#0070C0";
//						}

//						startRow = startRow + 1;
//						startColumn = 2;

//						AplicarFormatacaoConsolidadoCidade(worksheet, startRow, 0, 0, "A", color, cidade.Nome, cidade.destaque, cidadeFontColor, cidade.MunicipioOutraUf, cidade.totalizadorEstado);
//						if (1 >= inicio && 1 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde01), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor01), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (2 >= inicio && 2 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde02), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor02), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (3 >= inicio && 3 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde03), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor03), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (4 >= inicio && 4 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde04), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor04), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//						if (5 >= inicio && 5 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde05), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor05), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (6 >= inicio && 6 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde06), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor06), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (7 >= inicio && 7 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde07), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor07), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (8 >= inicio && 8 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde08), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor08), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (9 >= inicio && 9 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde09), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor09), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (10 >= inicio && 10 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde10), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor10), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (11 >= inicio && 11 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde11), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor11), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}
//						if (12 >= inicio && 12 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde12), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor12), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//						}

//						if (13 >= inicio && 13 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde13), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor13), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (14 >= inicio && 14 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde14), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor14), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (15 >= inicio && 15 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde15), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor15), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (16 >= inicio && 16 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde16), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor16), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (17 >= inicio && 17 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde17), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor17), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (18 >= inicio && 18 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde18), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor18), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (19 >= inicio && 19 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde19), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor19), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (20 >= inicio && 20 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde20), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor20), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (21 >= inicio && 21 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde21), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor21), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (22 >= inicio && 22 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde22), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor22), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (23 >= inicio && 23 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde23), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor23), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (24 >= inicio && 24 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde24), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor24), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (25 >= inicio && 25 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde25), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor25), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (26 >= inicio && 26 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde26), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor26), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (27 >= inicio && 27 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde27), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor27), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (28 >= inicio && 28 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde28), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor28), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (29 >= inicio && 29 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde29), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor29), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (30 >= inicio && 30 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde30), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor30), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (31 >= inicio && 31 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde31), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor31), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//					}
//				}
//			}

//			_posstartRow = startRow + 5;
//			if (relatorio.VistoriaDetalhada.Count > 1)
//			{
//				_nameColunas = new string[] { "PIN VISTORIADO (F)", "VISTORIADO", "REVISTORIADO", "EXTERNO", "POSTO SUFRAMA", "DEFERIDO", "INDEFERIDO", "TOTAL" };
//				_title = "VISTORIAS - DETALHAMENTO NO PERÍODO";
//				_corRel = "#228B22";
//				//worksheet.Cells["A841:O844"].Copy(worksheet.Cells["A811:O814"]);
//				_posstartRow = PrepararTemplateDetalhadoVistoria(worksheet, _posstartRow, relatorio, _title, _corRel, param, _nameColunas);
//				//worksheet.Cells["A841:O844"].Clear();
//				//_posstartRow = SelecionarRelatorioDetalhadoVistoria(worksheet, _posstartRow, relatorio.VistoriaDetalhada, _title, _corRel, param, _nameColunas);
//				startRow = _posstartRow + 5;
//			}
			
//			if (relatorio.UnidadesDetalhamentoInternado.Count > 0)
//            {
//				var startColumn = 3;
//				_colunasIndex = 0;
//				startRow = startRow + 10;

//				for (int i = inicio; i <= fim; i++)
//				{
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, 1, "#375623", "PIN INTERNADO");
//					//AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, 1, "#CCCCCC", "GERAL SUFRAMA");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, 1, "#262626", "GERAL-SUFRAMA", 0, "#FFFFFF", ExcelHorizontalAlignment.Center);

//					worksheet.Cells[startRow, startColumn, startRow, startColumn + 4].Merge = true;
//					AplicarFormatacaoTituloConsolidadoTopo2(worksheet, startRow, startColumn, "#375623", _meses[i]);
//					//startRow = startRow + 1;
//					if (i == 1)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde01), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor01, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 2)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde02), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor02, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 3)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde03), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor03, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 4)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde04), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor04, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 5)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde05), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor05, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 6)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde06), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor06, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 7)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde07), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor07, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 8)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde08), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor08, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 9)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde09), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor09, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 10)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde10), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor10, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 11)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde11), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor11, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 12)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde12), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor12, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 13)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde13), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor13, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 14)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde14), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor14, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 15)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde15), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor15, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 16)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde16), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor16, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 17)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde17), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor17, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 18)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde18), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor18, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 19)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde19), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor19, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 20)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde20), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor20, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 21)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde21), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor21, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 22)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde22), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor22, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 23)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde23), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor23, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 24)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde24), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor24, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 25)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde25), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor25, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 26)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde26), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor26, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 27)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde27), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor27, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 28)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde28), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor28, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 29)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde29), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor29, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 30)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde30), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor30, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					else if (i == 31)
//					{
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "Quant");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", FormatarQuantidade(relatorio.TotalInternadoQtde31), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//						AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "R$");
//						AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", "", relatorio.TotalInternadoValor31, "#FFFFFF", ExcelHorizontalAlignment.Right);
//					}
//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% PIN");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#CCCCCC", "% VALORES");
//					AplicarCorGeralSuframa(worksheet, startRow + 2, 0, startColumn++, "#262626", relatorio.TotalNfePct.ToString(), 0, "#FFC000");

//					AplicarFormatacaoTituloConsolidado(worksheet, startRow + 1, 0, startColumn, "#262626", "% PIN INTERNADO X REGISTRADOS", true);


//					startColumn++;
//					startColumn++;
//				}
//				startColumn = 3;
//				startRow = startRow + 2;

//				foreach (var und in relatorio.UnidadesDetalhamentoRegistrado)
//				{
//					startColumn = 3;

//					foreach (var cidade in und.CidadesDetalhamentoGenerico)
//					{
//						string color = cidade.destaque == true ? "#C6E0B4" : "#FFFFFF";
//						string cidadeFontColor = "#000000";
//						string cidadePinsValoresFontColor = "#000000";
//						string cidadePinsValoresFontColorBack = color;
//						if (cidade.destaque && cidade.totalizadorEstado && color == "#C6E0B4")
//						{
//							color = "#595959";
//							cidadeFontColor = "#FFFFFF";
//							cidadePinsValoresFontColor = "#F4B084";
//							cidadePinsValoresFontColorBack = "#404040";
//						}
//						else
//						{

//						}

//						if (cidade.MunicipioUfUc)
//						{
//							color = "#DDEBF7";
//							cidadeFontColor = "#7030A0";
//							cidadePinsValoresFontColor = "#7030A0";
//							cidadePinsValoresFontColorBack = "#DDEBF7";
//						}
//						else if (cidade.MunicipioOutraUf)
//						{
//							cidade.destaque = true;
//							color = "#FFFFFF";
//							cidadeFontColor = "#0070C0";
//						}

//						startRow = startRow + 1;
//						startColumn = 2;

//						AplicarFormatacaoConsolidadoCidade(worksheet, startRow, 0, 0, "A", color, cidade.Nome, cidade.destaque, cidadeFontColor, cidade.MunicipioOutraUf, cidade.totalizadorEstado);
//						if (1 >= inicio && 1 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde01), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor01), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador01, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (2 >= inicio && 2 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde02), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor02), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador02, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (3 >= inicio && 3 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde03), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor03), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador03, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (4 >= inicio && 4 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde04), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor04), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador04, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//						if (5 >= inicio && 5 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde05), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor05), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador05, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (6 >= inicio && 6 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde06), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor06), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador06, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (7 >= inicio && 7 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde07), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor07), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador07, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (8 >= inicio && 8 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde08), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor08), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador08, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (9 >= inicio && 9 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde09), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor09), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador09, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (10 >= inicio && 10 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde10), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor10), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador10, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (11 >= inicio && 11 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde11), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor11), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador11, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (12 >= inicio && 12 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde12), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor12), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador12, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//						if (13 >= inicio && 13 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde13), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor13), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador13, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (14 >= inicio && 14 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde14), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor14), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador14, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (15 >= inicio && 15 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde15), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor15), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador15, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (16 >= inicio && 16 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde16), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor16), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador16, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (17 >= inicio && 17 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde17), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor17), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador17, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (18 >= inicio && 18 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde18), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor18), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador18, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (19 >= inicio && 19 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde19), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor19), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador19, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (20 >= inicio && 20 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde20), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor20), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador20, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (21 >= inicio && 21 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde21), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor21), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador21, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (22 >= inicio && 22 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde22), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor22), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador22, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (23 >= inicio && 23 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde23), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor23), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador23, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (24 >= inicio && 24 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde24), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor24), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador24, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (25 >= inicio && 25 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde25), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor25), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador25, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (26 >= inicio && 26 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde26), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor26), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador26, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (27 >= inicio && 27 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde27), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor27), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador27, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (28 >= inicio && 28 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde28), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor28), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador28, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (29 >= inicio && 29 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde29), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor29), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador29, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (30 >= inicio && 30 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde30), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor30), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador30, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}
//						if (31 >= inicio && 31 <= fim)
//						{
//							startColumn++;
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, FormatarQuantidade(cidade.TotalQtde31), 0, false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, "", Convert.ToDecimal(cidade.TotalValor31), false, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctPin31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctValor31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//							AplicarFormatacaoConsolidado(worksheet, startRow, 0, startColumn++, color, cidade.TotalPctIndicador31, 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//						}

//					}
//				}
//			}


//            #endregion

//            byte[] reportBytes;

//            reportBytes = pck.GetAsByteArray();


//            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
//            MemoryStream ms = new MemoryStream(reportBytes);
//            result.Content = new StreamContent(ms);

//            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
//            {
//                FileName = "RelatorioSituacaoPIN.xlsx"
//            };

//            result.Content.Headers.ContentType =
//                new MediaTypeHeaderValue("application/octet-stream");
//            return result;

//        }

//		private int SelecionarRelatorioDetalhado2(ExcelWorksheet _worksheet, int _startRow,
//												List<RelatorioGerencialDetalhadoDto> _listDetails,
//												string _titleRelatorio, string _formatCorRelatorio,
//												RelatorioGerencialVM param, string[] _nameColunas)
//		{
//			int _retstartRow = 0;

//			int tipoRegra = AplicarRegrasPeriodo(param.dataInicio, param.dataFim);

//			switch (tipoRegra)
//			{
//				case (int)EnumRegrasFiltro.MES:
//					_retstartRow = RelatorioDetalhadoMES(_worksheet, _startRow, _listDetails,
//														_titleRelatorio,
//														_formatCorRelatorio, _nameColunas);
//					break;
//				case (int)EnumRegrasFiltro.DIA:
//					_retstartRow = RelatorioDetalhadoDIA(_worksheet, _startRow, _listDetails,
//														_titleRelatorio,
//														_formatCorRelatorio, _nameColunas);

//					break;
//				case (int)EnumRegrasFiltro.HORA:
//					_retstartRow = RelatorioDetalhadoHORA(_worksheet, _startRow, _listDetails,
//														_titleRelatorio,
//														_formatCorRelatorio, _nameColunas);
//					break;
//				default:
//					break;
//			}

//			return _retstartRow;

//		}

//		private int SelecionarRelatorioDetalhado(ExcelWorksheet _worksheet, int _startRow,
//                                                List<RelatorioGerencialDetalhadoDto> _listDetails,
//                                                string _titleRelatorio, string _formatCorRelatorio,
//                                                RelatorioGerencialVM param, string[] _nameColunas)
//        {
//            int _retstartRow = 0;

//            int tipoRegra = AplicarRegrasPeriodo(param.dataInicio, param.dataFim);

//            switch (tipoRegra)
//            {
//                case (int)EnumRegrasFiltro.MES:
//                    _retstartRow = RelatorioDetalhadoMES(_worksheet, _startRow, _listDetails,
//                                                        _titleRelatorio,
//                                                        _formatCorRelatorio, _nameColunas);
//                    break;
//                case (int)EnumRegrasFiltro.DIA:
//                    _retstartRow = RelatorioDetalhadoDIA(_worksheet, _startRow, _listDetails,
//                                                        _titleRelatorio,
//                                                        _formatCorRelatorio, _nameColunas);

//                    break;
//                case (int)EnumRegrasFiltro.HORA:
//                    _retstartRow = RelatorioDetalhadoHORA(_worksheet, _startRow, _listDetails,
//                                                        _titleRelatorio,
//                                                        _formatCorRelatorio, _nameColunas);
//                    break;
//                default:
//                    break;
//            }

//            return _retstartRow;

//        }

//		private int PrepararTemplateDetalhadoVistoria(ExcelWorksheet _worksheet, int _startRow,
//										RelatorioGerencialDto relatorio,
//										string _titleRelatorio, string _formatCorRelatorio,
//										RelatorioGerencialVM param, string[] _nameColunas)
//		{
//			 int tipoRegra = AplicarRegrasPeriodo(param.dataInicio, param.dataFim);
//			string[] cells = { "V2241:AL2243", "AN2241:BE2243", "BF2241:BV2243", "BX2241:CN2243", "CP2241:DF2243", "DH2241:DX2243", "DZ2241:EP2243", "ER2241:FH2243", "FJ2241:FZ2243", "GB2241:GR2243", "GT2241:HJ2243", "HL2241:IB2243", "ID2241:IT2243", "IV2241:JL2243", "JN2241:KD2243", "KF2241:KV2243", "KX2241:LN2243", "LP2241:MF2243", "MH2241:MX2243", "MZ2241:NP2243", "NR2241:OH2243", "OJ2241:OZ2243", "PB2241:PR2243", "PT2241:QJ2243", "QL2241:RB2243", "RD2241:RT2243", "RV2241:SL2243", "SN2241:TDP2243", "TF2241:TV2243", "TX2241:UN2243" }; //, "HL2241:IB2243" 
//			var fim = param.dataFim.Month;
//			var inicio = param.dataInicio.Month;
			

//			switch (tipoRegra)
//			{
//				case (int)EnumRegrasFiltro.MES:

//					break;
//				case (int)EnumRegrasFiltro.DIA:
//					 fim = param.dataFim.Day;
//					 inicio = param.dataInicio.Day;

//					break;
//				case (int)EnumRegrasFiltro.HORA:
//					 fim = param.dataFim.Hour;
//					 inicio = param.dataInicio.Hour;
//					break;
//				default:
//					break;
//			}
//			var diferenca = fim - inicio;
//			int columnMonth = 4;
//			for (int i = 0; i < cells.Length && i < diferenca; i++)
//			{

//				//_worksheet.Cells["D2241:T2243"].Copy(_worksheet.Cells[cells[i]]);
//			}

//			for (int i = inicio; i <= fim; i++)
//			{
//				//_worksheet.Cells[2241, columnMonth].Value = _meses[i];
//				//columnMonth += 18;
//			}
//			_startRow = _startRow + 5;
//			var pos = "A" + _startRow + ":" + "BE" + (_startRow + 4);
//			_worksheet.Cells["A2250:BE2256"].Copy(_worksheet.Cells[pos]);
//			_worksheet.Cells["A2250:BE2256"].Clear();

//			var posCelula = 0;

//			_startRow = _startRow +7;
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalAzulVistoriado), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalAzulVistoriadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVerdeVistoriado), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVerdeVistoriadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVerdeRevistoriado), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVerdeRevistoriadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVerdeDeferidoo), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVerdeDeferidooValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVerdeIndeferido), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVerdeIndeferidoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			//
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVermelhoVistoriado), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVermelhoVistoriadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVermelhoRevistoriado), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVermelhoRevistoriadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVermelhoExterno), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVermelhoExternoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVermelhoPosto), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVermelhoPostoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVermelhoDeferidoo), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVermelhoDeferidooValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalVermelhoIndeferido), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalVermelhoIndeferidoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//			//

//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalCinzaVistoriado), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalCinzaVistoriadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalCinzaRevistoriado), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalCinzaRevistoriadoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalCinzaExterno), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalCinzaExternoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalCinzaPosto), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalCinzaPostoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalCinzaDeferidoo), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalCinzaDeferidooValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(relatorio.CanalCinzaIndeferido), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", relatorio.CanalCinzaIndeferidoValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//			//total
//			int totalRelatorio = relatorio.CanalCinzaVistoriado + relatorio.CanalVermelhoVistoriado + relatorio.CanalVerdeVistoriado + relatorio.CanalAzulVistoriado;
//			decimal totalRelatorioValor = relatorio.CanalCinzaVistoriadoValor + relatorio.CanalVermelhoVistoriadoValor + relatorio.CanalVerdeVistoriadoValor + relatorio.CanalAzulVistoriadoValor;


//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", FormatarQuantidade(totalRelatorio), 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "", totalRelatorioValor, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);
//			AplicarCorGeralSuframa(_worksheet, _startRow - 1, 0, _colunasVist[posCelula++], "#262626", "100%", 0, "#FFFFFF", ExcelHorizontalAlignment.Right);

//			//
//			foreach (var estado in relatorio.UnidadesDetalhamentoVistoria)
//			{
//				foreach(var cidade in estado.CidadesDetalhamentoGenerico)
//				{
//					posCelula = 0;
//					string color = cidade.destaque == true ? "#C6E0B4" : "#FFFFFF";
//					string cidadeFontColor = "#000000";
//					string cidadePinsValoresFontColor = "#000000";
//					string cidadePinsValoresFontColorBack = color;
//					if (cidade.destaque && cidade.totalizadorEstado && color == "#C6E0B4")
//					{
//						color = "#595959";
//						cidadeFontColor = "#FFFFFF";
//						cidadePinsValoresFontColor = "#F4B084";
//						cidadePinsValoresFontColorBack = "#404040";
//					}
//					else
//					{

//					}

//					if (cidade.MunicipioUfUc)
//					{
//						color = "#DDEBF7";
//						cidadeFontColor = "#7030A0";
//						cidadePinsValoresFontColor = "#7030A0";
//						cidadePinsValoresFontColorBack = "#DDEBF7";
//					}
//					else if (cidade.MunicipioOutraUf)
//					{
//						cidade.destaque = true;
//						color = "#FFFFFF";
//						cidadeFontColor = "#0070C0";
//					}
					
//					AplicarFormatacaoConsolidadoCidade(_worksheet, _startRow, 0, 0, "A", color, cidade.Nome, cidade.destaque, cidadeFontColor, cidade.MunicipioOutraUf, cidade.totalizadorEstado);
					
//					//startColumn++;

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalAzulVistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalAzulVistoriadoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalAzulVistoriado, relatorio.CanalAzulVistoriado)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
					
//					//

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVerdeVistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVerdeVistoriadoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVerdeVistoriado, relatorio.CanalAzulVistoriado)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVerdeRevistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVerdeRevistoriadoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVerdeRevistoriado, relatorio.CanalVerdeRevistoriado)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVerdeDeferido), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVerdeDeferidooValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVerdeDeferido, relatorio.CanalVerdeDeferidoo)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVerdeIndeferido), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVerdeIndeferidoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVerdeIndeferido, relatorio.CanalVerdeIndeferido)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					//

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVermelhoVistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVermelhoVistoriadoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVermelhoVistoriado, relatorio.CanalAzulVistoriado)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVermelhoRevistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVermelhoRevistoriadoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVermelhoRevistoriado, relatorio.CanalVermelhoRevistoriado)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVermelhoExterno), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVermelhoExternoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVermelhoExterno, relatorio.CanalVermelhoExterno)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVermelhoPostoSuframa), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVermelhoPostoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVermelhoPostoSuframa, relatorio.CanalVermelhoPosto)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);


//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVermelhoDeferido), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVermelhoDeferidooValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVermelhoDeferido, relatorio.CanalVermelhoDeferidoo)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalVermelhoIndeferido), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalVermelhoIndeferidoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalVermelhoIndeferido, relatorio.CanalVermelhoIndeferido)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					//

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalCinzaVistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalCinzaVistoriadoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalCinzaVistoriado, relatorio.CanalAzulVistoriado)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalCinzaRevistoriado), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalCinzaRevistoriadoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalCinzaRevistoriado, relatorio.CanalCinzaRevistoriado)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalCinzaExterno), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalCinzaExternoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalCinzaExterno, relatorio.CanalCinzaExterno)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalCinzaPostoSuframa), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalCinzaPostoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalCinzaPostoSuframa, relatorio.CanalCinzaPosto)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);


//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalCinzaDeferido), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalCinzaDeferidooValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalCinzaDeferido, relatorio.CanalCinzaDeferidoo)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(cidade.CanalCinzaIndeferido), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(cidade.CanalCinzaIndeferidoValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(cidade.CanalCinzaIndeferido, relatorio.CanalCinzaIndeferido)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);


//					//total
//					//int totalRelatorio = relatorio.CanalCinzaVistoriado + relatorio.CanalVermelhoVistoriado + relatorio.CanalVerdeVistoriado + relatorio.CanalAzulVistoriado;
//					//decimal totalRelatorioValor = relatorio.CanalCinzaVistoriadoValor + relatorio.CanalVermelhoVistoriadoValor + relatorio.CanalVerdeVistoriadoValor + relatorio.CanalAzulVistoriadoValor;
//					int total = cidade.CanalCinzaVistoriado + cidade.CanalVermelhoVistoriado + cidade.CanalVerdeVistoriado + cidade.CanalAzulVistoriado;
//					decimal totalValor = cidade.CanalCinzaVistoriadoValor + cidade.CanalVermelhoVistoriadoValor + cidade.CanalVerdeVistoriadoValor+ cidade.CanalAzulVistoriadoValor;

//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, FormatarQuantidade(total), 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, "", Convert.ToDecimal(totalValor), cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(total, totalRelatorio)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);
//					AplicarFormatacaoConsolidado(_worksheet, _startRow, 0, _colunasVist[posCelula++], color, ((DivisaoSegura(totalValor, totalRelatorioValor)) * 100).ToString("0.##") + "%", 0, cidade.destaque, cidadeFontColor, ExcelHorizontalAlignment.Right);




//					_startRow++;
//				}
//			}

//			//_startRow = _startRow ;
//			foreach (var und in relatorio.UnidadesDetalhamento)
//			{

//				foreach (var cit in und.CidadesDetalhamento)
//				{
//					_startRow = _startRow + 1;
//					posCelula = 0;
						
						
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, 1, "#FFFFFF", cit.Nome, false, false, false);
					
//					//azul
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriadoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalAzulVistoriado01, relatorio.CanalAzulVistoriado)) * 100).ToString("0.##") + "%", false, false, false);
//					//verde
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriadoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVerdeVistoriadoValor01, relatorio.CanalVerdeVistoriado)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriadoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVerdeRevistoriado01, relatorio.CanalVerdeRevistoriado)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferidooValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVerdeDeferido01, relatorio.CanalVerdeDeferidoo)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferidoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVerdeIndeferido01, relatorio.CanalVerdeIndeferido)) * 100).ToString("0.##") + "%", false, false, false);
//					//vermelho
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriadoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVermelhoVistoriado01, relatorio.CanalVermelhoVistoriado)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriadoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVermelhoRevistoriado01, relatorio.CanalVermelhoRevistoriado)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExternoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVermelhoExterno01, relatorio.CanalVermelhoExterno)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVermelhoPostoSuframa01, relatorio.CanalVermelhoPosto)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferidooValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVermelhoDeferido01, relatorio.CanalVermelhoDeferidoo)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferidoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalVermelhoIndeferido01, relatorio.CanalVermelhoIndeferido)) * 100).ToString("0.##") + "%", false, false, false);


//					//cinza
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriadoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalCinzaVistoriado01, relatorio.CanalCinzaVistoriado)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriadoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalCinzaRevistoriado01, relatorio.CanalCinzaRevistoriado)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExternoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalCinzaExterno01, relatorio.CanalCinzaExterno)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalCinzaPostoSuframa01, relatorio.CanalCinzaPosto)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferidooValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalCinzaDeferido01, relatorio.CanalCinzaDeferidoo)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferidoValor01.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(cit.CanalCinzaIndeferido01, relatorio.CanalCinzaIndeferido)) * 100).ToString("0.##") + "%", false, false, false);

//					//total
//					//int totalRelatorio = relatorio.CanalCinzaVistoriado + relatorio.CanalVermelhoVistoriado + relatorio.CanalVerdeVistoriado + relatorio.CanalAzulVistoriado;
//					//decimal totalRelatorioValor = relatorio.CanalCinzaVistoriadoValor + relatorio.CanalVermelhoVistoriadoValor + relatorio.CanalVerdeVistoriadoValor + relatorio.CanalAzulVistoriadoValor;
//					int total = cit.CanalCinzaVistoriado01 + cit.CanalVermelhoVistoriado01 + cit.CanalVerdeVistoriado01 + cit.CanalAzulVistoriado01;
//					decimal totalValor = cit.CanalCinzaVistoriadoValor01 + cit.CanalVermelhoVistoriadoValor01 + cit.CanalVerdeVistoriadoValor01 + cit.CanalAzulVistoriadoValor01;
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", total.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", totalValor.ToString(), false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(total, totalRelatorio)) * 100).ToString("0.##") + "%", false, false, false);
//					AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", ((DivisaoSegura(totalValor, totalRelatorioValor)) * 100).ToString("0.##") + "%", false, false, false);
//					posCelula++;
					
//					/*
//					if (2 >= inicio && 2 <= fim)
//					{
//						if (inicio == 2)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado02.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido02.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido02.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido02.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido02.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado02.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (3 >= inicio && 3 <= fim)
//					{
//						if (inicio == 3)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado03.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido03.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido03.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido03.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido03.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado03.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (4 >= inicio && 4 <= fim)
//					{
//						if (inicio == 4)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado04.ToString(), false, false, false);

//						}

//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido04.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido04.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido04.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido04.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado04.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (5 >= inicio && 5 <= fim)
//					{
//						if (inicio == 5)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado05.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido05.ToString(), false, false, false);
//						//vermelho

//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido05.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido05.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido05.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado05.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (6 >= inicio && 6 <= fim)
//					{
//						if (inicio == 6)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado06.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido06.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido06.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido06.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido06.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado06.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (7 >= inicio && 7 <= fim)
//					{
//						//cinza
//						if (inicio == 7)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado07.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido07.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido07.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido07.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido07.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado07.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (8 >= inicio && 8 <= fim)
//					{
//						if (inicio == 8)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado08.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido08.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido08.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido08.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido08.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado08.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (9 >= inicio && 9 <= fim)
//					{
//						if (inicio == 9)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado09.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido09.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido09.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido09.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido09.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado09.ToString(), false, false, false);
//						posCelula++;
//					}
//					//CINZA
//					if (10 >= inicio && 10 <= fim)
//					{
//						if (inicio == 10)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado10.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido10.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido10.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido10.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido10.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado10.ToString(), false, false, false);
//						posCelula++;
//					}
//					//CINZA
//					if (11 >= inicio && 11 <= fim)
//					{
//						if (inicio == 11)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado11.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido11.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido11.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido11.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido11.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado11.ToString(), false, false, false);
//						posCelula++;
//					}
//					//CINZA
//					if (12 >= inicio && 12 <= fim)
//					{
//						if (inicio == 12)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado12.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido12.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido12.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido12.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido12.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado12.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (13 >= inicio && 13 <= fim)
//					{
//						if (inicio == 13)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado13.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido13.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido13.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido13.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido13.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado13.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (14 >= inicio && 14 <= fim)
//					{
//						if (inicio == 14)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado14.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido14.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido14.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido14.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido14.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado14.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (15 >= inicio && 15 <= fim)
//					{
//						if (inicio == 15)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado15.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido15.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido15.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido15.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido15.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado15.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (16 >= inicio && 16 <= fim)
//					{
//						if (inicio == 16)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado16.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido16.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido16.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido16.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido16.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado16.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (17 >= inicio && 17 <= fim)
//					{
//						if (inicio == 17)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado17.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido17.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido17.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido17.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido17.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado17.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (18 >= inicio && 18 <= fim)
//					{
//						if (inicio == 18)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado18.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido18.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido18.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido18.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido18.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado18.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (19 >= inicio && 19 <= fim)
//					{
//						if (inicio == 19)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado19.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido19.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido19.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido19.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido19.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado19.ToString(), false, false, false);
//						posCelula++;
//					}
//					if (20 >= inicio && 20 <= fim)
//					{
//						if (inicio == 20)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado20.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido20.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido20.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido20.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido20.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado20.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (21 >= inicio && 21 <= fim)
//					{
//						if (inicio == 21)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado21.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido21.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido21.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido21.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido21.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado21.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (22 >= inicio && 22 <= fim)
//					{
//						if (inicio == 22)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado22.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido22.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido22.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido22.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido22.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado22.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (23 >= inicio && 23 <= fim)
//					{
//						if (inicio == 23)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado23.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido23.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido23.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido23.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido23.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado23.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (24 >= inicio && 24 <= fim)
//					{
//						if (inicio == 24)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado24.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido24.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido24.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido24.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido24.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado24.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (25 >= inicio && 25 <= fim)
//					{
//						if (inicio == 25)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado25.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido25.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido25.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido25.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido25.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado25.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (26 >= inicio && 26 <= fim)
//					{
//						if (inicio == 26)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado26.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido26.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido26.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido26.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido26.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado26.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (27 >= inicio && 27 <= fim)
//					{
//						if (inicio == 27)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado27.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido27.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido27.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido27.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido27.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado27.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (28 >= inicio && 28 <= fim)
//					{
//						if (inicio == 28)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado28.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido28.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido28.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido28.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido28.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado28.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (29 >= inicio && 29 <= fim)
//					{
//						if (inicio == 29)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado29.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido29.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido29.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido29.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido29.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado29.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (30 >= inicio && 30 <= fim)
//					{
//						if (inicio == 30)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado30.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido30.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido30.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido30.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido30.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado30.ToString(), false, false, false);
//						posCelula++;
//					}

//					if (31 >= inicio && 31 <= fim)
//					{
//						if (inicio == 31)
//						{
//							AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado31.ToString(), false, false, false);
//						}
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaVistoriado31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaRevistoriado31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaExterno31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaPostoSuframa31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaDeferido31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalCinzaIndeferido31.ToString(), false, false, false);
//						//vermelho
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoVistoriado31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoRevistoriado31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoExterno31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoPostoSuframa31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoDeferido31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVermelhoIndeferido31.ToString(), false, false, false);
//						//verde
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeVistoriado31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeRevistoriado31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeDeferido31.ToString(), false, false, false);
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalVerdeIndeferido31.ToString(), false, false, false);
//						//azul
//						AplicarLinhasDetalhadoColunas2(_worksheet, _startRow, 0, _colunasVist[posCelula++], "#FFFFFF", cit.CanalAzulVistoriado31.ToString(), false, false, false);
//						posCelula++;
//					}
//					*/
//					//_worksheet.Cells["D209:T209"].Merge = true;
//					//AplicarFormatacaoTituloConsolidadoTopo2(_worksheet, 209, 4, "#375623", "FEV");
//					//_worksheet.Cells["D209:T209"].Value = "JANEIRO";

//				}
//			}

//			return _startRow;
//		}

//		private int SelecionarRelatorioDetalhadoVistoria(ExcelWorksheet _worksheet, int _startRow,
//                                        List<RelatorioVistoriaDetalhadaDto> _listDetails,
//                                        string _titleRelatorio, string _formatCorRelatorio,
//                                        RelatorioGerencialVM param, string[] _nameColunas)
//        {
//            int _retstartRow = 0;

//            int tipoRegra = AplicarRegrasPeriodo(param.dataInicio, param.dataFim);

//            switch (tipoRegra)
//            {
//                case (int)EnumRegrasFiltro.MES:
//                    _retstartRow = RelatorioDetalhadoVistoriaMES(_worksheet, _startRow, _listDetails,
//                                                        _titleRelatorio,
//                                                        _formatCorRelatorio, _nameColunas);
//                    break;
//                case (int)EnumRegrasFiltro.DIA:
//                    //_retstartRow = RelatorioDetalhadoDIA(_worksheet, _startRow, _listDetails,
//                    //									_titleRelatorio,
//                    //									_formatCorRelatorio, _nameColunas);

//                    break;
//                case (int)EnumRegrasFiltro.HORA:
//                    //_retstartRow = RelatorioDetalhadoHORA(_worksheet, _startRow, _listDetails,
//                    //									_titleRelatorio,
//                    //									_formatCorRelatorio, _nameColunas);
//                    break;
//                default:
//                    break;
//            }

//            return _retstartRow;

//        }

//        private int RelatorioDetalhadoMES(ExcelWorksheet _worksheet, int _startRow,
//                                                List<RelatorioGerencialDetalhadoDto> _listDetails,
//                                                string _title, string _formatColor, string[] _nameColunas)
//        {
           
//			var _rowSize = 1;
//			_startRow = _startRow + 5;
//			_worksheet.Cells["A" + _startRow.ToString() + ":L" + _startRow.ToString()].Merge = true;

//            AplicarFormatacaoTituloConsolidadoTopo(_worksheet, "A" + _startRow.ToString() + ":L" +
//                                                    _startRow.ToString(), _formatColor, _title + " : " + " MÊS ");

//            _startRow = _startRow + 2;

//            int posicaoCol = 0;
//            int posicaoColMerge = _nameColunas.Length - 1;

//            _startRow = _startRow + 1;
//            string _columnFontColor = "#FFFFFF";

//            //Colunas com o Total Geral Quantidade e Valor
//            int totalGeralQtd = 0;
//            decimal totalGeralValor = 0;
//            int totalTodosQtd = 0;
//            decimal totalTodosValor = 0;

//            string[,] ArrayIndexMesColumns = new string[1000, 4];

//            int contLinha = 0;

//            var listArrayNameColunas = new ArrayList();
//            var listArrayTotal = new ArrayList();
//            int percentColuna = 100;
//			//

//			var agruparMes = _listDetails.GroupBy(g => g.Mes).OrderBy(o => o.Key);

//			foreach (var item in agruparMes)
//            {
//				if (posicaoColMerge <= 4)
//                {
//                    _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                        _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//                    AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                                          _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                                          _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                                          _meses[int.Parse(item.Key)], _formatColor);
//                }
//                else
//                {
//                    posicaoCol = posicaoCol + 1;
//                    posicaoColMerge = posicaoColMerge + 1;

//                    _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                        _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//                    AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                                          _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                                          _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                                          _meses[int.Parse(item.Key)], _formatColor);

//                }

//                totalGeralQtd = item.Where(f => f.Mes == item.Key).Sum(s => s.QuantidadeImportada);
//                totalGeralValor = item.Where(f => f.Mes == item.Key).Sum(s => s.ValorImportada);

//                if (totalTodosQtd == 0 && totalTodosValor == 0)
//                {
//                    totalTodosQtd = totalGeralQtd;
//                    totalTodosValor = totalGeralValor;

//                    ArrayIndexMesColumns[contLinha, 0] = item.Key.ToString();
//                    ArrayIndexMesColumns[contLinha, 1] = posicaoCol.ToString();
//                    ArrayIndexMesColumns[contLinha, 2] = totalGeralQtd.ToString();
//                    ArrayIndexMesColumns[contLinha, 3] = totalGeralValor.ToString();
//                    contLinha++;
//                }
//                else
//                {
//                    totalTodosQtd = totalTodosQtd + totalGeralQtd;
//                    totalTodosValor = totalTodosValor + totalGeralValor;

//                    ArrayIndexMesColumns[contLinha, 0] = item.Key.ToString();
//                    ArrayIndexMesColumns[contLinha, 1] = posicaoCol.ToString();
//                    ArrayIndexMesColumns[contLinha, 2] = totalGeralQtd.ToString();
//                    ArrayIndexMesColumns[contLinha, 3] = totalGeralValor.ToString();
//                    contLinha++;
//                }

//                int pos = posicaoCol;

//                posicaoCol = posicaoColMerge;
//                posicaoCol++;
//                posicaoColMerge = _nameColunas.Length - 1;
//                posicaoColMerge = posicaoCol + posicaoColMerge;

//                if (pos <= 4)
//                {
//                    listArrayTotal.Add(int.Parse(totalGeralQtd.ToString()));
//                    listArrayTotal.Add(totalGeralValor.ToString());
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add(percentColuna);
//					listArrayTotal.Add("");

//					// Salvar nome das colunas para os totalizadores e cabecalho
//					for (int i = 0; i < _nameColunas.Length; i++)
//                    {
//                        listArrayNameColunas.Add(_nameColunas[i].ToString());
//                    }
//					listArrayNameColunas.Add("");
//				}
//                else
//                {
//                    listArrayTotal.Add(int.Parse(totalGeralQtd.ToString()));
//                    listArrayTotal.Add(totalGeralValor.ToString());
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add("");

//                    // Salvar nome das colunas para os totalizadores e cabecalho
//                    for (int i = 0; i < _nameColunas.Length; i++)
//                    {
//                        listArrayNameColunas.Add(_nameColunas[i].ToString());
//                    }

//                    listArrayNameColunas.Add("");
//                }
//            }

//            // Coluna TOTAL
//            if (posicaoCol >= 4)
//            {
//                posicaoCol = posicaoCol + 2;
//                posicaoColMerge = posicaoColMerge + 2;
//            }
//            else
//            {
//                posicaoCol = posicaoCol + 1;
//                posicaoColMerge = posicaoColMerge + 1;
//            }


//            _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                    _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//            AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//												  _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//												  _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//													"TOTAL", _formatColor);
//            //

//            _worksheet.Cells["A" + _startRow.ToString() + ":" +
//                             "A" + (_startRow + 2).ToString()].Merge = true;

//            AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, "A", "GERAL-SUFRAMA", _columnFontColor);

//            listArrayTotal.Add("");
//            listArrayTotal.Add(totalTodosQtd);
//            listArrayTotal.Add(totalTodosValor);
//            listArrayTotal.Add(percentColuna);
//            listArrayTotal.Add(percentColuna);

//			//Adiciona a posição para coluna Total
//			ArrayIndexMesColumns[contLinha, 0] = "TOTAL";
//			ArrayIndexMesColumns[contLinha, 1] = posicaoCol.ToString();
//			ArrayIndexMesColumns[contLinha, 2] = totalTodosQtd.ToString();
//			ArrayIndexMesColumns[contLinha, 3] = totalTodosValor.ToString();

//			//Adiciona o nome das colunas para os totais
//			listArrayNameColunas.Add("");
//            for (int l = 0; l < _nameColunas.Length; l++)
//            {
//                listArrayNameColunas.Add(_nameColunas[l].ToString());
//			}

//			// Nome das colunas
//			_startRow = _startRow + 1;
//            for (int n = 0; n < listArrayNameColunas.Count; n++)
//            {
//                if (!string.IsNullOrEmpty(listArrayNameColunas[n].ToString()))
//                {
//                    AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[n], listArrayNameColunas[n].ToString(), _columnFontColor);
//				}      
//            }

//			// Valores das colunas dos totalizadores
//			_startRow = _startRow + 1;

//            for (int j = 0; j < listArrayTotal.Count; j++)
//            {
//                if (!string.IsNullOrEmpty(listArrayTotal[j].ToString()))
//                {
//                    AplicarLinhasColunasTotais(_worksheet, _startRow, 0, _colunas[j], "#262626",
//                                                listArrayTotal[j].ToString(), 0,
//                                                "#FFFFFF", ExcelHorizontalAlignment.Right);
//				}
//            }

//            //

//            int _rowCount = 0;
//            _startRow = _startRow + 1;

//            string calculoPIN;
//            string calculoValores;

//            var agruparCidade = _listDetails.GroupBy(g => g.Cidade).OrderBy(o => o.FirstOrDefault().Estado);

//            foreach (var itemCidade in agruparCidade)
//            {
//                string _colorCidade = "#FFFFFF";
//                string _cidadeFontColor = "#000000";

//                AplicarFormatacaoConsolidadoCidade(_worksheet, _startRow, _rowCount, _rowSize, "A", _colorCidade, itemCidade.Key,
//                                                    false, _cidadeFontColor);

//                int totalQuantidade = 0;
//                decimal totalValor = 0;

//                int totalColunaQuantidade = 0;
//                decimal totalColunaValor = 0;

//				string[] _resultVetor;

//				ArrayList retLinha = ArrayList.Repeat(0, listArrayNameColunas.Count);

//                if (itemCidade.Count() > 1)
//                {
//                    string sMes = "";
//                    string sMesAnt = "";

//                    for (int cont = 0; cont < itemCidade.Count(); cont++)
//                    {
//                        sMes = itemCidade.ElementAt(cont).Mes;

//                        if (cont == 0 && itemCidade.Where(w => w.Mes == sMes).Count() == 1)
//                        {
//                            totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                            totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                            totalColunaQuantidade = totalQuantidade;
//                            totalColunaValor = totalValor;

//                            calculoPIN = "0%";
//                            calculoValores = "0%";

//							_resultVetor = TratarPosicaoColuna(ArrayIndexMesColumns, sMes, _nameColunas.Count());

//							if (totalQuantidade > 0)
//							{
//								calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//							}

//							if (totalValor > 0)
//							{
//								calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//							}

//							var registro = new RelatorioGerencialDetalheLinhaDto()
//                            {
//                                QtdColuna = totalQuantidade,
//                                ValorColuna = totalValor,
//                                ResultadoCalculoPin = calculoPIN + "%",
//                                ResultadoCalculoValores = calculoValores + "%"
//                            };

//                            retLinha = TratarLinhaCidade(retLinha, registro,false,int.Parse(_resultVetor[1].ToString()));

//                            if (cont == itemCidade.Count() - 1)
//                            {
//                                totalQuantidade = 0;
//                                totalValor = 0;
//                            }

//                            sMesAnt = itemCidade.ElementAt(cont).Mes;
//                        }
//                        else if (itemCidade.Where(w => w.Mes == sMes).Count() > 1)
//                        {
//                            totalQuantidade = itemCidade.Where(w => w.Mes == sMes).Sum(s => s.QuantidadeImportada);
//                            totalValor = itemCidade.Where(w => w.Mes == sMes).Sum(s => s.ValorImportada);

//                            totalColunaQuantidade = totalColunaQuantidade + totalQuantidade;
//                            totalColunaValor = totalColunaValor + totalValor;

//                            calculoPIN = "0%";
//                            calculoValores = "0%";

//							_resultVetor = TratarPosicaoColuna(ArrayIndexMesColumns, sMes, _nameColunas.Count());

//							if (totalQuantidade > 0)
//							{
//								calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//							}

//							if (totalValor > 0)
//							{
//								calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//							}

//							var registro = new RelatorioGerencialDetalheLinhaDto()
//                            {
//                                QtdColuna = totalQuantidade,
//                                ValorColuna = totalValor,
//                                ResultadoCalculoPin = calculoPIN + "%",
//                                ResultadoCalculoValores = calculoValores + "%"
//                            };

//							retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                            cont = cont + 1;

//                            if (cont < itemCidade.Count())
//                            {
//                                sMesAnt = itemCidade.ElementAt(cont).Mes;

//                            }
//                        }
//                        else
//                        {
//                            if (sMes == sMesAnt)
//                            {
//                                if (itemCidade.Where(w => w.Mes == sMes).Count() > 1)
//                                {
//                                    totalQuantidade = itemCidade.Where(w => w.Mes == sMes).Sum(s => s.QuantidadeImportada);
//                                    totalValor = itemCidade.Where(w => w.Mes == sMes).Sum(s => s.ValorImportada);

//                                    totalColunaQuantidade = totalQuantidade;
//                                    totalColunaValor = totalValor;

//                                    calculoPIN = "0%";
//                                    calculoValores = "0%";

//									_resultVetor = TratarPosicaoColuna(ArrayIndexMesColumns, sMes, _nameColunas.Count());

//									if (totalQuantidade > 0)
//									{
//										calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//									}

//									if (totalValor > 0)
//									{
//										calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//									}

//									var registro = new RelatorioGerencialDetalheLinhaDto()
//                                    {
//                                        QtdColuna = totalQuantidade,
//                                        ValorColuna = totalValor,
//                                        ResultadoCalculoPin = calculoPIN + "%",
//                                        ResultadoCalculoValores = calculoValores + "%"
//                                    };

//									retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                                    cont = cont + 1;

//                                    if (cont < itemCidade.Count())
//                                    {
//                                        sMesAnt = itemCidade.ElementAt(cont).Mes;

//                                    }
//                                }
//                                else
//                                {
//                                    totalQuantidade = totalQuantidade + itemCidade.ElementAt(cont).QuantidadeImportada;
//                                    totalValor = totalValor + itemCidade.ElementAt(cont).ValorImportada;

//                                    totalColunaQuantidade = totalColunaQuantidade + itemCidade.ElementAt(cont).QuantidadeImportada;
//                                    totalColunaValor = totalColunaValor + itemCidade.ElementAt(cont).ValorImportada;

//                                    calculoPIN = "0%";
//                                    calculoValores = "0%";

//									_resultVetor = TratarPosicaoColuna(ArrayIndexMesColumns, sMes, _nameColunas.Count());

//									if (totalQuantidade > 0)
//									{
//										calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//									}

//									if (totalValor > 0)
//									{
//										calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//									}

//									var registro = new RelatorioGerencialDetalheLinhaDto()
//                                    {
//                                        QtdColuna = totalQuantidade,
//                                        ValorColuna = totalValor,
//                                        ResultadoCalculoPin = calculoPIN + "%",
//                                        ResultadoCalculoValores = calculoValores + "%"
//                                    };

//									retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                                    sMesAnt = itemCidade.ElementAt(cont).Mes;
//                                }
//                            }
//                            else
//                            {
//                                totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                                totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                                totalColunaQuantidade = totalColunaQuantidade + totalQuantidade;
//                                totalColunaValor = totalColunaValor + totalValor;

//                                calculoPIN = "0%";
//                                calculoValores = "0%";

//								_resultVetor = TratarPosicaoColuna(ArrayIndexMesColumns, sMes, _nameColunas.Count());

//								if (totalQuantidade > 0)
//								{
//									calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//								}

//								if (totalValor > 0)
//								{
//									calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//								}

//								var registro = new RelatorioGerencialDetalheLinhaDto()
//                                {
//                                    QtdColuna = totalQuantidade,
//                                    ValorColuna = totalValor,
//                                    ResultadoCalculoPin = calculoPIN + "%",
//                                    ResultadoCalculoValores = calculoValores + "%"
//                                };

//								retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                                sMesAnt = itemCidade.ElementAt(cont).Mes;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    for (int cont = 0; cont < itemCidade.Count(); cont++)
//                    {
//						string sMes1 = itemCidade.ElementAt(cont).Mes;

//						totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                        totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                        totalColunaQuantidade = totalQuantidade;
//                        totalColunaValor = totalValor;

//                        calculoPIN = "0%";
//                        calculoValores = "0%";

//						_resultVetor = TratarPosicaoColuna(ArrayIndexMesColumns, sMes1, _nameColunas.Count());

//						if (totalQuantidade > 0)
//                        {
//                            calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//                        }

//                        if (totalValor > 0)
//                        {
//                            calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//                        }

//                        var registro = new RelatorioGerencialDetalheLinhaDto()
//                        {
//                            QtdColuna = totalQuantidade,
//                            ValorColuna = totalValor,
//                            ResultadoCalculoPin = calculoPIN + "%",
//                            ResultadoCalculoValores = calculoValores + "%"
//                        };

//						retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));
//                    }
//                }

//                //Atualizar os totalizadores

//                string calculoTotalPin = "0%";
//                string calculoTotalValor = "0%";

//				_resultVetor = TratarPosicaoColuna(ArrayIndexMesColumns, "TOTAL", _nameColunas.Count());

//                if (totalColunaQuantidade > 0)
//                {
//                    calculoTotalPin = (Math.Round((totalColunaQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//                }

//                if (totalColunaValor > 0)
//                {
//                    calculoTotalValor = (Math.Round((totalColunaValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//                }

//                var registroTotal = new RelatorioGerencialDetalheLinhaDto()
//                {
//                    QtdColuna = totalColunaQuantidade,
//                    ValorColuna = totalColunaValor,
//                    ResultadoCalculoPin = calculoTotalPin + "%",
//                    ResultadoCalculoValores = calculoTotalValor + "%"
//                };

//                retLinha = TratarLinhaCidade(retLinha, registroTotal, true);

//                // Ajustar vetor para planilha
//                for (int l = 0; l < listArrayNameColunas.Count; l++)
//                {
//                    if (string.IsNullOrEmpty(listArrayNameColunas[l].ToString()))
//                    {
//                        retLinha[l] = "";
//                    }
//                }

//                //Inserir Linha na Planilha
//                for (int posCelula = 0; posCelula < retLinha.Count; posCelula++)
//                {
//                    AplicarLinhasDetalhadoColunas( _worksheet, _startRow, _rowCount,
//													 _colunas[posCelula].ToString(),
//													  string.IsNullOrEmpty(retLinha[posCelula].ToString()) ? "#FFFFFF" : _formatColor,
//                                                      retLinha[posCelula].ToString(),
//                                                      false, false, false);

//                }
//                _rowCount++;
//            }
//            _rowCount++;

//            _startRow = _startRow + _rowCount;

//            return _startRow;
//        }

//        private int RelatorioDetalhadoDIA(ExcelWorksheet _worksheet, int _startRow,
//                                        List<RelatorioGerencialDetalhadoDto> _listDetails,
//                                        string _title, string _formatColor, string[] _nameColunas)
//        {
//			var _rowSize = 1;
//			_startRow = _startRow + 5;

//			_worksheet.Cells["A" + _startRow.ToString() + ":L" + _startRow.ToString()].Merge = true;

//            AplicarFormatacaoTituloConsolidadoTopo(_worksheet, "A" + _startRow.ToString() + ":L" +
//                                                    _startRow.ToString(), _formatColor, _title + " : " + " DIA ");

//            _startRow = _startRow + 2;

//            int posicaoCol = 0;
//            int posicaoColMerge = _nameColunas.Length - 1;

//            _startRow = _startRow + 1;
//            string _columnFontColor = "#FFFFFF";

//            //Colunas com o Total Geral Quantidade e Valor
//            int totalGeralQtd = 0;
//            decimal totalGeralValor = 0;
//            int totalTodosQtd = 0;
//            decimal totalTodosValor = 0;

//			string[,] ArrayIndexDiaColumns = new string[1000, 4];

//			int contLinha = 0;

//			var listArrayNameColunas = new ArrayList();
//            var listArrayTotal = new ArrayList();
					
//            int percentColuna = 100;
//			//

//			var agruparDia = _listDetails.GroupBy(g => g.Dia).OrderBy(o => o.Key);

//			foreach (var item in agruparDia)
//            {
//                if (posicaoColMerge <= 4)
//                {
//                    _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                        _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//                    AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                                        _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                                        _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                                        item.Key, _formatColor);
//                }
//                else
//                {
//                    posicaoCol = posicaoCol + 1;
//                    posicaoColMerge = posicaoColMerge + 1;

//                    _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                        _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//                    AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                                        _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                                        _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                                        item.Key, _formatColor);
//                }


//                // Salvar valores dos totalizadores
//                totalGeralQtd = item.Where(f => f.Dia == item.Key).Sum(s => s.QuantidadeImportada);
//                totalGeralValor = item.Where(f => f.Dia == item.Key).Sum(s => s.ValorImportada);

//				if (totalTodosQtd == 0 && totalTodosValor == 0)
//				{
//					totalTodosQtd = totalGeralQtd;
//					totalTodosValor = totalGeralValor;

//					ArrayIndexDiaColumns[contLinha, 0] = item.Key.ToString();
//					ArrayIndexDiaColumns[contLinha, 1] = posicaoCol.ToString();
//					ArrayIndexDiaColumns[contLinha, 2] = totalGeralQtd.ToString();
//					ArrayIndexDiaColumns[contLinha, 3] = totalGeralValor.ToString();
//					contLinha++;
//				}
//				else
//				{
//					totalTodosQtd = totalTodosQtd + totalGeralQtd;
//					totalTodosValor = totalTodosValor + totalGeralValor;

//					ArrayIndexDiaColumns[contLinha, 0] = item.Key.ToString();
//					ArrayIndexDiaColumns[contLinha, 1] = posicaoCol.ToString();
//					ArrayIndexDiaColumns[contLinha, 2] = totalGeralQtd.ToString();
//					ArrayIndexDiaColumns[contLinha, 3] = totalGeralValor.ToString();
//					contLinha++;
//				}

//				int pos = posicaoCol;

//				posicaoCol = posicaoColMerge;
//				posicaoCol++;
//				posicaoColMerge = _nameColunas.Length - 1;
//				posicaoColMerge = posicaoCol + posicaoColMerge;

//				if (pos <= 4)
//                {
//                    listArrayTotal.Add(int.Parse(totalGeralQtd.ToString()));
//                    listArrayTotal.Add(totalGeralValor.ToString());
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add(percentColuna);
//					listArrayTotal.Add("");

//					// Salvar nome das colunas para os totalizadores e cabecalho
//					for (int i = 0; i < _nameColunas.Length; i++)
//                    {
//                        listArrayNameColunas.Add(_nameColunas[i].ToString());
//                    }

//					listArrayNameColunas.Add("");
//				}
//                else
//                {
//                    listArrayTotal.Add(int.Parse(totalGeralQtd.ToString()));
//                    listArrayTotal.Add(totalGeralValor.ToString());
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add("");

//                    // Salvar nome das colunas para os totalizadores e cabecalho
//                    for (int i = 0; i < _nameColunas.Length; i++)
//                    {
//                        listArrayNameColunas.Add(_nameColunas[i].ToString());
//                    }

//                    listArrayNameColunas.Add("");
//                }
//            }

//            // Coluna TOTAL
//            if (posicaoCol >= 4)
//            {
//                posicaoCol = posicaoCol + 2;
//                posicaoColMerge = posicaoColMerge + 2;
//            }
//            else
//            {
//                posicaoCol = posicaoCol + 1;
//                posicaoColMerge = posicaoColMerge + 1;
//            }


//            _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                    _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//            AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                    _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                    _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                    "TOTAL", _formatColor);
//            //

//            _worksheet.Cells["A" + _startRow.ToString() + ":" +
//                             "A" + (_startRow + 2).ToString()].Merge = true;

//            AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, "A", "GERAL-SUFRAMA", _columnFontColor);


//            listArrayTotal.Add("");
//            listArrayTotal.Add(totalTodosQtd);
//            listArrayTotal.Add(totalTodosValor);
//            listArrayTotal.Add(percentColuna);
//            listArrayTotal.Add(percentColuna);

//			//Adiciona a posição para coluna Total
//			ArrayIndexDiaColumns[contLinha, 0] = "TOTAL";
//			ArrayIndexDiaColumns[contLinha, 1] = posicaoCol.ToString();
//			ArrayIndexDiaColumns[contLinha, 2] = totalTodosQtd.ToString();
//			ArrayIndexDiaColumns[contLinha, 3] = totalTodosValor.ToString();

//			//Adiciona o nome das colunas para os totais
//			listArrayNameColunas.Add("");
//            for (int l = 0; l < _nameColunas.Length; l++)
//            {
//                listArrayNameColunas.Add(_nameColunas[l].ToString());
//            }

//            // Nome das colunas
//            _startRow = _startRow + 1;
//            for (int n = 0; n < listArrayNameColunas.Count; n++)
//            {
//                if (!string.IsNullOrEmpty(listArrayNameColunas[n].ToString()))
//                {
//                    AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[n], listArrayNameColunas[n].ToString(), _columnFontColor);
//                }
//            }

//            // Valores das colunas dos totalizadores
//            _startRow = _startRow + 1;

//            for (int j = 0; j < listArrayTotal.Count; j++)
//            {
//                if (!string.IsNullOrEmpty(listArrayTotal[j].ToString()))
//                {
//                    AplicarLinhasColunasTotais(_worksheet, _startRow, 0, _colunas[j], "#262626",
//                                                listArrayTotal[j].ToString(), 0,
//                                                "#FFFFFF", ExcelHorizontalAlignment.Right);
//                }
//            }

//            //

//            int _rowCount = 0;
//            _startRow = _startRow + 1;

//            string calculoPIN;
//            string calculoValores;

//            var agruparCidade = _listDetails.GroupBy(g => g.Cidade).OrderBy(o => o.FirstOrDefault().Estado);

//            foreach (var itemCidade in agruparCidade)
//            {
//                string _colorCidade = "#FFFFFF";
//                string _cidadeFontColor = "#000000";

//                AplicarFormatacaoConsolidadoCidade(_worksheet, _startRow, _rowCount, _rowSize, "A", _colorCidade, itemCidade.Key,
//                                                    false, _cidadeFontColor);

//                int totalQuantidade = 0;
//                decimal totalValor = 0;

//                int totalColunaQuantidade = 0;
//                decimal totalColunaValor = 0;

//				string[] _resultVetor;

//				ArrayList retLinha = ArrayList.Repeat(0, listArrayNameColunas.Count);

//                if (itemCidade.Count() > 1)
//                {
//                    string sDia = "";
//                    string sDiaAnt = "";

//                    for (int cont = 0; cont < itemCidade.Count(); cont++)
//                    {
//                        sDia = itemCidade.ElementAt(cont).Dia;

//                        if (cont == 0 && itemCidade.Where(w => w.Dia == sDia).Count() == 1)
//                        {
//                            totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                            totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                            totalColunaQuantidade = totalQuantidade;
//                            totalColunaValor = totalValor;

//                            calculoPIN = "0%";
//                            calculoValores = "0%";

//							_resultVetor = TratarPosicaoColuna(ArrayIndexDiaColumns, sDia, _nameColunas.Count());

//							if (totalQuantidade > 0)
//							{
//								calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//							}

//							if (totalValor > 0)
//							{
//								calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//							}

//							var registro = new RelatorioGerencialDetalheLinhaDto()
//                            {
//                                QtdColuna = totalQuantidade,
//                                ValorColuna = totalValor,
//                                ResultadoCalculoPin = calculoPIN + "%",
//                                ResultadoCalculoValores = calculoValores + "%"
//                            };

//							retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                            if (cont == itemCidade.Count() - 1)
//                            {
//                                totalQuantidade = 0;
//                                totalValor = 0;
//                            }

//                            sDiaAnt = itemCidade.ElementAt(cont).Dia;
//                        }
//                        else if (itemCidade.Where(w => w.Dia == sDia).Count() > 1)
//                        {
//                            totalQuantidade = itemCidade.Where(w => w.Dia == sDia).Sum(s => s.QuantidadeImportada);
//                            totalValor = itemCidade.Where(w => w.Dia == sDia).Sum(s => s.ValorImportada);

//                            totalColunaQuantidade = totalColunaQuantidade + totalQuantidade;
//                            totalColunaValor = totalColunaValor + totalValor;

//                            calculoPIN = "0%";
//                            calculoValores = "0%";

//							_resultVetor = TratarPosicaoColuna(ArrayIndexDiaColumns, sDia, _nameColunas.Count());

//							if (totalQuantidade > 0)
//							{
//								calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//							}

//							if (totalValor > 0)
//							{
//								calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//							}

//							var registro = new RelatorioGerencialDetalheLinhaDto()
//                            {
//                                QtdColuna = totalQuantidade,
//                                ValorColuna = totalValor,
//                                ResultadoCalculoPin = calculoPIN + "%",
//                                ResultadoCalculoValores = calculoValores + "%"
//                            };

//							retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                            cont = cont + 1;

//                            if (cont < itemCidade.Count())
//                            {
//                                sDiaAnt = itemCidade.ElementAt(cont).Dia;

//                            }

//                        }
//                        else
//                        {
//                            if (sDia == sDiaAnt)
//                            {
//                                if (itemCidade.Where(w => w.Dia == sDia).Count() > 1)
//                                {
//                                    totalQuantidade = itemCidade.Where(w => w.Dia == sDia).Sum(s => s.QuantidadeImportada);
//                                    totalValor = itemCidade.Where(w => w.Dia == sDia).Sum(s => s.ValorImportada);

//                                    totalColunaQuantidade = totalQuantidade;
//                                    totalColunaValor = totalValor;

//                                    calculoPIN = "0%";
//                                    calculoValores = "0%";

//									_resultVetor = TratarPosicaoColuna(ArrayIndexDiaColumns, sDia, _nameColunas.Count());


//									if (totalQuantidade > 0)
//									{
//										calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//									}

//									if (totalValor > 0)
//									{
//										calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//									}

//									var registro = new RelatorioGerencialDetalheLinhaDto()
//                                    {
//                                        QtdColuna = totalQuantidade,
//                                        ValorColuna = totalValor,
//                                        ResultadoCalculoPin = calculoPIN + "%",
//                                        ResultadoCalculoValores = calculoValores + "%"
//                                    };

//									retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                                    cont = cont + 1;

//                                    if (cont < itemCidade.Count())
//                                    {
//                                        sDiaAnt = itemCidade.ElementAt(cont).Dia;

//                                    }
//                                }
//                                else
//                                {
//                                    totalQuantidade = totalQuantidade + itemCidade.ElementAt(cont).QuantidadeImportada;
//                                    totalValor = totalValor + itemCidade.ElementAt(cont).ValorImportada;

//                                    totalColunaQuantidade = totalColunaQuantidade + itemCidade.ElementAt(cont).QuantidadeImportada;
//                                    totalColunaValor = totalColunaValor + itemCidade.ElementAt(cont).ValorImportada;

//                                    calculoPIN = "0%";
//                                    calculoValores = "0%";

//									_resultVetor = TratarPosicaoColuna(ArrayIndexDiaColumns, sDia, _nameColunas.Count());


//									if (totalQuantidade > 0)
//									{
//										calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//									}

//									if (totalValor > 0)
//									{
//										calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//									}

//									var registro = new RelatorioGerencialDetalheLinhaDto()
//                                    {
//                                        QtdColuna = totalQuantidade,
//                                        ValorColuna = totalValor,
//                                        ResultadoCalculoPin = calculoPIN + "%",
//                                        ResultadoCalculoValores = calculoValores + "%"
//                                    };

//									retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                                    sDiaAnt = itemCidade.ElementAt(cont).Dia;
//                                }
//                            }
//                            else
//                            {
//                                totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                                totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                                totalColunaQuantidade = totalColunaQuantidade + totalQuantidade;
//                                totalColunaValor = totalColunaValor + totalValor;

//                                calculoPIN = "0%";
//                                calculoValores = "0%";

//								_resultVetor = TratarPosicaoColuna(ArrayIndexDiaColumns, sDia, _nameColunas.Count());

//								if (totalQuantidade > 0)
//								{
//									calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//								}

//								if (totalValor > 0)
//								{
//									calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//								}

//								var registro = new RelatorioGerencialDetalheLinhaDto()
//                                {
//                                    QtdColuna = totalQuantidade,
//                                    ValorColuna = totalValor,
//                                    ResultadoCalculoPin = calculoPIN + "%",
//                                    ResultadoCalculoValores = calculoValores + "%"
//                                };

//								retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                                sDiaAnt = itemCidade.ElementAt(cont).Dia;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    for (int cont = 0; cont < itemCidade.Count(); cont++)
//                    {
//						string sDia1 = itemCidade.ElementAt(cont).Dia;

//						totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                        totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                        totalColunaQuantidade = totalQuantidade;
//                        totalColunaValor = totalValor;

//                        calculoPIN = "0%";
//                        calculoValores = "0%";

//						_resultVetor = TratarPosicaoColuna(ArrayIndexDiaColumns, sDia1, _nameColunas.Count());

//						if (totalQuantidade > 0)
//						{
//							calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//						}

//						if (totalValor > 0)
//						{
//							calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//						}

//						var registro = new RelatorioGerencialDetalheLinhaDto()
//                        {
//                            QtdColuna = totalQuantidade,
//                            ValorColuna = totalValor,
//                            ResultadoCalculoPin = calculoPIN + "%",
//                            ResultadoCalculoValores = calculoValores + "%"
//                        };

//						retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));
//                    }
//                }

//                //Atualizar as colunas totalizadores

//                string calculoTotalPin = "0%";
//                string calculoTotalValor = "0%";

//				_resultVetor = TratarPosicaoColuna(ArrayIndexDiaColumns, "TOTAL", _nameColunas.Count());

//				if (totalColunaQuantidade > 0)
//				{
//					calculoTotalPin = (Math.Round((totalColunaQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//				}

//				if (totalColunaValor > 0)
//				{
//					calculoTotalValor = (Math.Round((totalColunaValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//				}


//				var registroTotal = new RelatorioGerencialDetalheLinhaDto()
//                {
//                    QtdColuna = totalColunaQuantidade,
//                    ValorColuna = totalColunaValor,
//                    ResultadoCalculoPin = calculoTotalPin + "%",
//                    ResultadoCalculoValores = calculoTotalValor + "%"
//                };

//                retLinha = TratarLinhaCidade(retLinha, registroTotal, true);

//				// Ajustar vetor para planilha
//				for (int l = 0; l < listArrayNameColunas.Count; l++)
//				{
//					if (string.IsNullOrEmpty(listArrayNameColunas[l].ToString()))
//					{
//						retLinha[l] = "";
//					}
//				}

//				//Inserir Linha na Planilha
//				for (int posCelula = 0; posCelula < retLinha.Count; posCelula++)
//				{
//					AplicarLinhasDetalhadoColunas(_worksheet, _startRow, _rowCount,
//													 _colunas[posCelula].ToString(),
//													  string.IsNullOrEmpty(retLinha[posCelula].ToString()) ? "#FFFFFF" : _formatColor,
//													  retLinha[posCelula].ToString(),
//													  false, false, false);

//				}
//				_rowCount++;
//            }
//            _rowCount++;

//            _startRow = _startRow + _rowCount;

//            return _startRow;
//        }

//        private int RelatorioDetalhadoHORA(ExcelWorksheet _worksheet, int _startRow,
//                                List<RelatorioGerencialDetalhadoDto> _listDetails,
//                                string _title, string _formatColor, string[] _nameColunas)
//        {
//			var _rowSize = 1;
//			_startRow = _startRow + 5;
           
//            _worksheet.Cells["A" + _startRow.ToString() + ":L" + _startRow.ToString()].Merge = true;

//            AplicarFormatacaoTituloConsolidadoTopo(_worksheet, "A" + _startRow.ToString() + ":L" +
//                                                    _startRow.ToString(), _formatColor, _title + " : " + " HORA ");

//            _startRow = _startRow + 2;

//            int posicaoCol = 0;
//            int posicaoColMerge = _nameColunas.Length - 1;

//            _startRow = _startRow + 1;
//            string _columnFontColor = "#FFFFFF";

//            //Colunas com o Total Geral Quantidade e Valor
//            int totalGeralQtd = 0;
//            decimal totalGeralValor = 0;
//            int totalTodosQtd = 0;
//            decimal totalTodosValor = 0;

//			string[,] ArrayIndexHoraColumns = new string[1000, 4];
//			int contLinha = 0;

//			var listArrayNameColunas = new ArrayList();
//            var listArrayTotal = new ArrayList();
//            int percentColuna = 100;
//			//

//			var agruparHora = _listDetails.GroupBy(g => g.Hora).OrderBy(o => o.Key);

//			foreach (var item in agruparHora)
//            {
//                if (posicaoColMerge <= 4)
//                {
//                    _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                        _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//                    AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                                          _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                                          _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                                            item.Key, _formatColor);
//                }
//                else
//                {
//                    posicaoCol = posicaoCol + 1;
//                    posicaoColMerge = posicaoColMerge + 1;

//                    _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                        _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//                    AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                                            _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                                            _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                                              item.Key, _formatColor);
//                }

//                // Salvar valores dos totalizadores
//                totalGeralQtd = item.Where(f => f.Hora == item.Key).Sum(s => s.QuantidadeImportada);
//                totalGeralValor = item.Where(f => f.Hora == item.Key).Sum(s => s.ValorImportada);

//                if (totalTodosQtd == 0 && totalTodosValor == 0)
//                {
//                    totalTodosQtd = totalGeralQtd;
//                    totalTodosValor = totalGeralValor;

//					ArrayIndexHoraColumns[contLinha, 0] = item.Key.ToString();
//					ArrayIndexHoraColumns[contLinha, 1] = posicaoCol.ToString();
//					ArrayIndexHoraColumns[contLinha, 2] = totalGeralQtd.ToString();
//					ArrayIndexHoraColumns[contLinha, 3] = totalGeralValor.ToString();
//					contLinha++;
//				}
//                else
//                {
//                    totalTodosQtd = totalTodosQtd + totalGeralQtd;
//                    totalTodosValor = totalTodosValor + totalGeralValor;

//					ArrayIndexHoraColumns[contLinha, 0] = item.Key.ToString();
//					ArrayIndexHoraColumns[contLinha, 1] = posicaoCol.ToString();
//					ArrayIndexHoraColumns[contLinha, 2] = totalGeralQtd.ToString();
//					ArrayIndexHoraColumns[contLinha, 3] = totalGeralValor.ToString();
//					contLinha++;
//				}

//				int pos = posicaoCol;

//				posicaoCol = posicaoColMerge;
//				posicaoCol++;
//				posicaoColMerge = _nameColunas.Length - 1;
//				posicaoColMerge = posicaoCol + posicaoColMerge;


//				if (pos <= 4)
//                {
//                    listArrayTotal.Add(int.Parse(totalGeralQtd.ToString()));
//                    listArrayTotal.Add(totalGeralValor.ToString());
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add(percentColuna);
//					listArrayTotal.Add("");

//					// Salvar nome das colunas para os totalizadores e cabecalho
//					for (int i = 0; i < _nameColunas.Length; i++)
//                    {
//                        listArrayNameColunas.Add(_nameColunas[i].ToString());
//                    }
//					listArrayNameColunas.Add("");
//				}
//                else
//                {
//                    listArrayTotal.Add(int.Parse(totalGeralQtd.ToString()));
//                    listArrayTotal.Add(totalGeralValor.ToString());
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add(percentColuna);
//                    listArrayTotal.Add("");

//                    // Salvar nome das colunas para os totalizadores e cabecalho
//                    for (int i = 0; i < _nameColunas.Length; i++)
//                    {
//                        listArrayNameColunas.Add(_nameColunas[i].ToString());
//                    }

//                    listArrayNameColunas.Add("");
//                }


//			}

//            // Coluna TOTAL
//            if (posicaoCol >= 4)
//            {
//                posicaoCol = posicaoCol + 2;
//                posicaoColMerge = posicaoColMerge + 2;
//            }
//            else
//            {
//                posicaoCol = posicaoCol + 1;
//                posicaoColMerge = posicaoColMerge + 1;
//            }


//            _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                    _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//            AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                                  _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                                  _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                                    "TOTAL", _formatColor);
//            //

//            _worksheet.Cells["A" + _startRow.ToString() + ":" +
//                             "A" + (_startRow + 2).ToString()].Merge = true;

//            AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, "A", "GERAL-SUFRAMA", _columnFontColor);


//            listArrayTotal.Add("");
//            listArrayTotal.Add(totalTodosQtd);
//            listArrayTotal.Add(totalTodosValor);
//            listArrayTotal.Add(percentColuna);
//            listArrayTotal.Add(percentColuna);

//			//Adicionar a posição para a coluna Total
//			ArrayIndexHoraColumns[contLinha, 0] = "TOTAL";
//			ArrayIndexHoraColumns[contLinha, 1] = posicaoCol.ToString();
//			ArrayIndexHoraColumns[contLinha, 2] = totalGeralQtd.ToString();
//			ArrayIndexHoraColumns[contLinha, 3] = totalGeralValor.ToString();

//			//Adiciona o nome das colunas para os totais
//			listArrayNameColunas.Add("");
//            for (int l = 0; l < _nameColunas.Length; l++)
//            {
//                listArrayNameColunas.Add(_nameColunas[l].ToString());
//            }

//            // Nome das colunas
//            _startRow = _startRow + 1;
//            for (int n = 0; n < listArrayNameColunas.Count; n++)
//            {
//                if (!string.IsNullOrEmpty(listArrayNameColunas[n].ToString()))
//                {
//                    AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[n], listArrayNameColunas[n].ToString(), _columnFontColor);
//                }
//            }

//            // Valores das colunas dos totalizadores
//            _startRow = _startRow + 1;

//            for (int j = 0; j < listArrayTotal.Count; j++)
//            {
//                if (!string.IsNullOrEmpty(listArrayTotal[j].ToString()))
//                {
//                    AplicarLinhasColunasTotais(_worksheet, _startRow, 0, _colunas[j], "#262626",
//                                                listArrayTotal[j].ToString(), 0,
//                                                "#FFFFFF", ExcelHorizontalAlignment.Right);
//                }
//            }

//            //

//            int _rowCount = 0;
//            _startRow = _startRow + 1;

//            string calculoPIN;
//            string calculoValores;

//            var agruparCidade = _listDetails.GroupBy(g => g.Cidade).OrderBy(o => o.FirstOrDefault().Estado);

//            foreach (var itemCidade in agruparCidade)
//            {
//                string _colorCidade = "#FFFFFF";
//                string _cidadeFontColor = "#000000";

//                AplicarFormatacaoConsolidadoCidade(_worksheet, _startRow, _rowCount, _rowSize, "A", _colorCidade, itemCidade.Key,
//                                                    false, _cidadeFontColor);
//                int totalQuantidade = 0;
//                decimal totalValor = 0;

//                int totalColunaQuantidade = 0;
//                decimal totalColunaValor = 0;

//				string[] _resultVetor;

//				ArrayList retLinha = ArrayList.Repeat(0, listArrayNameColunas.Count);

//                if (itemCidade.Count() > 1)
//                {
//                    string sHora = "";
//                    string sHoraAnt = "";

//                    for (int cont = 0; cont < itemCidade.Count(); cont++)
//                    {
//                        sHora = itemCidade.ElementAt(cont).Hora;

//                        if (cont == 0 && itemCidade.Where(w => w.Hora == sHora).Count() == 1)
//                        {
//                            totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                            totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                            totalColunaQuantidade = totalQuantidade;
//                            totalColunaValor = totalValor;

//                            calculoPIN = "0%";
//                            calculoValores = "0%";

//							_resultVetor = TratarPosicaoColuna(ArrayIndexHoraColumns, sHora, _nameColunas.Count());


//							if (totalQuantidade > 0)
//							{
//								calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//							}

//							if (totalValor > 0)
//							{
//								calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//							}

//							var registro = new RelatorioGerencialDetalheLinhaDto()
//                            {
//                                QtdColuna = totalQuantidade,
//                                ValorColuna = totalValor,
//                                ResultadoCalculoPin = calculoPIN + "%",
//                                ResultadoCalculoValores = calculoValores + "%"
//                            };

//							retLinha = TratarLinhaCidade(retLinha, registro, false,int.Parse(_resultVetor[1].ToString()));

//                            if (cont == itemCidade.Count() - 1)
//                            {
//                                totalQuantidade = 0;
//                                totalValor = 0;
//                            }

//                            sHoraAnt = itemCidade.ElementAt(cont).Hora;
//                        }
//                        else if (itemCidade.Where(w => w.Hora == sHora).Count() > 1)
//                        {
//                            totalQuantidade = itemCidade.Where(w => w.Hora == sHora).Sum(s => s.QuantidadeImportada);
//                            totalValor = itemCidade.Where(w => w.Hora == sHora).Sum(s => s.ValorImportada);

//                            totalColunaQuantidade = totalColunaQuantidade + totalQuantidade;
//                            totalColunaValor = totalColunaValor + totalValor;

//                            calculoPIN = "0%";
//                            calculoValores = "0%";

//							_resultVetor = TratarPosicaoColuna(ArrayIndexHoraColumns, sHora, _nameColunas.Count());

//							if (totalQuantidade > 0)
//							{
//								calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//							}

//							if (totalValor > 0)
//							{
//								calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//							}

//							var registro = new RelatorioGerencialDetalheLinhaDto()
//                            {
//                                QtdColuna = totalQuantidade,
//                                ValorColuna = totalValor,
//                                ResultadoCalculoPin = calculoPIN + "%",
//                                ResultadoCalculoValores = calculoValores + "%"
//                            };

//                            retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                            cont = cont + 1;

//                            if (cont < itemCidade.Count())
//                            {
//                                sHoraAnt = itemCidade.ElementAt(cont).Hora;

//                            }

//                        }
//                        else
//                        {
//                            if (sHora == sHoraAnt)
//                            {
//                                if (itemCidade.Where(w => w.Hora == sHora).Count() > 1)
//                                {
//                                    totalQuantidade = itemCidade.Where(w => w.Hora == sHora).Sum(s => s.QuantidadeImportada);
//                                    totalValor = itemCidade.Where(w => w.Hora == sHora).Sum(s => s.ValorImportada);

//                                    totalColunaQuantidade = totalQuantidade;
//                                    totalColunaValor = totalValor;

//                                    calculoPIN = "0%";
//                                    calculoValores = "0%";

//									_resultVetor = TratarPosicaoColuna(ArrayIndexHoraColumns, sHora, _nameColunas.Count());

//									if (totalQuantidade > 0)
//									{
//										calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//									}

//									if (totalValor > 0)
//									{
//										calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//									}

//									var registro = new RelatorioGerencialDetalheLinhaDto()
//                                    {
//                                        QtdColuna = totalQuantidade,
//                                        ValorColuna = totalValor,
//                                        ResultadoCalculoPin = calculoPIN + "%",
//                                        ResultadoCalculoValores = calculoValores + "%"
//                                    };

//									retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                                    cont = cont + 1;

//                                    if (cont < itemCidade.Count())
//                                    {
//                                        sHoraAnt = itemCidade.ElementAt(cont).Hora;

//                                    }
//                                }
//                                else
//                                {
//                                    totalQuantidade = totalQuantidade + itemCidade.ElementAt(cont).QuantidadeImportada;
//                                    totalValor = totalValor + itemCidade.ElementAt(cont).ValorImportada;

//                                    totalColunaQuantidade = totalColunaQuantidade + itemCidade.ElementAt(cont).QuantidadeImportada;
//                                    totalColunaValor = totalColunaValor + itemCidade.ElementAt(cont).ValorImportada;

//                                    calculoPIN = "0%";
//                                    calculoValores = "0%";

//									_resultVetor = TratarPosicaoColuna(ArrayIndexHoraColumns, sHora, _nameColunas.Count());

//									if (totalQuantidade > 0)
//									{
//										calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//									}

//									if (totalValor > 0)
//									{
//										calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//									}

//									var registro = new RelatorioGerencialDetalheLinhaDto()
//                                    {
//                                        QtdColuna = totalQuantidade,
//                                        ValorColuna = totalValor,
//                                        ResultadoCalculoPin = calculoPIN + "%",
//                                        ResultadoCalculoValores = calculoValores + "%"
//                                    };

//									retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));

//                                    sHoraAnt = itemCidade.ElementAt(cont).Hora;
//                                }
//                            }
//                            else
//                            {
//                                totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                                totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                                totalColunaQuantidade = totalColunaQuantidade + totalQuantidade;
//                                totalColunaValor = totalColunaValor + totalValor;

//                                calculoPIN = "0%";
//                                calculoValores = "0%";

//								_resultVetor = TratarPosicaoColuna(ArrayIndexHoraColumns, sHora, _nameColunas.Count());

//								if (totalQuantidade > 0)
//								{
//									calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//								}

//								if (totalValor > 0)
//								{
//									calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//								}

//								var registro = new RelatorioGerencialDetalheLinhaDto()
//                                {
//                                    QtdColuna = totalQuantidade,
//                                    ValorColuna = totalValor,
//                                    ResultadoCalculoPin = calculoPIN + "%",
//                                    ResultadoCalculoValores = calculoValores + "%"
//                                };

//                                retLinha = TratarLinhaCidade(retLinha, registro,false, int.Parse(_resultVetor[1].ToString()));

//                                sHoraAnt = itemCidade.ElementAt(cont).Hora;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    for (int cont = 0; cont < itemCidade.Count(); cont++)
//                    {
//						var sHora1 = itemCidade.ElementAt(cont).Hora;

//						totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//                        totalValor = itemCidade.ElementAt(cont).ValorImportada;

//                        totalColunaQuantidade = totalQuantidade;
//                        totalColunaValor = totalValor;

//                        calculoPIN = "0%";
//                        calculoValores = "0%";

//						_resultVetor = TratarPosicaoColuna(ArrayIndexHoraColumns, sHora1, _nameColunas.Count());

//						if (totalQuantidade > 0)
//						{
//							calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//						}

//						if (totalValor > 0)
//						{
//							calculoValores = (Math.Round((totalValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//						}

//						var registro = new RelatorioGerencialDetalheLinhaDto()
//                        {
//                            QtdColuna = totalQuantidade,
//                            ValorColuna = totalValor,
//                            ResultadoCalculoPin = calculoPIN + "%",
//                            ResultadoCalculoValores = calculoValores + "%"
//                        };

//						retLinha = TratarLinhaCidade(retLinha, registro, false, int.Parse(_resultVetor[1].ToString()));
//                    }
//                }

//                //Atualizar as colunas totalizadores

//                string calculoTotalPin = "0%";
//                string calculoTotalValor = "0%";

//				_resultVetor = TratarPosicaoColuna(ArrayIndexHoraColumns, "TOTAL", _nameColunas.Count());

//				if (totalColunaQuantidade > 0)
//				{
//					calculoTotalPin = (Math.Round((totalColunaQuantidade / decimal.Parse(_resultVetor[2].ToString())) * 100, 2).ToString());
//				}

//				if (totalColunaValor > 0)
//				{
//					calculoTotalValor = (Math.Round((totalColunaValor / decimal.Parse(_resultVetor[3].ToString())) * 100, 2).ToString());
//				}

//				var registroTotal = new RelatorioGerencialDetalheLinhaDto()
//                {
//                    QtdColuna = totalColunaQuantidade,
//                    ValorColuna = totalColunaValor,
//                    ResultadoCalculoPin = calculoTotalPin + "%",
//                    ResultadoCalculoValores = calculoTotalValor + "%"
//                };

//				retLinha = TratarLinhaCidade(retLinha, registroTotal, true);

//				// Ajustar vetor para planilha
//				for (int l = 0; l < listArrayNameColunas.Count; l++)
//				{
//					if (string.IsNullOrEmpty(listArrayNameColunas[l].ToString()))
//					{
//						retLinha[l] = "";
//					}
//				}

//				//Inserir Linha na Planilha
//				for (int posCelula = 0; posCelula < retLinha.Count; posCelula++)
//				{
//					AplicarLinhasDetalhadoColunas(_worksheet, _startRow, _rowCount,
//													 _colunas[posCelula].ToString(),
//													  string.IsNullOrEmpty(retLinha[posCelula].ToString()) ? "#FFFFFF" : _formatColor,
//													  retLinha[posCelula].ToString(),
//													  false, false, false);

//				}

//				_rowCount++;
//            }
//            _rowCount++;

//            _startRow = _startRow + _rowCount;

//            return _startRow;
//        }

//        private int RelatorioDetalhadoVistoriaMES(ExcelWorksheet _worksheet, int _startRow,
//                                        List<RelatorioVistoriaDetalhadaDto> _listDetails,
//                                        string _title, string _formatColor, string[] _nameColunas)
//        {
//            _startRow = _startRow + 5;
//            var _rowSize = 1;

//            _worksheet.Cells["A" + _startRow.ToString() + ":L" + _startRow.ToString()].Merge = true;

//            AplicarFormatacaoTituloConsolidadoTopo(_worksheet, "A" + _startRow.ToString() + ":L" +
//                                                    _startRow.ToString(), _formatColor, _title + " : " + " MÊS ");

//            _startRow = _startRow + 2;

//            int posicaoCol = 0;
//            int posicaoColMerge = _nameColunas.Length - 1;

//            _startRow = _startRow + 1;
//            string _columnFontColor = "#FFFFFF";

//            var agruparMes = _listDetails.GroupBy(g => g.Mes).OrderBy(o => o.Key);

//            //Colunas com o Total Geral Quantidade e Valor
//            int totalGeralQtd = 0;
//            decimal totalGeralValor = 0;
//            int totalTodosQtd = 0;
//            decimal totalTodosValor = 0;

//            var listIndexColumns = new ArrayList();
//            var listIndexTotais = new ArrayList();

//            string[,] ArrayIndexMesColumns = new string[1000, 4];
//            string[,] ArrayCanalMes = new string[100, 2];
//            string[,] ArrayMesPosicao = new string[12, 3];

//            List<MesCanal> listMesCanal = new List<MesCanal>();

//            int contLinha = 0;

//            var listArrayNameColunas = new ArrayList();
//            var listArrayTotal = new ArrayList();
//            int percentColuna = 100;
//            //
//            _formatColor = "#FFFF00";

//            foreach (var itemMes in agruparMes)
//            {
//                foreach (var itemCanal in itemMes)
//                {
//                    //ArrayCanalMes[contLinha, 0] = itemCanal.Canal.ToString();
//                    //ArrayCanalMes[contLinha, 1] = itemCanal.Mes.ToString();

//                    //               contLinha++;
//                    var linhareg = new MesCanal()
//                    {
//                        Mes = itemCanal.Mes.ToString(),
//                        Canal = itemCanal.Canal.ToString()
//                    };

//                    listMesCanal.Add(linhareg);
//                }

//            }

//            contLinha = 0;

//            foreach (var item in agruparMes)
//            {
//                _worksheet.Cells[_colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                    _colunas[posicaoColMerge].ToString() + _startRow.ToString()].Merge = true;

//                AplicarCabecalhoDetalhadoColunasMerge(_worksheet,
//                                                    _colunas[posicaoCol].ToString() + _startRow.ToString() + ":" +
//                                                    _colunas[posicaoColMerge].ToString() + _startRow.ToString(),
//                                                    _meses[int.Parse(item.Key)], _formatColor);
//                int pos = posicaoCol;

//                ArrayIndexMesColumns[contLinha, 0] = item.Key.ToString();
//                ArrayIndexMesColumns[contLinha, 1] = _colunas[posicaoCol].ToString();
//                ArrayIndexMesColumns[contLinha, 2] = totalGeralQtd.ToString();
//                ArrayIndexMesColumns[contLinha, 3] = totalGeralValor.ToString();

//                ArrayMesPosicao[contLinha, 0] = item.Key.ToString();
//                ArrayMesPosicao[contLinha, 1] = posicaoCol.ToString();
//                ArrayMesPosicao[contLinha, 2] = posicaoColMerge.ToString();

//                contLinha++;

//                posicaoCol = posicaoColMerge;
//                posicaoCol++;
//                posicaoColMerge = _nameColunas.Length - 1;
//                posicaoColMerge = posicaoCol + posicaoColMerge;

//                //Abordagem para resolver os totalizadores
//                //          foreach (var grupoMes in agruparMes))
//                //          {
//                //              foreach (var cidade in grupoMes)
//                //              {
//                //cidade.TotalQuantidadeImportada += cidade.QuantidadeImportada;
//                //              }
//                //          }

//                //totalGeralQtd = item.Where(f => f.Mes == item.Key).Sum(s => s.QuantidadeImportada);
//                //totalGeralValor = item.Where(f => f.Mes == item.Key).Sum(s => s.ValorImportada);

//                //if (totalTodosQtd == 0 && totalTodosValor == 0)
//                //{
//                //	totalTodosQtd = totalGeralQtd;
//                //	totalTodosValor = totalGeralValor;

//                //	ArrayIndexMesColumns[contLinha, 0] = item.Key.ToString();
//                //	ArrayIndexMesColumns[contLinha, 1] = _colunas[posicaoCol].ToString();
//                //	ArrayIndexMesColumns[contLinha, 2] = totalGeralQtd.ToString();
//                //	ArrayIndexMesColumns[contLinha, 3] = totalGeralValor.ToString();
//                //	contLinha++;
//                //}
//                //else
//                //{
//                //	totalTodosQtd = totalTodosQtd + totalGeralQtd;
//                //	totalTodosValor = totalTodosValor + totalGeralValor;

//                //	ArrayIndexMesColumns[contLinha, 0] = item.Key.ToString();
//                //	ArrayIndexMesColumns[contLinha, 1] = _colunas[posicaoCol].ToString();
//                //	ArrayIndexMesColumns[contLinha, 2] = totalGeralQtd.ToString();
//                //	ArrayIndexMesColumns[contLinha, 3] = totalGeralValor.ToString();
//                //	contLinha++;
//                //}

//                //if (pos <= 8)
//                //{
//                //	listArrayTotal.Add(int.Parse(totalGeralQtd.ToString()));
//                //	listArrayTotal.Add(totalGeralValor.ToString());
//                //	listArrayTotal.Add(percentColuna);
//                //	listArrayTotal.Add(percentColuna);

//                //	// Salvar nome das colunas para os totalizadores e cabecalho
//                //	for (int i = 0; i < _nameColunas.Length; i++)
//                //	{
//                //		listArrayNameColunas.Add(_nameColunas[i].ToString());
//                //	}
//                //}
//                //else
//                //{
//                //	listArrayTotal.Add(int.Parse(totalGeralQtd.ToString()));
//                //	listArrayTotal.Add(totalGeralValor.ToString());
//                //	listArrayTotal.Add(percentColuna);
//                //	listArrayTotal.Add(percentColuna);
//                //	//listArrayTotal.Add("");

//                //	// Salvar nome das colunas para os totalizadores e cabecalho
//                //	for (int i = 0; i < _nameColunas.Length; i++)
//                //	{
//                //		listArrayNameColunas.Add(_nameColunas[i].ToString());
//                //	}

//                //	//listArrayNameColunas.Add("");
//                //}
//            }

//            _startRow = _startRow + 1;
//            _worksheet.Cells["A" + _startRow.ToString() + ":" +
//                 "A" + (_startRow + 2).ToString()].Merge = true;

//            AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, "A", "GERAL-SUFRAMA", _columnFontColor);

//            string sProximoMes = "";
//            string sCanal = "";
//            string sCanalAnterior = "";
//            string posicaoMesCol = "";
//            string posicaoMesColMerge = "";
//            int _rowCount = 0;
//            //_startRow = _startRow + 1;
//            posicaoCol = 0;

//            var listMesColuna = new List<MesColuna>();
//			int _linhaColuna = 0;

//			foreach (var item in listMesCanal)
//            {
//				_linhaColuna = 0;
//				sCanal = item.Canal.ToString();

//                var selecaoVetor = TratarPosicaoColuna(ArrayMesPosicao, item.Mes, 3);

//				posicaoMesColMerge = _colunas[int.Parse(selecaoVetor[2].ToString())].ToString();

//				if (item.Mes == sProximoMes)
//                {
//                    if (item.Canal == sCanalAnterior)
//                    {

//                    }
//                    else
//                    {
//                        if (int.Parse(selecaoVetor[1].ToString()) == int.Parse(selecaoVetor[2].ToString()))
//                        {
//                            posicaoMesCol = _colunas[posicaoCol].ToString();
//                            posicaoCol++;
//                        }
//                        else if (posicaoCol == int.Parse(selecaoVetor[1].ToString()))
//                        {
//                            posicaoCol++;
//                            posicaoMesCol = _colunas[posicaoCol].ToString();
//                        }
//                        else
//                        {
//                            posicaoCol++;
//                            posicaoMesCol = _colunas[posicaoCol].ToString();
//                            ;
//                        }

//                        //for (int l = 0; l < _nameColunas.Length; l++)
//                        //{
//                        //    var registroColuna = new MesColuna()
//                        //    {
//                        //        NomeMes = item.Mes,
//                        //        NomeCanal = item.Canal,
//                        //        NomeColuna = item.Canal == "AZUL" ? _nameColunas[1].ToString() : _nameColunas[l].ToString()
//                        //    };

//                        //    listMesColuna.Add(registroColuna);

//                        //    if (item.Canal == "AZUL")
//                        //    {
//                        //        break;
//                        //    }
//                        //}
//                    }
//                }
//                else
//                {
//                    if (int.Parse(selecaoVetor[1].ToString()) == int.Parse(selecaoVetor[2].ToString()))
//                    {
//                        posicaoMesCol = _colunas[posicaoCol].ToString();
//                        posicaoCol++;
//                    }
//                    else
//                    {
//                        posicaoMesCol = _colunas[int.Parse(selecaoVetor[1].ToString())].ToString();
//                        posicaoCol = int.Parse(selecaoVetor[1].ToString());
//                    }
//                }

//                if (item.Canal == "CINZA")
//                {
//                    _formatColor = "#808080";


//                    AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow,
//                                                        posicaoMesCol.ToString(),
//                                                        item.Canal.ToString(), _formatColor);
//					_linhaColuna = _startRow + 1;
//					for (int col = 1; col < _nameColunas.Count(); col++)
//					{
//						AplicarCabecalhoDetalhadoColunas(_worksheet, _linhaColuna,
//															posicaoMesCol.ToString(),
//															_nameColunas[col].ToString(), _formatColor);

//					}

//				}
//				else if (item.Canal == "AZUL")
//                {
//                    _formatColor = "#0000FF";

//                    AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow,
//                                                        posicaoMesCol.ToString(),
//                                                       item.Canal.ToString(), _formatColor);


//					AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow+1,
//														posicaoMesCol.ToString(),
//													  "VISTORIADO", _formatColor);

//				}
//                else if (item.Canal == "VERMELHO")
//                {
//                    _formatColor = "#FF0000";

//                    AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow,
//                                                        posicaoMesCol.ToString(),
//                                                        item.Canal.ToString(), _formatColor);
//					_linhaColuna = _startRow + 1;
//					for (int col = 1; col < _nameColunas.Count(); col++)
//					{
//						AplicarCabecalhoDetalhadoColunas(_worksheet, _linhaColuna,
//															posicaoMesCol.ToString(),
//															_nameColunas[col].ToString(), _formatColor);

//					}


//				}
//				else if (item.Canal == "VERDE")
//                {
//                    _formatColor = "#008000";

//                    AplicarCabecalhoDetalhadoColunasMerge(_worksheet, 
//														   posicaoMesCol.ToString()+ _startRow.ToString() +":"+
//														   posicaoMesColMerge.ToString() + _startRow.ToString(),
//                                                           item.Canal.ToString(), _formatColor);

//					_linhaColuna = _startRow + 1;
//                    for (int col = 1; col < _nameColunas.Count(); col++)
//                    {
//						AplicarCabecalhoDetalhadoColunas(_worksheet, _linhaColuna,
//														  posicaoMesCol.ToString(),
//														 _nameColunas[col].ToString(), _formatColor);

//					}
//				}
//                else
//                {
//                    _formatColor = "#FFFFFF";

//                    AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow,
//                                                        posicaoMesCol.ToString(),
//                                                        item.Canal.ToString(), _formatColor);
//					_linhaColuna = _startRow + 1;
//					for (int col = 1; col < _nameColunas.Count(); col++)
//					{
//						AplicarCabecalhoDetalhadoColunas(_worksheet, _linhaColuna,
//														   posicaoMesCol.ToString(),
//														_nameColunas[col].ToString(), _formatColor);

//					}

//				}


//				sProximoMes = item.Mes.ToString();
//                sCanalAnterior = item.Canal.ToString();
//            }

//            //Adiciona o nome das colunas para os totais
//            //var listMesColuna = new List<MesColuna>();

//            //foreach (var item in listMesCanal)
//            //{
//            //    for (int l = 0; l < _nameColunas.Length; l++)
//            //    {
//            //        //listArrayNameColunas.Add(_nameColunas[l].ToString());
//            //        var registroColuna = new MesColuna()
//            //        {
//            //            NomeMes = item.Mes,
//            //            NomeCanal = item.Canal,
//            //            NomeColuna = item.Canal == "AZUL" ? _nameColunas[1].ToString() : _nameColunas[l].ToString()
//            //        };

//            //        listMesColuna.Add(registroColuna);

//            //        if(item.Canal == "AZUL")
//            //        {
//            //            break;
//            //        }  
//            //    }
//            //}

//            //Nome das colunas
//            _startRow = _startRow + 1;
//            //for (int n = 0; n < listArrayNameColunas.Count; n++)
//            //{
//            //    if (!string.IsNullOrEmpty(listArrayNameColunas[n].ToString()))
//            //    {
//            //        AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[n], listArrayNameColunas[n].ToString(), _columnFontColor);
//            //        listIndexColumns.Add(_colunas[n]);
//            //    }
//            //}
//            int contColuna = 0;

//            //foreach (var item in listMesColuna)
//            //{
//            //    if (item.NomeCanal.ToString() == "CINZA")
//            //    {
//            //        _formatColor = "#808080";
//            //        AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[contColuna], item.NomeColuna.ToString(), _formatColor);
//            //        contColuna++;
//            //    }
//            //    else if (item.NomeCanal.ToString() == "AZUL")
//            //    {
//            //        _formatColor = "#0000FF";
//            //        AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[contColuna], item.NomeColuna.ToString(), _formatColor);
//            //        contColuna++;
//            //    }
//            //    else if (item.NomeCanal.ToString() == "VERMELHO")
//            //    {
//            //        _formatColor = "#FF0000";

//            //        AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[contColuna], item.NomeColuna.ToString(), _formatColor);
//            //        contColuna++;
//            //    }
//            //    else if (item.NomeCanal.ToString() == "VERDE")
//            //    {
//            //        _formatColor = "#008000";

//            //        AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[contColuna], item.NomeColuna.ToString(), _formatColor);
//            //        contColuna++;
//            //    }
//            //    else
//            //    {
//            //        _formatColor = "#FFFFFF";

//            //        AplicarCabecalhoDetalhadoColunas(_worksheet, _startRow, _colunas[contColuna], item.NomeColuna.ToString(), _formatColor);
//            //        contColuna++;
//            //    }
//            //}
//            //int _rowCount = 0;
//            _startRow = _startRow + 1;

//			string calculoPIN;
//            string calculoValores;

//			//var agruparCidade = _listDetails.GroupBy(g => g.Cidade).OrderBy(o => o.FirstOrDefault().Estado);

//			/*
//			foreach (var itemCidade in agruparCidade)
//			{
//				string _colorCidade = "#FFFFFF";
//				string _cidadeFontColor = "#000000";

//				AplicarFormatacaoConsolidadoCidade(_worksheet, _startRow, _rowCount, _rowSize, "A", _colorCidade, itemCidade.Key,
//													false, _cidadeFontColor);

//				int _colunaPosicao = 0;
//				int totalQuantidade = 0;
//				decimal totalValor = 0;

//				int totalColunaQuantidade = 0;
//				decimal totalColunaValor = 0;

//				ArrayList retLinha = ArrayList.Repeat(0, listIndexColumns.Count);

//				if (itemCidade.Count() > 1)
//				{
//					string sMes = "";
//					string sMesAnt = "";

//					for (int cont = 0; cont < itemCidade.Count(); cont++)
//					{
//						sMes = itemCidade.ElementAt(cont).Mes;

//						if (cont == 0 && itemCidade.Where(w => w.Mes == sMes).Count() == 1)
//						{
//							totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//							totalValor = itemCidade.ElementAt(cont).ValorImportada;

//							totalColunaQuantidade = totalQuantidade;
//							totalColunaValor = totalValor;

//							calculoPIN = "0%";
//							calculoValores = "0%";

//							//var selVetor = TratarMesColuna(ArrayIndexMesColumns, sMes);

//							//if (totalQuantidade > 0)
//							//{
//							//	calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(int.Parse(selVetor[2]).ToString())) * 100, 2).ToString());
//							//}

//							//if (totalValor > 0)
//							//{
//							//	calculoValores = (Math.Round((totalValor / decimal.Parse(decimal.Parse(selVetor[3]).ToString())) * 100, 2).ToString());
//							//}


//							if (totalQuantidade > 0)
//							{
//								calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(listIndexTotais[_colunaPosicao].ToString())) * 100, 2).ToString());
//							}

//							if (totalValor > 0)
//							{
//								calculoValores = (Math.Round((totalValor / decimal.Parse(listIndexTotais[_colunaPosicao + 1].ToString())) * 100, 2).ToString());
//							}

//							var registro = new RelatorioGerencialDetalheLinhaDto()
//							{
//								QtdColuna = totalQuantidade,
//								ValorColuna = totalValor,
//								ResultadoCalculoPin = calculoPIN + "%",
//								ResultadoCalculoValores = calculoValores + "%"
//							};

//							retLinha = TratarLinhaCidade(retLinha, registro);

//							if (_colunaPosicao == 0)
//							{
//								_colunaPosicao = _nameColunas.Length;
//							}
//							else
//							{
//								_colunaPosicao = _colunaPosicao + _nameColunas.Length;
//							}

//							if (cont == itemCidade.Count() - 1)
//							{
//								totalQuantidade = 0;
//								totalValor = 0;
//							}

//							sMesAnt = itemCidade.ElementAt(cont).Mes;
//						}
//						else if (itemCidade.Where(w => w.Mes == sMes).Count() > 1)
//						{
//							totalQuantidade = itemCidade.Where(w => w.Mes == sMes).Sum(s => s.QuantidadeImportada);
//							totalValor = itemCidade.Where(w => w.Mes == sMes).Sum(s => s.ValorImportada);

//							totalColunaQuantidade = totalColunaQuantidade + totalQuantidade;
//							totalColunaValor = totalColunaValor + totalValor;

//							calculoPIN = "0%";
//							calculoValores = "0%";

//							if (totalQuantidade > 0)
//							{
//								calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(listIndexTotais[_colunaPosicao].ToString())) * 100, 2).ToString());
//							}

//							if (totalValor > 0)
//							{
//								calculoValores = (Math.Round((totalValor / decimal.Parse(listIndexTotais[_colunaPosicao + 1].ToString())) * 100, 2).ToString());
//							}

//							var registro = new RelatorioGerencialDetalheLinhaDto()
//							{
//								QtdColuna = totalQuantidade,
//								ValorColuna = totalValor,
//								ResultadoCalculoPin = calculoPIN + "%",
//								ResultadoCalculoValores = calculoValores + "%"
//							};

//							retLinha = TratarLinhaCidade(retLinha, registro);

//							if (_colunaPosicao == 0)
//							{
//								_colunaPosicao = _nameColunas.Length;
//							}
//							else
//							{
//								_colunaPosicao = _colunaPosicao + _nameColunas.Length;
//							}

//							cont = cont + 1;

//							if (cont < itemCidade.Count())
//							{
//								sMesAnt = itemCidade.ElementAt(cont).Mes;

//							}

//						}
//						else
//						{
//							if (sMes == sMesAnt)
//							{
//								if (itemCidade.Where(w => w.Mes == sMes).Count() > 1)
//								{
//									totalQuantidade = itemCidade.Where(w => w.Mes == sMes).Sum(s => s.QuantidadeImportada);
//									totalValor = itemCidade.Where(w => w.Mes == sMes).Sum(s => s.ValorImportada);

//									totalColunaQuantidade = totalQuantidade;
//									totalColunaValor = totalValor;

//									calculoPIN = "0%";
//									calculoValores = "0%";

//									if (totalQuantidade > 0)
//									{
//										calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(listIndexTotais[_colunaPosicao].ToString())) * 100, 2).ToString());
//									}

//									if (totalValor > 0)
//									{
//										calculoValores = (Math.Round((totalValor / decimal.Parse(listIndexTotais[_colunaPosicao + 1].ToString())) * 100, 2).ToString());
//									}

//									var registro = new RelatorioGerencialDetalheLinhaDto()
//									{
//										QtdColuna = totalQuantidade,
//										ValorColuna = totalValor,
//										ResultadoCalculoPin = calculoPIN + "%",
//										ResultadoCalculoValores = calculoValores + "%"
//									};

//									retLinha = TratarLinhaCidade(retLinha, registro);

//									if (_colunaPosicao == 0)
//									{
//										_colunaPosicao = _nameColunas.Length;
//									}
//									else
//									{
//										_colunaPosicao = _colunaPosicao + _nameColunas.Length;
//									}

//									cont = cont + 1;

//									if (cont < itemCidade.Count())
//									{
//										sMesAnt = itemCidade.ElementAt(cont).Mes;

//									}
//								}
//								else
//								{
//									totalQuantidade = totalQuantidade + itemCidade.ElementAt(cont).QuantidadeImportada;
//									totalValor = totalValor + itemCidade.ElementAt(cont).ValorImportada;

//									totalColunaQuantidade = totalColunaQuantidade + itemCidade.ElementAt(cont).QuantidadeImportada;
//									totalColunaValor = totalColunaValor + itemCidade.ElementAt(cont).ValorImportada;

//									calculoPIN = "0%";
//									calculoValores = "0%";

//									if (totalQuantidade > 0)
//									{
//										calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(listIndexTotais[_colunaPosicao].ToString())) * 100, 2).ToString());
//									}

//									if (totalValor > 0)
//									{
//										calculoValores = (Math.Round((totalValor / decimal.Parse(listIndexTotais[_colunaPosicao + 1].ToString())) * 100, 2).ToString());
//									}

//									var registro = new RelatorioGerencialDetalheLinhaDto()
//									{
//										QtdColuna = totalQuantidade,
//										ValorColuna = totalValor,
//										ResultadoCalculoPin = calculoPIN + "%",
//										ResultadoCalculoValores = calculoValores + "%"
//									};

//									retLinha = TratarLinhaCidade(retLinha, registro);

//									if (_colunaPosicao == 0)
//									{
//										_colunaPosicao = _nameColunas.Length;
//									}
//									else
//									{
//										_colunaPosicao = _colunaPosicao + _nameColunas.Length;
//									}

//									sMesAnt = itemCidade.ElementAt(cont).Mes;
//								}
//							}
//							else
//							{
//								totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//								totalValor = itemCidade.ElementAt(cont).ValorImportada;

//								totalColunaQuantidade = totalColunaQuantidade + totalQuantidade;
//								totalColunaValor = totalColunaValor + totalValor;

//								calculoPIN = "0%";
//								calculoValores = "0%";

//								if (totalQuantidade > 0)
//								{
//									calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(listIndexTotais[_colunaPosicao].ToString())) * 100, 2).ToString());
//								}

//								if (totalValor > 0)
//								{
//									calculoValores = (Math.Round((totalValor / decimal.Parse(listIndexTotais[_colunaPosicao + 1].ToString())) * 100, 2).ToString());
//								}

//								var registro = new RelatorioGerencialDetalheLinhaDto()
//								{
//									QtdColuna = totalQuantidade,
//									ValorColuna = totalValor,
//									ResultadoCalculoPin = calculoPIN + "%",
//									ResultadoCalculoValores = calculoValores + "%"
//								};

//								retLinha = TratarLinhaCidade(retLinha, registro);

//								if (_colunaPosicao == 0)
//								{
//									_colunaPosicao = _nameColunas.Length;
//								}
//								else
//								{
//									_colunaPosicao = _colunaPosicao + _nameColunas.Length;
//								}

//								sMesAnt = itemCidade.ElementAt(cont).Mes;
//							}
//						}
//					}
//				}
//				else
//				{
//					for (int cont = 0; cont < itemCidade.Count(); cont++)
//					{
//						totalQuantidade = itemCidade.ElementAt(cont).QuantidadeImportada;
//						totalValor = itemCidade.ElementAt(cont).ValorImportada;

//						totalColunaQuantidade = totalQuantidade;
//						totalColunaValor = totalValor;

//						calculoPIN = "0%";
//						calculoValores = "0%";

//						if (totalQuantidade > 0)
//						{
//							calculoPIN = (Math.Round((totalQuantidade / decimal.Parse(listIndexTotais[_colunaPosicao].ToString())) * 100, 2).ToString());
//						}

//						if (totalValor > 0)
//						{
//							calculoValores = (Math.Round((totalValor / decimal.Parse(listIndexTotais[_colunaPosicao + 1].ToString())) * 100, 2).ToString());
//						}

//						var registro = new RelatorioGerencialDetalheLinhaDto()
//						{
//							QtdColuna = totalQuantidade,
//							ValorColuna = totalValor,
//							ResultadoCalculoPin = calculoPIN + "%",
//							ResultadoCalculoValores = calculoValores + "%"
//						};
//						retLinha = TratarLinhaCidade(retLinha, registro);

//						if (_colunaPosicao == 0)
//						{
//							_colunaPosicao = _nameColunas.Length;
//						}
//						else
//						{
//							_colunaPosicao = _colunaPosicao + _nameColunas.Length;
//						}
//					}
//				}

//				//Atualizar as colunas totalizadores
//				if (_colunaPosicao == 0)
//				{
//					_colunaPosicao = _nameColunas.Length;
//				}
//				else
//				{
//					_colunaPosicao = _colunaPosicao + _nameColunas.Length;

//					if (listIndexTotais.Count == _colunaPosicao)
//					{
//						_colunaPosicao = _colunaPosicao - _nameColunas.Length;

//					}
//				}

//				string calculoTotalPin = "0%";
//				string calculoTotalValor = "0%";

//				if (totalColunaQuantidade > 0)
//				{
//					calculoTotalPin = (Math.Round((totalColunaQuantidade / decimal.Parse(listIndexTotais[_colunaPosicao].ToString())) * 100, 2).ToString());
//				}

//				if (totalColunaValor > 0)
//				{
//					calculoTotalValor = (Math.Round((totalColunaValor / decimal.Parse(listIndexTotais[_colunaPosicao + 1].ToString())) * 100, 2).ToString());
//				}

//				var registroTotal = new RelatorioGerencialDetalheLinhaDto()
//				{
//					QtdColuna = totalColunaQuantidade,
//					ValorColuna = totalColunaValor,
//					ResultadoCalculoPin = calculoTotalPin + "%",
//					ResultadoCalculoValores = calculoTotalValor + "%"
//				};
//				retLinha = TratarLinhaCidade(retLinha, registroTotal, true);

//				//Inserir Linha na Planilha
//				for (int posCelula = 0; posCelula < retLinha.Count; posCelula++)
//				{
//					AplicarLinhasDetalhadoColunas(_worksheet, _startRow, _rowCount,
//														listIndexColumns[posCelula].ToString(),
//														_formatColor,
//														retLinha[posCelula].ToString(),
//														false, false, false);

//				}
//				_rowCount++;
//			}
//			*/
//			_rowCount++;

//			_startRow = _startRow + _rowCount;

//			return _startRow;
//		}

//        private string[] TratarPosicaoColuna(string[,] vetorColuna, string sPesquisa,int size)
//        {
//            string[] sVetorRet = new string[4];
//            int linCorrente = 0;
//            bool bEncoontrou = false;

//            for (int lin = 0; lin < vetorColuna.Length; lin++)
//            {
//                for (int col = 0; col < size; col++)
//                {
//                    if (vetorColuna[lin, 0].ToString() == sPesquisa)
//                    {
//							sVetorRet[linCorrente] = vetorColuna[lin, col];
//                        linCorrente++;

//                        if (linCorrente == size)
//                        {
//                            bEncoontrou = true;
//                        }
//                    }
//                }

//                if (bEncoontrou)
//                {
//                    break;
//                }
//                else
//                {
//                    continue;
//                }
//            }

//            return sVetorRet;
//        }

//		private ArrayList TratarLinhaCidade(ArrayList _arrayLinha,
//											RelatorioGerencialDetalheLinhaDto _registro,
//											bool bTotalizadores = false, int posicao = 0)
//		{
//			var arrayResult = new ArrayList();
//			int contador = 0;

//			arrayResult.Add(_registro.QtdColuna);
//			arrayResult.Add(_registro.ValorColuna);
//			arrayResult.Add(_registro.ResultadoCalculoPin);
//			arrayResult.Add(_registro.ResultadoCalculoValores);

//			while (contador <= _arrayLinha.Count)
//			{
//				if (_arrayLinha[posicao].ToString() == "0" && !bTotalizadores)
//				{
//					contador = posicao;

//					for (int l = 0; l < arrayResult.Count; l++)
//					{
//							_arrayLinha[contador] = arrayResult[l].ToString();
//						contador++;
//					}

//					break;
//				}

//				if (bTotalizadores)
//				{
//					contador = arrayResult.Count - 1;

//					for (int i = _arrayLinha.Count - 1; (i + arrayResult.Count) > _arrayLinha.Count - 1; i--)
//					{
//						_arrayLinha[i] = arrayResult[contador].ToString();
//						contador--;
//					}

//					break;

//				}

//				contador++;
//			}

//			return _arrayLinha;

//		}

//		private int AplicarRegrasPeriodo(DateTime dtInicio, DateTime dtFim)
//		{
//			#region Regra de Interface RI10 para formatar os relatorio de detalhamento

//			// Regra 1 - Relatorio Detalhado por Mês
//			// Quando intervalo de datas inicio e fim ocorrer meses distintos
//			// retorno 1 - Mês

//			if (int.Parse(dtInicio.Month.ToString()) != int.Parse(dtFim.Month.ToString()))
//			{
//				return (int)EnumRegrasFiltro.MES;
//			}

//			// Regra 2- Relatorio Detalhado por Dia
//			// Quando intervalo de datas inicio e fim envolvam dias do mesmo mes
//			// retorno 2 - Dia
//			else if ((int.Parse(dtInicio.Month.ToString()) == int.Parse(dtFim.Month.ToString())) &&
//					(int.Parse(dtInicio.Day.ToString()) != int.Parse(dtFim.Day.ToString())))
//			{
//				return (int)EnumRegrasFiltro.DIA;
//			}

//			// Regra 3 - Relatorio Detalhado por Hora
//			// Quando intervalo de datas inicio e fim envolvam o mesmo dia
//			// retorno 3 - Hora
//			else if ((int.Parse(dtInicio.Month.ToString()) == int.Parse(dtFim.Month.ToString())) &&
//					(int.Parse(dtInicio.Day.ToString()) == int.Parse(dtFim.Day.ToString())))
//			{
//				return (int)EnumRegrasFiltro.HORA;
//			}
//			// Sem regra definida ==>> Data Invalida
//			else
//			{
//				return 0;
//			}

//			#endregion
//		}

//		private void AplicarCabecalhoDetalhadoColunasMerge(ExcelWorksheet worksheet, string columnPos,
//													 string value, string color)
//        {
//            // Formatar Cabecalho com merge das colunas com os Periodos MES, DIA E HORA
//			worksheet.Cells[columnPos].Style.Font.Name = "Calibri";
//			worksheet.Cells[columnPos].Style.Font.Size = 14;
//			worksheet.Cells[columnPos].Style.Font.Bold = true;
//			worksheet.Cells[columnPos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

//			worksheet.Cells[columnPos].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[columnPos].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[columnPos].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//			worksheet.Cells[columnPos].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


//			worksheet.Cells[columnPos].Value = value;
//		}

//		private void AplicarCabecalhoDetalhadoVist(ExcelWorksheet worksheet, string columnPos,
//													 string value)
//		{
//			// Formatar Cabecalho com merge das colunas com os Periodos MES, DIA E HORA
//			worksheet.Cells[columnPos].Style.Font.Name = "Calibri";
//			worksheet.Cells[columnPos].Style.Font.Size = 22;
//			worksheet.Cells[columnPos].Style.Font.Bold = true;
//			worksheet.Cells[columnPos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

//			worksheet.Cells[columnPos].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[columnPos].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//			worksheet.Cells[columnPos].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


//			worksheet.Cells[columnPos].Value = value;
//		}

//		private void AplicarCabecalhoDetalhadoColunas(ExcelWorksheet worksheet, int startRow,string columnPos,
//													 string value,string color)
//		{
//			// Formatar Cabecalho Com os Periodos MES, DIA E HORA
//			worksheet.Cells[columnPos + startRow].Style.Font.Name = "Calibri";
//			worksheet.Cells[columnPos + startRow].Style.Font.Size = 14;
//			worksheet.Cells[columnPos + startRow].Style.Font.Bold = true;
//			worksheet.Cells[columnPos + startRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

//			worksheet.Cells[columnPos + startRow].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[columnPos + startRow].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[columnPos + startRow].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + startRow].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + startRow].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + startRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + startRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

//			worksheet.Cells[columnPos + startRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center; 
//			worksheet.Cells[columnPos + startRow].Value = value;
//		}

//		private void AplicarLinhasDetalhadoColunas(ExcelWorksheet worksheet, int startRow,
//														int rowCount, string columnPos, string color, string value,
//														bool bold, bool italic, bool centered)
//		{
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Name = "Calibri";
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Size = 14;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Bold = bold;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Italic = italic;

//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			if (centered)
//			{
//				worksheet.Cells[columnPos + (startRow + rowCount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//			}
//			else
//			{
//				worksheet.Cells[columnPos + (startRow + rowCount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
//			}
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//			if (value.Length > 6)
//			{
//				if (ValidarValorColuna(value))
//				{
//					worksheet.Cells[columnPos + (startRow + rowCount)].Style.Numberformat.Format = "###,###,##0.00";
//					worksheet.Cells[columnPos + (startRow + rowCount)].Value = decimal.Parse(value);
//				}
//				else
//				{
//					worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//				}
//			}
//			else
//			{
//				worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//			}
//		}

//		private void AplicarLinhasDetalhadoColunas2(ExcelWorksheet worksheet, int startRow,
//														int rowCount, int columnPos, string color, string value,
//														bool bold, bool italic, bool centered)
//		{
//			worksheet.Cells[(startRow + rowCount), columnPos ].Style.Font.Name = "Calibri";
//			worksheet.Cells[(startRow + rowCount), columnPos ].Style.Font.Size = 14;
//			worksheet.Cells[ (startRow + rowCount),columnPos].Style.Font.Bold = bold;
//			worksheet.Cells[ (startRow + rowCount),columnPos].Style.Font.Italic = italic;

//			worksheet.Cells[ (startRow + rowCount),columnPos].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[ (startRow + rowCount),columnPos].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[ (startRow + rowCount),columnPos].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[ (startRow + rowCount),columnPos].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[ (startRow + rowCount),columnPos].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[ (startRow + rowCount),columnPos].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			if (centered)
//			{
//				worksheet.Cells[ (startRow + rowCount),columnPos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//			}
//			else
//			{
//				worksheet.Cells[(startRow + rowCount), columnPos ].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
//			}
//			worksheet.Cells[(startRow + rowCount), columnPos ].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//			if (value.Length > 6)
//			{
//				if (ValidarValorColuna(value))
//				{
//					worksheet.Cells[(startRow + rowCount), columnPos ].Style.Numberformat.Format = "###,###,##0.00";
//					worksheet.Cells[(startRow + rowCount), columnPos ].Value = decimal.Parse(value);
//				}
//				else
//				{
//					worksheet.Cells[(startRow + rowCount), columnPos ].Value = value;
//				}
//			}
//			else
//			{
//				worksheet.Cells[(startRow + rowCount), columnPos ].Value = value;
//			}
//		}
//		private void AplicarLinhasColunasTotais(ExcelWorksheet worksheet, int startRow, int rowCount, string columnPos, string color, string value, decimal numberValue = 0, string fontColor = "#FFFFFF", ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Center)
//		{

//			if (ValidarValorColuna(value))
//			{
//				worksheet.Cells[columnPos + (startRow + rowCount)].Style.Numberformat.Format = "###,###,##0.00";
//				worksheet.Cells[columnPos + (startRow + rowCount)].Value = decimal.Parse(value);
//			}
//			else
//			{
//				worksheet.Cells[columnPos + (startRow + rowCount)].Value = value;
//			}


//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Name = "Calibri";
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Size = 22;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(fontColor));

//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(color));
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.HorizontalAlignment = align;
//			worksheet.Cells[columnPos + (startRow + rowCount)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

//		}

//		private bool ValidarValorColuna(string valor)
//        {
//			bool bRet = false;

//			var splitValor = valor.Split(',');

//			var ret = valor.IndexOf("%");

//			if (ret > -1)
//			{
//				bRet = false;

//			}
//			else
//			{

//				if (splitValor.Length > 1)
//				{
//					bRet = true;
//				}
//				else
//				{
//					bRet = false;
//				}
//			}

//			return bRet;
//        }

//		private decimal DivisaoSegura(decimal value1,decimal value2)
//		{
//			if(value2 > 0)
//			{
//				return value1 / value2;
//			}
//			return 0;
//		}

//		private float DivisaoSegura(int value1, float value2)
//		{
//			if (value2 > 0)
//			{
//				return value1 / value2;
//			}
//			return 0;
//		}
//	}
//}