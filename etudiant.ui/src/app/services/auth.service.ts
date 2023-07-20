import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http : HttpClient,private router: Router) { 

    const token = localStorage.getItem('authToken');
    this._isLoggedIn$.next(!!token);
  }
  public _isLoggedIn$ = new BehaviorSubject<boolean>(false);
  isLoggedIn$ = this._isLoggedIn$.asObservable();
  public registerAPI(user:User): Observable<any>{
    return this.http.post<any>(
      'https://localhost:7239/api/Auth/register', user
    );
  }
  
  public loginAPI(user:User): Observable<string>{
    return this.http.post(
      'https://localhost:7239/api/Auth/login', user,{responseType :'text'}
    );
  }

  logout(){
    localStorage.clear();
    this.router.navigate(['/']).then(() => {
      window.location.reload();
    });
  }
}
