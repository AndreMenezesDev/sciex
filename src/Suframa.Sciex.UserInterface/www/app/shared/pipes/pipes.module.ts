import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BooleanoTextoPipe } from './booleano-texto.pipe';
import { CodigoNaturezaJuridicaPipe } from './codigo-natureza-juridica.pipe';
import { CepPipe } from './cep.pipe';
import { CnpjPipe } from './cnpj.pipe';
import { CpfPipe } from './cpf.pipe';
import { SafeHtmlPipe } from './safeHtml.pipe';
import { EnumPipe } from './enum.pipe';
import { ProtocoloPipe } from './protocolo.pipe';
import { GroupByPipe } from './group-by.pipe';
import { PadLeftPipe } from './pad-left.pipe';
import { MonthPipe } from './month.pipe';
import { PorcentagemPipe } from './porcentagem.pipe';
import { FormatCodePipe } from './format-code.pipe';
import { SearchFilterPipe } from './filter-pipe';
import { LetterBoldPipe } from './letter-bold.pipe';
import { FormatDatePipe } from './format-date.pipe';
import { NcmPipe } from './ncm.pipe';
import { DuePipe } from './due.pipe';

@NgModule({
	imports: [
		CommonModule,
	],
	exports: [
		BooleanoTextoPipe,
		CepPipe,
		CnpjPipe,
		CodigoNaturezaJuridicaPipe,
		CpfPipe,
		EnumPipe,
		GroupByPipe,
		MonthPipe,
		PadLeftPipe,
		PorcentagemPipe,
		ProtocoloPipe,
		SafeHtmlPipe,
		FormatCodePipe,
		SearchFilterPipe,
		LetterBoldPipe,
		FormatDatePipe,
		NcmPipe,
		DuePipe
	],
	declarations: [
		BooleanoTextoPipe,
		CepPipe,
		CnpjPipe,
		CodigoNaturezaJuridicaPipe,
		CpfPipe,
		EnumPipe,
		GroupByPipe,
		MonthPipe,
		PadLeftPipe,
		PorcentagemPipe,
		ProtocoloPipe,
		SafeHtmlPipe,
		FormatCodePipe,
		SearchFilterPipe,
		LetterBoldPipe,
		FormatDatePipe,
		NcmPipe,
		DuePipe
	],
})
export class PipesModule { }
