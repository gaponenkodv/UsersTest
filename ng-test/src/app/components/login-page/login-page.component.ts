import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {

  public get isLoggedIn(): boolean {
    return this.as.isAuthenticated();
  }

  constructor(private as: AuthService) {}

  login(email: string, password: string): void{
    this.as.login(email, password)
      .subscribe(
        res => {},
        error => {
          alert(error.message);
        });
  }

  ngOnInit(): void {
  }

}
