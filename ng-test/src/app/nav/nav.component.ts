import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import {AuthService} from "../services/auth.service";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  public get isLoggedIn(): boolean {
    return this.as.isAuthenticated();
  }

  constructor(private breakpointObserver: BreakpointObserver, private as: AuthService) {}

  login(email: string, password: string): void{
    this.as.login(email, password)
      .subscribe(
        res => {},
        error => {
          alert(error.message);
        });
  }

  logout(): void{
    this.as.logout();
  }
}
