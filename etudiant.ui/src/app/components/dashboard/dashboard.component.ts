import { Component } from '@angular/core';
import { User } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {

  user = new User();
  public alertType!: string;
  public alertMessage!: string;
  alertVisible = false;
  constructor(public authService:AuthService, private router: Router) {
  }

  register(user:User){
    this.authService.registerAPI(user).subscribe(
      (res: any) => {
        this.showAlert("success",res.message);
      },
      (error: any) => {
        this.showAlert("danger",error.error);
      }
    );
  }

  login(user:User){
    this.authService.loginAPI(user).subscribe((token:string)=>{
      if(token){
        this.showAlert("success","LoggedIn Successfully, Please login");
      }
      localStorage.setItem('authToken',token);
      this.router.navigate(['/etudiants']).then(() => {
        window.location.reload();
      });
     },
     (error: any) => {
       this.showAlert("danger",error.error);
      });
  }
  closeAlert(){
    this.alertVisible = false;
  }

  showAlert(alertType:string, alertMessage:string){
    this.alertMessage = alertMessage;
    this.alertType = alertType;
    this.alertVisible = true;
  }

}
