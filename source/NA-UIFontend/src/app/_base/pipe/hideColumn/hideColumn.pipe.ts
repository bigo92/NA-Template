import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'hideColumn'
})
export class HideColumnPipe implements PipeTransform {

  transform(value: any[], args?: any): any {
    return value.filter(x=>x.isShow);
  }

}
