import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TextInputComponent } from './components/text-input/text-input.component';
import { DatePickerComponent } from './components/date-picker/date-picker.component';
import {FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { PagerComponent } from './components/pager/pager.component';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BrowserModule } from '@angular/platform-browser';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TimeagoModule } from 'ngx-timeago';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@NgModule({
  declarations: [
    TextInputComponent,
    DatePickerComponent,
    PagerComponent,
    PagingHeaderComponent,

  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    BsDatepickerModule.forRoot(),
    BsDropdownModule.forRoot(),
    CarouselModule.forRoot(),
    TabsModule.forRoot(),
    PaginationModule.forRoot(),
    NgxGalleryModule,
    FileUploadModule,
    FileUploadModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    TimeagoModule.forRoot(),
    ModalModule.forRoot(),
    ButtonsModule.forRoot(),
  ],
  exports: [
    ButtonsModule,
    TextInputComponent,
    DatePickerComponent,
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    NgxGalleryModule,
    FileUploadModule,
    ReactiveFormsModule,
    BsDropdownModule,
    FormsModule,
    CarouselModule,
    TabsModule,
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule,
    BsDatepickerModule,
    PaginationModule,
    TimeagoModule,
    ModalModule
  ]
})
export class SharedModule { }
