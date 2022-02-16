import { Injectable } from '@angular/core';

@Injectable()
export class DateService {
	constructor() { }

	JoinDateTime(date: Date, hours?: string) {
		const day = parseInt(date.toString().split('-')[2], 10);
		const month = parseInt(date.toString().split('-')[1], 10) - 1;
		const year = parseInt(date.toString().split('-')[0], 10);

		if (hours == null) { hours = '00:00:00'; }

		const hour = parseInt(hours.split(':')[0], 10);
		const minute = parseInt(hours.split(':')[1], 10);
		const seconds = parseInt(hours.split(':')[2], 10);

		return new Date(
			year,
			month,
			day,
			hour, minute, seconds);
	}
}
