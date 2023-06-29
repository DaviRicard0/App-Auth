import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/Login';
import { Register } from '../models/Register';
import { JwtAuth } from '../models/JwtAuth';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  registerUrl = "Auth/Register";
  loginUrl = "Auth/Login";
  weatherUrl = "WeatherForecast";

  constructor(private http: HttpClient) { }

  public register(user: Register): Observable<JwtAuth> {
    return this.http.post<JwtAuth>(`${environment.apiUrl}/${this.registerUrl}`, user);
  }

  public login(user: Login): Observable<JwtAuth> {
    return this.http.post<JwtAuth>(`${environment.apiUrl}/${this.registerUrl}`, user);
  }

  public getWeather(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/${this.weatherUrl}`);
  }
}
