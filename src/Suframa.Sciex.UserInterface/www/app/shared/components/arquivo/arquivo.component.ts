import { Component, Input, ElementRef, Injectable, Output, EventEmitter, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { arquivoVM } from '../../../view-model/ArquivoVM';
import { Observable } from 'rxjs/Observable';
import { ModalService } from '../../services/modal.service';
import { ArquivoService } from '../../services/arquivo.service';
import { MessagesService } from '../../services/messages.service';
import { ApplicationService } from '../../services/application.service';

@Component({
	selector: 'app-arquivo',
	templateUrl: './arquivo.component.html',
	styleUrls: ['./arquivo.component.css'],
	providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: ArquivoComponent, multi: true }]
})

@Injectable()
export class ArquivoComponent implements ControlValueAccessor {
	@Input() autoUpload = false;
	@Input() isDisabled = false;
	@Input() isRequired = false;
	@Input() isSimpleMode = false;
	@Input() types: string[] = ['PDF'];

	@Output() fileChange: EventEmitter<any> = new EventEmitter();

	arquivo: any;
	data: any;
	model: arquivoVM = new arquivoVM();

	@ViewChild('fileInput') fileInput;

	constructor(
		private arquivoService: ArquivoService,
		private elm: ElementRef,
		private modal: ModalService,
		private msg: MessagesService,
	) { }

	writeValue(obj: any): void {
		const fileList = obj.target.files;

		if (fileList.length > 0) {
			const file = fileList[0];

			if (!this.fileTypeRules(file)) { return; }

			if (!this.fileSizeRule(file)) { return; }

			if (this.autoUpload) {
				this.arquivoService.upload(file)
					.subscribe(result => {
						this.model.nome = result.nome;
						this.fileChange.emit(result);
					});
			} else {
				this.model.nome = file.name;
				this.fileChange.emit(file);
			}
		}
	}

	onChange(value) {
		this.writeValue(value);
	}

	cleanFile() {
		this.model = new arquivoVM();
		this.fileInput.nativeElement.value = '';
	}

	registerOnChange(fn: any): void { this.onChange = fn; }

	registerOnTouched(fn: any): void { }

	setDisabledState(isDisabled: boolean): void { }

	fileSizeRule(file: any) {
		if (file.size > 20971520) {
			this.modal.alerta(this.msg.TAMANHO_DOCUMENTO_INVALIDO);
			this.cleanFile();
			return false;
		}
		return true;
	}

	fileTypeRules(file: any) {
		let msg = '';
		if (!this.types) {
			this.modal.alerta(this.msg.NENHUM_FORMATO_DE_ARQUIVO_CONFIGURADO);
			return false;
		}

		const typesUpper = this.types.map(function (x) { return x.toUpperCase(); });

		if (file.type != 'application/pdf' &&
			typesUpper.indexOf('PDF') >= 0) {
			msg = this.msg.FAVOR_SELECIONAR_UM_ARQUIVO_NO_FORMATO_PDF;

			if (typesUpper.length == 1) {
				return this.alertMsg(msg);
			}
		}

		if (file.type != 'image/png' &&
			typesUpper.indexOf('PNG') >= 0) {
			msg = this.msg.FAVOR_SELECIONAR_UM_ARQUIVO_NO_FORMATO.replace('{0}', 'PNG');

			if (typesUpper.length == 1) {
				return this.alertMsg(msg);
			}
		}

		if (file.type != 'application/pdf' && typesUpper.indexOf('PDF') >= 0 &&
			file.type != 'image/jpeg' && typesUpper.indexOf('JPG') >= 0) {
			msg = this.msg.FAVOR_SELECIONAR_UM_ARQUIVO_NO_FORMATO_PDF_OU_JPG;
		} else {
			msg = '';
		}

		if (msg != '') {
			return this.alertMsg(msg);
		}

		return true;
	}

	alertMsg(msg: string) {
		this.modal.alerta(msg);
		this.cleanFile();
		return false;
	}
}
