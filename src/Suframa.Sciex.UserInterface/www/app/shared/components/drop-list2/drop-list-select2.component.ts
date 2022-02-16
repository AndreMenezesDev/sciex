import { Component, HostListener, Input, OnInit, ElementRef, OnChanges, Output, EventEmitter, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../environments/environment';
import { ArrayService } from '../../services/array.service';
import { ApplicationService } from '../../services/application.service';
import { observeOn } from 'rxjs/operators';

@Component({
	selector: 'app-drop-list-select2',
	templateUrl: './drop-list-select2.component.html',
	styleUrls: ['./drop-list-select2.component.css'],
	providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: DropListSelect2Component, multi: true }],
})

export class DropListSelect2Component implements OnInit, OnChanges {
	@Input() isDisabled = false;
	@Input() isRequired = false;
	@Input() lazy = true;
	@Input() loadWhenParameterEmpty: boolean;
	@Input() parametro: any;
	@Input() parametroChave: string;
	@Input() parametro1: any;
	@Input() parametroChave1: string;
	@Input() parametro2: any;
	@Input() parametroChave2: string;
	@Input() parametro3: any;
	@Input() parametroChave3: string;
	@Input() placeholder: string;
	@Input() servico: string;
	@Input() selMinimumInputLength: number;
	@Input() idSelecionado: number;
	@Input() isCloseNormal: boolean = false;

	@ViewChild('selecao') selecao;
	@Input("valorSelecionadoParent")
	valorSelecionado: string;

	@Input() clearInput: boolean;

	@Output() valorModificado: EventEmitter<string> = new EventEmitter<string>();
	@Output() onSelected: EventEmitter<any> = new EventEmitter();
	@Output() limparObjetosAuxiliares: EventEmitter<string> = new EventEmitter<string>();

	@Input()
	@ViewChild('valorInput') valorInput;

	list: any = new Array<any>();
	listAutoComplete: any = new Array<any>();
	required: any;
	settings: any = {};
	data: any = [];
	options: any;
	i: any;
	parametros: any = {};
	model: any = '';
	valor: string;
	openDrop: boolean = false;
	focused: boolean = false;
	clearValue: boolean = false;
	whiteInputValue: boolean = false;

	mensagem: string;
	mensagemError: string;



	constructor(
		private applicationService: ApplicationService,
		private arrayService: ArrayService,
		private elm: ElementRef,
	) {

	}

	ngOnInit() {

		this.selMinimumInputLength = this.selMinimumInputLength || 3;

		this.mensagemError = "Nenhum registro encontrado.";
		this.mensagem = this.mensagem || this.getMensagemInformacao();

	}

	@HostListener('document:click', ['$event'])
	documentClick(event: MouseEvent) {
		if (this.clearInput) {
			this.onClear(true);
			this.onBlur();
		}
	}

	getMensagemInformacao() {
		var a = "Digite e pressione enter";
		return a
	}



	getCount() {
		return this.list ? this.list.length : 0;
	}

	reset() {

		this.list = null;
		this.listAutoComplete = null;
		this.model = '';
	}

	listItemsObservable() {

		return Observable.create(observer => {
			this.callService(this.urlService(), observer);
		});
	}

	urlService() {

		let url = `${this.servico}DropDown/`;

		//if (this.parametroChave && this.parametro) {
		//	url = url + '?' + this.parametroChave + '=' + this.parametro;
		//} else {
		//	if (!this.parametroChave && this.parametro) {
		//		url = url + this.parametro;
		//	}
		//}

		return url;
	}

	public onClear(limpar) {
		this.valorSelecionado = "";
		this.valorModificado.emit(null);
		this.limparObjetosAuxiliares.emit(null);
		if (limpar)
			this.valorInput.nativeElement.value = "";
		this.list = null;
		this.model = "";
		this.clearValue = false;
		this.openDrop = false;
		this.focused = false;
		this.mensagem = this.getMensagemInformacao();
	}

	onBlur() {
		this.focused = this.list != undefined && this.list.length > 0 ? true : false;
		if (!this.isDisabled && (this.idSelecionado == undefined || this.idSelecionado == null)) {
			if (this.valorInput.nativeElement.value == "") {
				this.valorModificado.emit(null);
				this.limparObjetosAuxiliares.emit(null);
				this.valorInput.nativeElement.value = "";
				this.mensagem = this.getMensagemInformacao();
			}
		}
	}

	onFocus() {
		this.focused = true;
	}

