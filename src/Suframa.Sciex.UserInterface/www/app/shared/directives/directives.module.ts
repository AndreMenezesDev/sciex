import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CollapseDirective } from './collapse.directive';
import { CpfCnpjDirective } from './cpf-cnpj.directive';
import { DateDirective } from './date.directive';
import { DisableOnClickDirective } from './disable-onclick.directive';
import { EmailDirective } from './email.directive';
import { OnlyNumbersDirective } from './only-numbers.directive';
import { OrdenacaoDirective } from './ordenacao.directive';
import { PorcentagemDirective } from './porcentagem.directive';
import { RemoveHostDirective } from './remove-host.directive';
import { TelefoneDirective } from './telefone.directive';
import { NumeroProtocoloDirective } from './numero-protocolo.directive';
import { FormatarNumeroDirective } from './formatar-numero.directive';
import { InputTextFilterDirective } from './input-text-filter.directive';
import { ClickOutsideDirective } from './dropdown.directive';
import { FormatarNumeroPLIDirective } from './formatar-numero-pli.directive';
import { FormatarNumeroDecimal5Directive } from './formatar-numero-decimal-5.directive';
import { FormatarNumeroDecimal7Directive } from './formatar-numero-decimal-7.directive';
import { FormatarNumeroNCMDirective } from './formatar-numero-ncm.directive';
import { MaskedInputDirective } from './angular2TextMask.directive';
import { ApenasNumerosDirective } from './apenas-numeros.directive';
import { MascaraNcmDirective } from './mascara-ncm.directive';
import { UploadDirective } from './upload.directive';
import { DropZoneDirective } from './drop-zone.directive';
@NgModule({
	imports: [
		CommonModule
	],
	declarations: [
		CollapseDirective,
		CpfCnpjDirective,
		MascaraNcmDirective,
		DateDirective,
		DisableOnClickDirective,
		EmailDirective,
		NumeroProtocoloDirective,
		OnlyNumbersDirective,
		OrdenacaoDirective,
		PorcentagemDirective,
		RemoveHostDirective,
		FormatarNumeroDirective,
		TelefoneDirective,
		InputTextFilterDirective,
		ClickOutsideDirective,
		FormatarNumeroPLIDirective,
		FormatarNumeroDecimal5Directive,
		FormatarNumeroDecimal7Directive,
		FormatarNumeroNCMDirective,
		MaskedInputDirective,
		ApenasNumerosDirective,
		UploadDirective,
		DropZoneDirective
	],
	exports: [
		CollapseDirective,
		CpfCnpjDirective,
		MascaraNcmDirective,
		DateDirective,
		DisableOnClickDirective,
		EmailDirective,
		NumeroProtocoloDirective,
		OnlyNumbersDirective,
		OrdenacaoDirective,
		PorcentagemDirective,
		RemoveHostDirective,
		FormatarNumeroDirective,
		TelefoneDirective,
		InputTextFilterDirective,
		ClickOutsideDirective,
		FormatarNumeroPLIDirective,
		FormatarNumeroDecimal5Directive,
		FormatarNumeroDecimal7Directive,
		FormatarNumeroNCMDirective,
		MaskedInputDirective,
		ApenasNumerosDirective,
		UploadDirective,
		DropZoneDirective
	]
})
export class DirectivesModule { }
