import { Component, OnInit, ViewEncapsulation, forwardRef, AfterViewInit, ElementRef, Output, Input, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, NG_VALIDATORS, Validator, FormControl } from '@angular/forms';
import { ExtensionService } from '../../services/extension.service';
import { ShareDataService } from '../../../_shared/services/share-data.service';

declare var $;
@Component({
  selector: 'input-file',
  templateUrl: './input-file.component.html',
  styleUrls: ['./input-file.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputFileComponent),
      multi: true,
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputFileComponent),
      multi: true,
    }
  ]
})
export class InputFileComponent implements OnInit, AfterViewInit, ControlValueAccessor, Validator {

  public controlValue: any[] = [];
  @Input() class: any = '';
  @Input() placeholder: any = '';
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() size: number = 12;
  @Input() folder: string = "shared";
  @Input() sv: any;
  @Input() type: string = ".doc,.docx,.pdf,.txt,.xls,.xlsx,.ppt,.pptx";
  @Output() onChange = new EventEmitter<any>();
  private error: any = null;
  propagateChange = (_: any) => { };
  onTouched = () => { };
  onChangeValid: () => void;
  constructor(
    private el: ElementRef,
    private ex: ExtensionService,
    public data_base: ShareDataService
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    $(this.el.nativeElement).removeClass(this.class);
  }

  InitFile() {
    let outside = this;
    let i = 0;
    this.controlValue.forEach(x => {
      if (x.url !== '') {
        $(outside.el.nativeElement.querySelector('#image-preview-' + i)).removeAttr('style');
        $(outside.el.nativeElement.querySelector('#image-preview-' + i)).css({ 'background-image': 'url(' + this.data_base.getImgFile(x.url) + ')' });
        $(outside.el.nativeElement.querySelector('#image-label-' + i)).css({ 'opacity': '0' });
      }
      i++;
    });
  }

  readURL(event) {
    let outside = this;
    const input = event.currentTarget;
    let index = event.currentTarget.getAttribute('index');
    let fileData = this.controlValue[index];
    if (input.files && input.files[0]) {
      let checkType = false;
      let lstType = this.type.split(',');
      lstType.forEach(x=>{
        if (input.files[0].name.toLowerCase().indexOf(x) !== -1) {
          checkType = true;
        }
      });
      if (checkType) {
        //upload file        
        outside.uploadFile(index);
      } else {
        // show error
        this.error = {fomat:'File không đúng định dạng: '+this.type};
        this.change();
      }      
    } else {
      $(outside.el.nativeElement.querySelector('#image-preview-' + index)).removeAttr('style');
      $(outside.el.nativeElement.querySelector('#image-label-' + index)).css({ 'opacity': '0.8' });
      fileData.url = '';
      fileData.id = 0;
    }
    this.onTouched();
  }

  async uploadFile(index: number) {
    let fileDev = $(this.el.nativeElement.querySelector('#image-upload-' + index));
    if ($(fileDev)[0].files.length > 0) {
      let formUpload = new FormData();
      formUpload.append('Folder', this.folder);
      formUpload.append('Flag', $(fileDev)[0].getAttribute('flag'));
      var file = $(fileDev)[0].files[0];
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
  }

  change() {
    this.propagateChange(this.controlValue); // update value to form
    this.onChange.emit(this.controlValue);
    setTimeout(() => {
      this.InitFile();
    }, 0);
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

}
