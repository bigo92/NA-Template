import { AsyncValidatorFn, AbstractControl, ValidationErrors, FormGroup } from '@angular/forms';
export class GlobalAsyncValidate {
    public static lstTask: any[] = [];
    // public static uniqueValidator(tag: string, tableService: TableService, controlName: string, mess: string, minlength: number = 1, dataId = null, extraSearch = []): AsyncValidatorFn {
    //     return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
    //         if (!control.value || control.value === '' || control.value.length < minlength) {
    //             return new Observable(null);
    //         }
    //         let objectJson = `{"${controlName}": "${control.value}"}`
    //         let where = {
    //             and: [
    //                 JSON.parse(objectJson)
    //             ]
    //         };
    //         if (dataId) {
    //             where.and.push({
    //                 id: {
    //                     neq: dataId
    //                 }
    //             })
    //         }

    //         if (extraSearch.length > 0) {
    //             extraSearch.forEach(x => {
    //                 objectJson = `{"${x.key}": "${x.searchData}"}`
    //                 where.and.push(JSON.parse(objectJson))
    //             });

    //         }

    //         let taskId = this.lstTask.find(x => x.key === controlName);
    //         if (taskId === undefined) {
    //             taskId = { key: controlName, value: (new Date).toISOString() + '|' + controlName };
    //             this.lstTask.push(taskId);
    //         }

    //         let taskcurent = taskId.value;

    //         return new Promise((resolve) => {
    //             setTimeout(() => {
    //                 if (taskId.value === taskcurent) {
    //                     return tableService.getData(tag, { page: 1, size: 1, where: where }).then(
    //                         rs => {
    //                             if (rs.success && rs.data.length > 0) {
    //                                 resolve({ error: mess });
    //                             } else {
    //                                 resolve(null);
    //                             }
    //                         }
    //                     ).catch(x => {
    //                         resolve({ error: 'shared.server.timeout' });
    //                     });
    //                 }
    //             }, 300);
    //         });
    //     };
    // }
}
