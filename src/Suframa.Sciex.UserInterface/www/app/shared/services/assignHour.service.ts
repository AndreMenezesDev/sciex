import { Injectable } from "@angular/core";

@Injectable()
export class AssignHour{
    constructor(){}

    getHourCurrent(dateObj: Date){
        var month = dateObj.getUTCMonth().toString().length <= 1 ? '0' + (dateObj.getUTCMonth() + 1).toString() : dateObj.getUTCMonth() + 1;
        var day = dateObj.getUTCDate();
        var year = dateObj.getUTCFullYear();

        var hh = (dateObj.getHours() < 10 ? '0' : '') + dateObj.getHours();
        var mm = (dateObj.getMinutes() < 10 ? '0' : '') + dateObj.getMinutes();
        var ss = (dateObj.getSeconds() < 10 ? '0' : '') + dateObj.getSeconds();

        var newhr = hh + ":" + mm + ":" + ss;

        var newdate = day + "/" + month + "/" + year;

        return {newhr, newdate};
    }

}