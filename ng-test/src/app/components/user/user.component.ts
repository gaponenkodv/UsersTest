import { Component, OnInit } from '@angular/core';
import {User} from '../../Models/user';
import {UsersService} from '../../services/users.service';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService} from '../../services/auth.service';

@Component({
  selector: 'app-user-info',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  public user = new User();
  public userId = '';
  constructor(private usersService: UsersService, private router: Router, private route: ActivatedRoute, private as: AuthService) { }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('id') ?? '';

    if (this.userId !== 'add')
    {
      this.usersService
        .getUser(this.userId)
        .subscribe(res => {
          this.user = res;
        });
    }
  }

  save(): void{
    if (this.userId !== 'add') {
      this.usersService
        .updateUser(this.userId, this.user)
        .subscribe(res => {
          this.user = res;
          alert('Saved');
        });
    }
    else
      {
      this.usersService
        .addUser(this.user)
        .subscribe(res => {
          this.user = res;
          this.router.navigate(['users', res.id]);
        });
    }
  }

  disableEdit(): boolean{
    return !this.as.isAdmin();
  }
}
