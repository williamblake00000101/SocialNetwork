import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './core/guards/admin.guard';
import { AuthGuard } from './core/guards/auth.guard';
import { PreventUnsavedChangesGuard } from './core/guards/prevent-unsaved-changes.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home/home.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}},

  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [

      {path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard]},



      {path: 'admin', canActivate: [AdminGuard],
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule), data: {breadcrumb: {skip: true}}}
    ]
  },

  {path: 'test-error', canActivate: [AdminGuard], component: TestErrorComponent, data: {breadcrumb: 'Test Errors'}},
  {path: 'not-found', canActivate: [AdminGuard], component: NotFoundComponent, data: {breadcrumb: 'Not Found'}},
  {path: 'server-error', canActivate: [AdminGuard], component: ServerErrorComponent, data: {breadcrumb: 'Server Errors'}},

  {path: 'account', loadChildren: () => import('./account/account.module').then(m => m.AccountModule), data: {breadcrumb: {skip: true}}},
  {path: '**', redirectTo: '', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
