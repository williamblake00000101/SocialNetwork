import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from '../shared/shared.module';
import {AdminPanelComponent} from './admin-panel/admin-panel.component';
import {PhotoManagementComponent} from './photo-management/photo-management.component';
import {UserManagementComponent} from './user-management/user-management.component';
import {AdminRoutingModule} from './admin-routing.module';
import {RouterModule} from '@angular/router';
import {CoreModule} from '../core/core.module';

@NgModule({
  declarations: [
    AdminPanelComponent,
    PhotoManagementComponent,
    UserManagementComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    SharedModule,
    AdminRoutingModule
  ]
})
export class AdminModule {
}
