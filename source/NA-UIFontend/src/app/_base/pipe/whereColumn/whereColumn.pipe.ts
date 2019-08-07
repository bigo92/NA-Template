import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'whereColumn'
})
export class WhereColumnPipe implements PipeTransform {

  transform(value: any[], args?: any): any {
    return value.filter(x=>x.isWhere);
  }

}
