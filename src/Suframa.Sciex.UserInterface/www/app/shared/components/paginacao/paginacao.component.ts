import { Component, Input, Output, OnInit, EventEmitter, ChangeDetectionStrategy } from '@angular/core';

@Component({
	selector: 'app-paginacao',
	templateUrl: './paginacao.component.html',
})
export class PaginacaoComponent implements OnInit {
	@Input() pagerLeftArrowIcon: string;
	@Input() pagerNextIcon: string;
	@Input() pagerPreviousIcon: string;
	@Input() pagerRightArrowIcon: string;
	@Input() widthPagination: number;

	@Input()
	set size(val: number) {
		this._size = val;
		this.pages = this.calcPages();
	}

	@Input()
	set total(val: number) {
		this._total = val;
		this.pages = this.calcPages();
	}

	@Input()
	set page(val: number) {
		this._page = val;
		this.pages = this.calcPages();
	}

	@Output() change: EventEmitter<any> = new EventEmitter();

	_total = 0;
	_page = 1;
	_size = 0;
	pages: any;

	constructor() { }

	ngOnInit() {
		this.widthPagination = this.widthPagination || 10;
		this.size = this.size || 10;
		this.page = this.page || 1;
	}

	get size(): number {
		return this._size;
	}

	get total(): number {
		return this._total;
	}

	get page(): number {
		return this._page;
	}

	get totalPages(): number {
		const total = this.size < 1 ? 1 : Math.ceil(this.total / this.size);
		return Math.max(total || 0, 1);
	}

	canPrevious(): boolean {
		return this.page > 1;
	}

	canNext(): boolean {
		return this.page < this.totalPages;
	}

	prevPage(): void {
		this.selectPage(this.page - 1);
	}

	nextPage(): void {
		this.selectPage(this.page + 1);
	}

	selectPage(page: number): void {
		if (page > 0 && page <= this.totalPages && page != this.page) {
			this.page = page;
			this.change.emit(+page);
		}
	}

	calcPages(page?: number): any[] {
		const pages = [];
		let startPage = 1;
		let endPage = this.totalPages;
		const isMaxSized = this.widthPagination < this.totalPages;

		page = page || this.page;

		if (isMaxSized) {
			startPage = page - Math.floor(this.widthPagination / 2);
			endPage = page + Math.floor(this.widthPagination / 2);

			if (startPage < 1) {
				startPage = 1;
				endPage = Math.min(startPage + this.widthPagination - 1, this.totalPages);
			} else if (endPage > this.totalPages) {
				startPage = Math.max(this.totalPages - this.widthPagination + 1, 1);
				endPage = this.totalPages;
			}
		}

		for (let num = startPage; num <= endPage; num++) {
			pages.push({
				number: num,
				text: <string><any>num
			});
		}

		return pages;
	}
}
