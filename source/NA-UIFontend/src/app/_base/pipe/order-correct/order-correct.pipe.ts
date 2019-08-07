import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orderCorect'
})
export class OrderCorrectPipe implements PipeTransform {

  transform(value: any, args?: any): any {            
    return value.filter(x=>x.itemId === args);
  }

}
