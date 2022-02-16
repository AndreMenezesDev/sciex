import {
	Component, HostListener, Input, OnInit,
	ElementRef, OnChanges, Output, EventEmitter, ViewChild, Injectable
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FileUploader, FileLikeObject } from 'ng2-file-upload';
import { Observable } from 'rxjs';
import { HttpClient } from "@angular/common/http";
import { ModalService } from '../../services/modal.service';
import { MessagesService } from '../../services/messages.service';


declare var $: any;

@Component({
	selector: 'app-upload-file',
	templateUrl: './upload-file.component.html',
	styleUrls: ['./upload-file.component.css'],
	providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: UploadFileComponent, multi: true }],
})


export class UploadFileComponent implements OnInit {

	isFileUpload: boolean;
	uploadForm: FormGroup;
	errorMessage: string;

	@ViewChild("file") file;

	@Input() maximoTamanhoArquivo: number = 20 * 1024 * 1024; //20 mb default
	@Input() maximoArquivo: number = 1;
	@Input() tipoArquivo: string[] = ['application/x-zip-compressed'];
	public uploader: FileUploader;

	constructor(
		private modal: ModalService,
		private msg: MessagesService) {

	}

	ngOnInit() {

		this.uploader = new FileUploader({
			isHTML5: true, maxFileSize: this.maximoTamanhoArquivo, queueLimit: this.maximoArquivo
		});
		this.uploader.onWhenAddingFileFailed = (item, filter, options) => this.onWhenAddingFileFailed(item, filter, options);		
	}

	onFileChange(event) {
		let files = event.target.files;
		this.uploader.addToQueue(files);
		this.file.nativeElement.value = "";
	}

	public saveFiles() {
		for (var j = 0; j < this.uploader.queue.length; j++) {
			let data = new FormData();
			let fileItem = this.uploader.queue[j]._file;
			data.append('file', fileItem);
			data.append('fileSeq', 'seq' + j);
			data.append('dataType', this.uploadForm.controls.type.value);

		}
	}

	public isFiles() {
		return this.uploader.queue.length > 0;
	}

	public fileOverBase(e: any): void {
		this.isFileUpload = e;
	}

	confirmarExclusao(item) {
		
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.uploader.removeFromQueue(item);
					this.errorMessage = "";
				}
			});

	}

	public removerTodos() {
		this.errorMessage = "";

		try {
			this.uploader.clearQueue();
			this.file.nativeElement.value = "";
		} catch (e) {
			console.log(e);
		}
	}

	onWhenAddingFileFailed(item: FileLikeObject, filter: any, options: any) {
		this.errorMessage = "";
		switch (filter.name) {
			case 'fileSize':
				var valor = (this.maximoTamanhoArquivo / 1000000).toString().split(".")[0];
				this.errorMessage = `Tamanho do arquivo ultrapassou o limite permitido de ` + valor + 'MB, não é possível realizar envio';
				break;
			case 'mimeType':
				const allowedTypes = this.tipoArquivo.join();
				this.errorMessage = `O tipo "${item.type} não é aceito. Somente os tipos: "${allowedTypes}"`;
				break;
			case 'fileType':
				const allowedTypes1 = this.tipoArquivo.join();
				this.errorMessage = `O tipo "${item.type} não é aceito. Somente os tipos: "${allowedTypes1}"`;
				break;
			default:
				this.errorMessage = `Tipo de arquivo  (filter is ${filter.name})`;
		}
	}


}
