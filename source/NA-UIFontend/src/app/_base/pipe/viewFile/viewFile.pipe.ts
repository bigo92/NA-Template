import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'viewFile'
})
export class ViewFilePipe implements PipeTransform {

  transform(value: any[], args?: any): any {
    return value.filter(x=>x.flag === args && x.url !== '');
  }

}