	replaceText() {
		if (this.valorInput.nativeElement.value.length == 1 && this.valorInput.nativeElement.value == " ")
			this.trimText();
	}

	trimText() {
		(this.valorInput.nativeElement as HTMLInputElement).value = (this.valorInput.nativeElement as HTMLInputElement).value.trim();
	}


	onKeyUp($event: KeyboardEvent) {

		try {
			if (this.isDisabled) {
				return;
			}

			if (this.valorInput.nativeElement.value == undefined || this.valorInput.nativeElement.value.length < this.selMinimumInputLength) {
				this.list = null;
				this.clearValue = false;
				this.openDrop = false;
				this.mensagem = this.getMensagemInformacao();
				this.valorModificado.emit(null);
				return;
			}

			if (this.valorInput.nativeElement.value.length > 0) {
				this.valorInput.nativeElement.setCustomValidity('');
			}
			this.replaceText();
			this.parametros.id = null;

			this.parametros.descricao = this.valorInput.nativeElement.value;
			if (this.valorInput.nativeElement.value.length >= this.selMinimumInputLength) {
				this.callService(this.urlService(), this.parametros);
				this.clearValue = true;
			}

			setTimeout(() => { this.valorInput.nativeElement.focus(); });

		} catch (e) {
			alert(e);
		}
		
		

	}


	onKeyDown($event: KeyboardEvent) {

		if (this.isDisabled) {
			return;
		}

		//	var regexStr = '[()&-_,!ãÃâÂáÁàÀêÊéÉèÈîÎíÍìÌõÕôÔóÓòÒûÛúÚùÙÇça-zA-Z0-9 ]';

		const e = <KeyboardEvent>$event;

		if (e.keyCode == 8) {
			this.onClear(false);
		}

		if ([46, 8, 9, 27, 13, 110, 190].indexOf(e.keyCode) != -1 ||
			// Allow: Ctrl+A
			(e.keyCode == 65 && e.ctrlKey == true) ||
			// Allow: Ctrl+C
			(e.keyCode == 67 && e.ctrlKey == true) ||
			// Allow: Ctrl+V
			(e.keyCode == 86 && e.ctrlKey == true) ||
			// Allow: Ctrl+X
			(e.keyCode == 88 && e.ctrlKey == true) ||
			// Allow: home, end, left, right
			(e.keyCode >= 35 && e.keyCode <= 39)) {
			// let it happen, don't do anything
			return;
		}

		//const regEx = new RegExp(regexStr);

		this.replaceText();

		//if (!regEx.test(e.key)) {
		//	e.preventDefault();
		//}


		if (this.valorInput.nativeElement.value.length < this.selMinimumInputLength) {
			this.clearValue = false;
			this.list = null;
			this.openDrop = false;
			this.mensagem = this.getMensagemInformacao();
			this.valorModificado.emit(null);
			return;
		}
		else {
			if (this.valorInput.nativeElement.value.length > 0)
				this.clearValue = true;
		}

	}

