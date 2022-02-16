import { Component, ElementRef, OnInit, EventEmitter, Input, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
	selector: 'app-calendar',
	templateUrl: './calendar.component.html',
	styleUrls: ['./calendar.component.css'],
	providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: CalendarComponent, multi: true }],
})
export class CalendarComponent implements ControlValueAccessor, OnInit {
	@Input() isDisabled = false;
	@Input() isDisabledWeekend = false;
	@Input() isMaxDateCurrent: boolean;
	@Input() isMinDateCurrent: boolean;
	@Input() isMultiDate = false;
	@Input() isSelectMonth = true;
	@Input() maxDate?: Date;
	@Input() minDate?: Date;

	@Output() onNextDate: EventEmitter<any> = new EventEmitter();
	@Output() onPreviousDate: EventEmitter<any> = new EventEmitter();
	@Output() onSelectedDate: EventEmitter<any> = new EventEmitter();

	date: Date;
	isValidDate = false;
	required: any;
	selectedDate: Date;
	selectedDates: any[] = new Array<any>();
	validDates: Date[];
	weekdays = new Array<Weekday>();
	weeks = new Array<Week>();

	ngOnInit(): void {
		this.initLoad();
	}

	initLoad() {
		const nowDate = new Date(this.date.getFullYear(), this.date.getMonth(), this.date.getDate());
		if (this.isMinDateCurrent) { this.minDate = nowDate; }
		if (this.isMaxDateCurrent) { this.maxDate = nowDate; }
	}

	constructor(private el: ElementRef) {
		this.required = el.nativeElement.attributes['required'];
		this.generateCalendar();
	}

	writeValue(obj: any): void {
		this.selectedDate = obj;

		if (obj) {
			this.date.setMonth(obj.getMonth());
			this.date.setFullYear(obj.getFullYear());
		}
	}

	onChange(value: any) { }

	registerOnChange(fn: any): void { this.onChange = fn; }

	registerOnTouched(fn: any): void { }

	setDisabledState(isDisabled: boolean): void { }

	capitalize(text: string) {
		return text.charAt(0).toUpperCase() + text.slice(1);
	}

	setValidDates(dates?: Date[]) {
		if (dates) { this.validDates = dates; }
		this.isValidDate = true;
	}

	isDisabledDate(day: number) {
		const currentDate = new Date(this.date.getFullYear(), this.date.getMonth(), day);
		const minDate = this.minDate ? new Date(this.minDate.getFullYear(), this.minDate.getMonth(), this.minDate.getDate()) : undefined;
		const maxDate = this.maxDate ? new Date(this.maxDate.getFullYear(), this.maxDate.getMonth(), this.maxDate.getDate()) : undefined;

		if (this.isDisabledWeekend && (currentDate.getDay() == 6 || currentDate.getDay() == 0)) {
			return true;
		}

		return (this.minDate && currentDate < minDate) ||
			(this.maxDate && currentDate > maxDate) ||
			(
				(this.isValidDate && !this.validDates) ||
				(this.validDates && !this.validDates.find(function (x) {
					return x.getDate() == currentDate.getDate() &&
						x.getMonth() == currentDate.getMonth() &&
						x.getFullYear() == currentDate.getFullYear();
				}))
			);
	}

	selectDate(day: number) {
		if (this.isMultiDate) {
			let date = this.selectedDates.find(x => x.day == day);
			let index = this.selectedDates.findIndex(x => x.day == day);

			if (!date) { date = { day: day }; }

			if (index == -1) { index = this.selectedDates.length; }

			date.selected = !date.selected;

			this.selectedDates[index] = date;
		} else {
			this.selectedDate = new Date(this.date.getFullYear(), this.date.getMonth(), day);
			this.onSelectedDate.emit(this.selectedDate);
			this.onChange(this.selectedDate);
		}
	}

	getDates() {
		return this.selectedDates.filter(x => x.selected == true);
	}

	isCurrentDate(day: number) {
		if (this.isMultiDate) {
			return (
				this.selectedDates &&
				this.selectedDates.find(x => x.day == day) &&
				this.selectedDates.find(x => x.day == day).selected == true
			);
		} else {
			return (
				this.selectedDate &&
				this.selectedDate.getDate() == day &&
				this.selectedDate.getMonth() == this.date.getMonth() &&
				this.selectedDate.getFullYear() == this.date.getFullYear());
		}
	}

	previousDate() {
		this.date.setMonth(this.date.getMonth() - 1);
		this.onPreviousDate.emit(this.date);
		this.generateWeeks();
	}

	nextDate() {
		this.date.setMonth(this.date.getMonth() + 1);
		this.onNextDate.emit(this.date);
		this.generateWeeks();
	}

	monthName() {
		return this.capitalize(this.date.toLocaleString(navigator.language, { month: 'long' }));
	}

	generateCalendar(dateCalendar?: Date) {
		this.date = !dateCalendar ? new Date() : dateCalendar;
		this.generateWeekDays();
		this.generateWeeks();
	}

	generateWeekDays() {
		const date = new Date();

		this.weekdays = new Array<Weekday>();

		date.setDate(date.getDate() + (7 - date.getDay()) % 7);

		for (let i = 0; i < 7; i++) {
			this.weekdays.push(new Weekday(
				date.toLocaleString(window.navigator.language, { weekday: 'narrow' }),
				date.toLocaleString(window.navigator.language, { weekday: 'long' })
			));

			date.setDate(date.getDate() + 1);
		}
	}

	generateWeeks() {
		this.weeks = new Array<Week>();
		const firstDate = new Date(this.date.getFullYear(), this.date.getMonth(), 1);
		const lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 1, 0);
		const numberDays = lastDate.getDate();
		let start = 1;
		let end = 7 - firstDate.getDay();

		while (start <= numberDays) {
			const week = new Week();

			for (let i = start; i <= end; i++) {
				week.days.push(i);
			}

			this.weeks.push(week);

			start = end + 1;
			end = end + 7;

			if (end > numberDays) {
				end = numberDays;
			}
		}
	}
}

export class Weekday {
	constructor(public shortName: string, public longName: string) { }
}

export class Week {
	days: number[] = new Array<number>();
}
