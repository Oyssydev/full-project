import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { EditEtdComponent } from './components/dashboard/dashboard-childs/etudiant-edit-form/edit-etd.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NavbarComponent } from './components/dashboard/dashboard-childs/navbar/navbar.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ManageEtdDialogComponent } from './components/dashboard/dashboard-childs/manage-etd-dialog/manage-etd-dialog.component';
import { EtudiantsComponent } from './components/dashboard/dashboard-childs/etudiants/etudiants.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AlertModule } from 'ngx-bootstrap/alert';
import { AuthInterceptor } from './services/auth.interceptor';
import { getMultipleValuesInSingleSelectionError } from '@angular/cdk/collections';

@NgModule({
  declarations: [
    AppComponent,
    EditEtdComponent,
    DashboardComponent,
    NavbarComponent,
    ManageEtdDialogComponent,
    EtudiantsComponent
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MatDialogModule,
    BrowserModule,
    ModalModule.forRoot(),
    AlertModule.forRoot()
  ],
  providers: [
    {
      provide:HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi : true
    }
  ],
    bootstrap: [AppComponent]

})
export class AppModule { }