	callService(url, parametros) {
				
		if ((parametros == undefined || parametros == null) && !this.whiteInputValue) {

			this.onClear(true);
			return;
		}

		this.parametros = parametros;

		this.valorInput.nativeElement.disabled = true;


		if (this.parametros.id == undefined) {
			this.valor = JSON.parse(JSON.stringify(this.parametros.descricao));
			this.parametros.descricao = this.valor.replace("+", "%2B");
		}
		else {
			this.valor = this.parametros.descricao;
		}

		if (!this.whiteInputValue && this.parametroChave != "" && this.parametros.id == undefined) {

			url = url + '?' + this.parametroChave + '=' + this.parametro;
			url = url + '&' + "Descricao" + '=' + this.parametros.descricao;

			if (this.parametroChave1 != "") {
				url = url + '&' + this.parametroChave1 + '=' + this.parametro1;
			}

			if (this.parametroChave2 != "") {
				url = url + '&' + this.parametroChave2 + '=' + this.parametro2;
			}

			this.applicationService.get(url).subscribe(result => {

				this.list = result;
				if (result[0] != undefined && this.list.length > 0) {

					if (this.whiteInputValue) {

						this.openDrop = false;
						this.clearValue = true;
						this.focused = false;
						this.valorModificado.emit(result[0].id);
						this.valorInput.nativeElement.value = result[0].text;
						this.model = result[0].text;
						this.whiteInputValue = false;


					} else {
						this.openDrop = true;

					}

					this.mensagem = this.getMensagemInformacao();
				}
				else {
					this.openDrop = true;
					this.list = null;
					this.mensagem = this.mensagemError;
				}
				this.valorInput.nativeElement.disabled = false;
				if (this.openDrop) {

					this.valorInput.nativeElement.focus();
					this.focusOnFirstLi();
				}
				else {
					this.list = null;
				}

			});
		}
		else {

			if(this.parametroChave != "" && this.parametroChave != undefined) {
				this.parametros[this.parametroChave] = this.parametro;
			}

			if (this.parametroChave1 != "") {
				this.parametros[this.parametroChave1] = this.parametro1;
			}

			if (this.parametroChave2 != "") {
				this.parametros[this.parametroChave2] = this.parametro2;
			}

			if (this.parametroChave3 != "") {
				this.parametros[this.parametroChave3] = this.parametro3;
			}

			this.applicationService.get(url, this.parametros).subscribe(result => {

				this.list = result;
				if (result[0] != undefined && this.list.length > 0) {

					if (this.whiteInputValue) {

						this.openDrop = false;
						this.clearValue = true;
						this.focused = false;
						this.valorModificado.emit(result[0].id);
						this.valorInput.nativeElement.value = result[0].text;
						this.model = result[0].text;
						this.whiteInputValue = false;


					} else {
						this.openDrop = true;

					}

					this.mensagem = this.getMensagemInformacao();
				}
				else {
					this.openDrop = true;
					this.list = null;
					this.mensagem = this.mensagemError;
				}
				this.valorInput.nativeElement.disabled = false;
				if (this.openDrop) {

					this.valorInput.nativeElement.focus();
					this.focusOnFirstLi();
				}
				else {
					this.list = null;
				}

			});
		}
		this.valorInput.nativeElement.value = this.valor;

	}

	//listItems() {
	//	return this.callService(this.urlService());
	//}

	removeItem(item: string) {
		this.arrayService.remove(this.list, 'id', item);

		if (this.list.length == 1) {
			this.model = this.list[0].id;
			this.onChange(this.list[0].id);
		}
	}

	addItem(item: any) {
		this.arrayService.add(this.list, 'id', item, false);
	}

	getAllItems() {
		return this.list;
	}

	getSelectedKey() { return this.model; }

	getSelectedItem() {
		const empty = { value: this.getSelectedKey() };

		if (!this.list) { return empty; }

		const item = this.list.filter(x => x.id == this.getSelectedKey());

		if (!item) { return empty; }

		return item[0];
	}

	getSelectedValue() {		
		if (this.getSelectedItem()) {
			return this.getSelectedItem().value;
		}
	}

	writeValue(obj: string): void {
		
		if (obj == undefined || obj == null
			|| this.valorInput.nativeElement.value == undefined) {
			return;
		}
		if (Object.values(obj).length < 20) {
			this.model = obj;
			this.whiteInputValue = true;
			this.parametros.id = this.model;
			this.parametros.descricao = null;



			this.callService(this.urlService(), this.parametros);
		}

	}

	ngOnChanges(changes: any) {
		if (changes.parametro) {
			if (!changes.parametro.firstChange) {
				if (changes.parametro.currentValue || this.loadWhenParameterEmpty) {
					//this.listItems();
				}
			}
		}
	}

	onChange(value) {		
		this.writeValue(value);
	}

	registerOnChange(fn: any): void { this.onChange = fn; }

	registerOnTouched(fn: any): void { }

	setDisabledState(isDisabled: boolean): void { }

	onInput(value) {		
		if (!value) {
			this.list = this.listAutoComplete;
			return;
		}

		if (this.list && this.list.length > 1) {
			this.list = this.listAutoComplete.filter(function (el) {
				return el.value.toLowerCase().indexOf(value.toLowerCase()) != -1;
			});
		}
	}

	onSelect(obj) {
		//this.model = `${id}`;

		this.valorModificado.emit(null);
		this.limparObjetosAuxiliares.emit(null);

		if (!obj) { obj = ''; }
		this.model = obj;

		this.valorSelecionado = obj.id;
		this.valorModificado.emit(obj.id);
		this.onSelected.emit(obj);
		this.valorInput.nativeElement.value = obj.text;

		this.list = null;
		this.openDrop = false;
		this.focused = false;
		var control = document.nextSibling;

	}

	public clear() {
		this.onClear(true);
	}

	focusOnFirstLi() {
		var that = this;
		//setTimeout(function () {
		//	that.selecao.nativeElement.getElementsByTagName("li")[0].focus();
		//}, 10);
	}


}
