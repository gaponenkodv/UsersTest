import { Component, OnInit } from '@angular/core';
import {User} from '../../Models/user';
import {UsersService} from '../../services/users.service';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {Role} from "../../Models/role";

@Component({
  selector: 'app-user-info',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  public user = new User();
  public userId = '';
  public availableRoles: Role[];
  constructor(private usersService: UsersService, private router: Router, private route: ActivatedRoute, private as: AuthService) { }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('id') ?? '';
    this.usersService
      .getAvailableRoles()
      .subscribe(res => {
        this.availableRoles = res;
      });

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

  isCheckedRole(role: Role): boolean{
    return this.user.roles?.find(x => x.id === role.id) != null;
  }

  roleChange(user: User , role: Role): void{
    if (user.roles.find(x => x.id === role.id ) == null)
    {
      this.usersService
        .addRole(user.id, role.id)
        .subscribe(res => {
          user.roles = res;
        });
    }else{
      this.usersService
        .deleteRole(user.id, role.id)
        .subscribe(res => {
          user.roles = res;
        });
    }
  }
}
