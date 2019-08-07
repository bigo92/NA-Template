import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orderCode'
})
export class OrderCodePipe implements PipeTransform {

  transform(value: any, args?: any): any {    
    if (!value || value === '') {
      return '';
    }
    return value.substring(value.indexOf('-')+1);
  }

}
