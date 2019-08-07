import { Injectable } from '@angular/core';
import { RequestOptions, ResponseContentType, Http, Headers } from '@angular/http';
import { saveAs } from 'file-saver';


@Injectable({
    providedIn: 'root'
})
export class FileService {
    constructor(
        private http: Http,
    ) { }

    downloadFile(url: string, body: any, fileName: string) {
        const options = new RequestOptions({ responseType: ResponseContentType.Blob, headers: this.getHeaders() });
        this.http.post(url, body, options).subscribe(res => {
            saveFile(res.blob(), fileName);
        });
    }

    getFile(url: string, fileName: string, params: any = null) {
        let result: Promise<boolean>;
        const options = new RequestOptions({ responseType: ResponseContentType.Blob, headers: this.getHeaders(), params: params });
        result = this.http.get(url, options).toPromise()
            .then(res => {
                saveFile(res.blob(), fileName);
                return true;
            })
            .catch(err => {
                return false;
            });
        return result;
    }

    private getHeaders() {
        const headers = new Headers();
        if (localStorage.getItem('Authorization')) {
            headers.append('Authorization', localStorage.getItem('Authorization'));
        }
        if (localStorage.getItem('Language')) {
            headers.append('Language', localStorage.getItem('Language'));
        }
        return headers;
    }
}

export const saveFile = (blobContent: Blob, fileName: string) => {
    const blob = new Blob([blobContent], { type: 'application/octet-stream' });
    saveAs(blob, fileName);
};
