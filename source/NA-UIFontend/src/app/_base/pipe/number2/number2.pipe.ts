import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'number2'
})
export class Number2Pipe implements PipeTransform {

  transform(value: any, args?: any): any {
    value += '';
    var x = value.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    if (args) {
      var y = args.split('.');
      if (x1.length < y[0]) {
        let part = '';
        for(let i = 0; i < y[0]-x1.length;i++ )
        {
          part+= '0';
        }
        x1 = part+ x1;
      }
    }
    return x1 + x2;
  }

}