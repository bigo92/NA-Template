import { Component, OnInit, ViewEncapsulation, forwardRef, AfterViewInit, ElementRef, Input, Output, EventEmitter, Injector } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, FormControl, NgControl } from '@angular/forms';
import { ExtensionService } from '../../services/extension.service';
import { Lightbox } from 'angular2-lightbox';
import { isTemplateRef } from 'ng-zorro-antd';

declare var $;
@Component({
  selector: 'input-img',
  templateUrl: './input-img.component.html',
  styleUrls: ['./input-img.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputImgComponent),
      multi: true,
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputImgComponent),
      multi: true,
    }
  ]
})
export class InputImgComponent implements OnInit, AfterViewInit, ControlValueAccessor, Validator {

  public controlValue: any[] = [];
  private controllName: string;
  @Input() class: any = '';
  @Input() placeholder: any = '';
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() size: number = 12;
  @Input() folder: string = "shared";
  @Input() sv: any;
  @Input() type: string = ".png,.jpg,.gif,.ico";
  @Input() view: boolean;
  @Output() onChange = new EventEmitter<any>();
  private error: any = null;
  public indexShow: number;
  private _album: any[] = [];
  propagateChange = (_: any) => { };
  onTouched = () => { };
  onChangeValid: () => void;
  constructor(
    private el: ElementRef,
    private ex: ExtensionService,
    private injector: Injector,
    private _lightbox: Lightbox
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    $(this.el.nativeElement).removeClass(this.class);
    const ngControl: NgControl = this.injector.get(NgControl, null);
    this.controllName = ngControl.name;
  }

  InitFile() {
    let outside = this;
    let i = 0;
    this.indexShow = 0;
    let isSetShow: boolean;
    this.controlValue.forEach(x => {
      if (x.url !== '') {
        $(outside.el.nativeElement.querySelector('#image-preview-' + i)).removeAttr('style');
        $(outside.el.nativeElement.querySelector('#image-preview-' + i)).css({ 'background-image': 'url(' + x.url + ')' });
        $(outside.el.nativeElement.querySelector('#image-label-' + i)).css({ 'opacity': '0' });

        const album = {
          id: x.id,
          src: x.url,
          caption: x.name,
          thumb: x.url
        };

        this._album.push(album);
      } else {
        if (!isSetShow) {
          isSetShow = true;
          this.indexShow = i;
        }
      }
      i++;
    });
  }

  readURL(event) {
    let outside = this;
    const input = event.currentTarget;
    let index = parseInt(event.currentTarget.getAttribute('index'));
    let fileData = this.controlValue[index];
    if (input.files && input.files.length > 0) {
      let lstType = this.type.split(',');
      for (let i = 0; i < input.files.length; i++) {
        let checkType = false;
        if (i + index >= this.controlValue.length) {
          continue;
        }
        const f = input.files[i];
        lstType.forEach(x => {
          if (f.name.toLowerCase().indexOf(x) !== -1) {
            checkType = true;
          }
        });

        if (checkType) {
          //upload file
          outside.uploadFile(index + i, f);
        } else {
          // show error
          this.error = { fomat: 'File không đúng định dạng: ' + this.type };
          this.change();
        }
      };
    } else {
      $(outside.el.nativeElement.querySelector('#image-preview-' + index)).removeAttr('style');
      $(outside.el.nativeElement.querySelector('#image-label-' + index)).css({ 'opacity': '0.8' });
      fileData.url = '';
      fileData.id = 0;
    }
    this.onTouched();
  }

  async uploadFile(index: number, file: any) {
    let fileDev = $(this.el.nativeElement.querySelector('#image-upload-' + index));

    let formUpload = new FormData();
    formUpload.append('Folder', this.folder);
    formUpload.append('Flag', $(fileDev)[0].getAttribute('flag'));
    formUpload.append('File', file, file.name);
    let rs = await this.sv.UploadFile(formUpload);
    this.ex.logDebug('upload', rs);
    if (rs.success) {
      this.controlValue[index] = {
        id: rs.data[0].id,
        url: rs.data[0].url,
        name: rs.data[0].name,
        keyName: this.ex.BoDau(rs.data[0].name),
        flag: rs.data[0].flag,
        extension: rs.data[0].extension,
        size: rs.data[0].size
      };
      this.error = null;
      this.change();
    } else {
      rs.error.forEach(x => {
        let result = JSON.parse('{"' + x.key + '":"' + x.value + '"}');
        this.error = result;
        this.change();
      });
    }
  }

  change() {
    this.propagateChange(this.controlValue); // update value to form
    this.onChange.emit(this.controlValue);
    setTimeout(() => {
      this.InitFile();
    }, 0);
  }

  async remove(index: number) {
    let result = await this.ex.confirmDialog('', 'Are you sure you want to delete this image?', '', 'OK', 'Cancel');
    if (!result) return;
    // let item = this.controlValue[index];
    // item.url = '';
    // item.id = 0;
    // $(this.el.nativeElement.querySelector('#image-preview-' + index)).removeAttr('style');
    // $(this.el.nativeElement.querySelector('#image-preview-' + index)).css({ 'background-color': '#e6e0e0' });
    // $(this.el.nativeElement.querySelector('#image-label-' + index)).css({ 'opacity': '0.8' });

    this.controlValue.splice(index, 1);
    this.controlValue.push({
      id: 0,
      date: new Date(),
      flag: this.controllName,
      type: '.jpg',
      url: ''
    });

    let i = 0;
    let isSetShow: boolean;
    this.controlValue.forEach(x => {
      if (x.url === '') {
        if (!isSetShow) {
          isSetShow = true;
          this.indexShow = i;
        }
      }
      i++;
    });
  }

  writeValue(obj: any) {
    this.controlValue = obj;
    setTimeout(() => {
      this.InitFile();
    }, 0);
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean) {
    this.disabled = isDisabled;
  }

  validate(c: FormControl): { [key: string]: any; } {
    return this.error;
  }

  registerOnValidatorChange?(fn: () => void) {
    this.onChangeValid = fn;
  }

  open(item: any) {
    // open lightbox
    if(!this.view) return;
    let index = this._album.findIndex(x=>x.id === item.id)
    this._lightbox.open(this._album, index);
  }
}
