import { NzModalService } from 'ng-zorro-antd';

declare global{
  export interface INzModalService extends NzModalService {
    confirmAsync(title: string, content: string): Promise<boolean>;
  }
}

(NzModalService as any).confirmAsync = function (title: string, content: string): Promise<boolean> {
  debugger;
  let modelService = this as NzModalService;
  return new Promise(resolve => {
    modelService.confirm({
      nzTitle: title,
      nzContent: content,
      nzOnOk: () => {
        resolve(true);
      },
      nzOnCancel: () => {
        resolve(false);
      }
    });
  });
}
