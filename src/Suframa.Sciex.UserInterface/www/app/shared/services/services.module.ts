import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// ORDEM ALFABÉTICA
import { ApplicationService } from './application.service';
import { ArquivoService } from './arquivo.service';
import { ArrayService } from './array.service';
import { AuthenticationService } from './authentication.service';
import { ConverterService } from './converter.service';
import { DateService } from './date.service';
import { ExtractNumberService } from './extract-number.service';
import { GeneratorService } from './generator.service';
import { InformationService } from './information.service';
import { LoadingService } from './loading.service';
import { MessagesService } from './messages.service';
import { ModalService } from './modal.service';
import { ObjectService } from './object.service';
import { PessoaService } from './pessoa.service';
import { StringService } from './string.service';
import { ValidationService } from './validation.service';
import { ExcelService } from './excel.service';
import { PDFService } from './pdf.service';
import { FormatCodeService } from './format-code.service';
import { ApiService } from './api.service';
import { AssignHour } from './assignHour.service';


@NgModule({
	imports: [
		CommonModule,
	],
	declarations: [],
	providers: [
		ApplicationService,
		ArquivoService,
		ArrayService,
		AuthenticationService,
		ConverterService,
		DateService,
		ExtractNumberService,
		GeneratorService,
		InformationService,
		LoadingService,
		MessagesService,
		ModalService,
		ObjectService,
		PessoaService,
		StringService,
		ValidationService,
		ExcelService,
		PDFService,
		FormatCodeService,
		ApiService,
		AssignHour

	]
})
export class ServicesModule { }
