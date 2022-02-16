import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'groupBy', pure: false })
export class GroupByPipe implements PipeTransform {
	transform(items: Array<any>, conditions: { [field: string]: any }): Array<any> {
		if (items != undefined) {
			return items.filter(item => {
				for (const field in conditions) {
					if (item[field] != conditions[field]) {
						return false;
					}
				}
				return true;
			});
		}
	}
}
