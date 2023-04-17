import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AccountModule } from './account/account.module';
import { AdminRoutingModule } from './admin/admin-routing.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';
import { HomeModule } from './home/home.module';
import { SharedModule } from './shared/shared.module';

import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';

import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { ListsComponent } from './lists/lists.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { MemberListComponent } from './members/member-list/member-list.component';


@NgModule({
  declarations: [
    AppComponent,
    ListsComponent,
    MemberCardComponent,
    MemberDetailComponent,
    MemberEditComponent,
    MessagesComponent,
    MemberMessagesComponent,
    PhotoEditorComponent,
    MemberListComponent,


  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    

    SharedModule,
    CommonModule,

    CoreModule,
    HomeModule,
  ],
  providers: [
  {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
  {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
  {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
],
  bootstrap: [AppComponent]
})
export class AppModule { }
