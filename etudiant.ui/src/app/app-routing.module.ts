import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EtudiantsComponent } from './components/dashboard/dashboard-childs/etudiants/etudiants.component';

const routes: Routes = [
  {path:"",component:DashboardComponent},
  {path:"etudiants",component:EtudiantsComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
