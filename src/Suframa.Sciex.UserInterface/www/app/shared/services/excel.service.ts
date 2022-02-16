
import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
import { forEach } from '@angular/router/src/utils/collection';
import { map } from 'rxjs/operator/map';
import { filter } from 'rxjs/operators';
import { print } from 'util';

@Injectable()
export class ExcelService {

	constructor() {

	}

	static toExportFileName(excelFileName: string): string {
		return `${excelFileName}_listagem_${new Date().getTime()}.xlsx`;
	}

	public exportAsExcelFile(json: any[], excelFileName: string, titulo: string): void {

		//Conta a quantidade de colunas existentes no relatório
		var tam = json[0].length - 1;
		var column, i;

		for (i = 0; i < tam; i++) {

			column = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Z"];
		}

		var ws = XLSX.utils.json_to_sheet(json);

		/* merge cells A1:columns until last cell */
		var merge = XLSX.utils.decode_range("A1:" + column[i] + "1"); // this is equivalent
		if (!ws['!merges']) ws['!merges'] = [];
		ws['!merges'].push(merge);


		//Deleta o conteúdo que está na coluna C1 e substitui pelo 'titulo'
		delete ws['A1'].w; ws['A1'].v = titulo;

		const worksheet: XLSX.WorkSheet = ws;

		//Tamanho
		//var wscols = [
		//	{ wch: 30 },
		//	{ wch: 50 },
		//	{ wch: 30 },
		//	{ wch: 30 }
		//];

		//ws['!cols'] = wscols;

		const workbook: XLSX.WorkBook = { Sheets: { 'Registros': worksheet }, SheetNames: ['Registros'] };
		const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });

		XLSX.writeFile(workbook, ExcelService.toExportFileName(excelFileName));
	}



}
