import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TextInputComponent } from './components/text-input/text-input.component';
import { DatePickerComponent } from './components/date-picker/date-picker.component';
import {FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CarouselModule } from 'ngx-bootstrap/carousel';



@NgModule({
  declarations: [
    TextInputComponent,
    DatePickerComponent,

  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    BsDatepickerModule.forRoot(),
    BsDropdownModule.forRoot(),
    CarouselModule.forRoot(),
  ],
  exports: [
    TextInputComponent,
    DatePickerComponent,

    ReactiveFormsModule,
    BsDropdownModule,
    FormsModule,
    CarouselModule,

  ]
})
export class SharedModule { }
