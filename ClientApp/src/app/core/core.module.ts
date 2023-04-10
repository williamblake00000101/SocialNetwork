import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { ToastrModule } from 'ngx-toastr';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { HasRoleDirective } from './directives/has-role.directive'
import { NgxSpinnerModule } from 'ngx-spinner';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';



@NgModule({
  declarations: [
    NavBarComponent,
    SectionHeaderComponent,
    HasRoleDirective,
    RolesModalComponent
  ],
  imports: [
    BreadcrumbModule,
    SharedModule,
    CommonModule,
    RouterModule,
    NgxSpinnerModule,
    ToastrModule.forRoot(
      { positionClass: 'toast-bottom-right',
        preventDuplicates: true}

    ),
    TabsModule.forRoot(),
    ModalModule.forRoot()
  ],
  exports: [
    NavBarComponent,
    SectionHeaderComponent,
    NgxSpinnerModule,
    TabsModule,
    ModalModule
  ]
})
export class CoreModule { }
