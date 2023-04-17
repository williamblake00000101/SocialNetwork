import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './core/guards/admin.guard';
import { AuthGuard } from './core/guards/auth.guard';
import { PreventUnsavedChangesGuard } from './core/guards/prevent-unsaved-changes.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { MemberDetailedResolver } from './core/resolver/member-detailed.resolver';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}},

  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MemberListComponent},
      {path: 'members/:username', component: MemberDetailComponent, resolve: {member: MemberDetailedResolver}},
      {path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'lists', component: ListsComponent},


      {path: 'admin', canActivate: [AdminGuard],
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule), data: {breadcrumb: {skip: true}}}
    ]
  },



  {path: 'account', loadChildren: () => import('./account/account.module').then(m => m.AccountModule), data: {breadcrumb: {skip: true}}},
  {path: 'test-error', canActivate: [AdminGuard], component: TestErrorComponent, data: {breadcrumb: 'Test Errors'}},
  {path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'Not Found'}},
  {path: 'server-error', component: ServerErrorComponent, data: {breadcrumb: 'Server Errors'}},
  {path: '**', redirectTo: '', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
