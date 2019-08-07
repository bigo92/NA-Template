import { Pipe, PipeTransform } from '@angular/core';

declare var $;
@Pipe({
  name: 'date2'
})
export class Date2Pipe implements PipeTransform {

  lstDate: any[] = [
    {key: 1, value: 'st'},
    {key: 11, value: 'st'},
    {key: 21, value: 'st'},
    {key: 31, value: 'st'},
    {key: 2, value: 'nd'},
    {key: 12, value: 'nd'},
    {key: 22, value: 'nd'},
    {key: 3, value: 'rd'},
    {key: 13, value: 'rd'},
    {key: 23, value: 'rd'},
  ];
  
  transform(value: any, args?: any): any {
    if (value) {
      let formatted = $.datepicker.formatDate(args, new Date(value + '')) + '';
      // const day = parseInt(formatted.split(' ')[0]);
      // const indexDay = this.lstDate.findIndex(x => x.key === day);
      // if (indexDay !== -1)
      //   {
      //     formatted = formatted.replace(' ', this.lstDate[indexDay].value + ' ');
      //   }else{
      //     formatted = formatted.replace(' ', 'th ');
      //   }
      return formatted;
    }
  }

}