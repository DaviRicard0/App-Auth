import { Component } from '@angular/core';
import { Login } from '../models/Login';
import { Register } from '../models/Register';
import { JwtAuth } from '../models/JwtAuth';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Angular.FrontEnd';
  loginDto = new Login();
  registerDto = new Register();
  jwtAuthDto = new JwtAuth();

  constructor(private authService: AuthenticationService) { }

  register(registerDto: Register) {
    this.authService.register(registerDto).subscribe((jwtAuthDto) => {
      localStorage.setItem("jwtAuth", jwtAuthDto.token);
    });
  }

  login(loginDto: Login) {
    this.authService.login(loginDto).subscribe((jwtAuthDto) => {
      localStorage.setItem("jwtAuth", jwtAuthDto.token);
    });
  }

  getWeather() {
    this.authService.getWeather().subscribe(data => {
      console.log(data);
    })
  }
}
