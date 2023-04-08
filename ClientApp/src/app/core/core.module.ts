import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { ToastrModule } from 'ngx-toastr';
import { BreadcrumbModule } from 'xng-breadcrumb'



@NgModule({
  declarations: [
    NavBarComponent
  ],
  imports: [

    BreadcrumbModule,
    SharedModule,
    CommonModule,
    RouterModule,
    ToastrModule.forRoot(
      { positionClass: 'toast-bottom-right',
        preventDuplicates: true}
    )
  ],
  exports: [
    NavBarComponent
  ]
})
export class CoreModule { }
