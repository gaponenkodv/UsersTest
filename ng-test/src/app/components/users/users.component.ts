import { Component, OnInit } from '@angular/core';
import {UsersService} from '../../services/users.service';
import {User} from '../../Models/user';
import {Router} from '@angular/router';


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  columns = ['id', 'login', 'name', 'email', 'actions'];

  constructor(private usersService: UsersService, protected router: Router) { }

  ngOnInit(): void {
    this.usersService.getUsers()
      .subscribe(res => {
        this.users = res;
      });
  }

  getUser(id: string): void{
    this.router.navigate(['users', id]);
  }

  deleteUser(id: string): void{
    this.usersService.deleteUser(id)
      .subscribe(res => {
        alert('Deleted');
        this.ngOnInit();
    });
  }

  addUser(): void
  {
    this.router.navigate(['users/add']);
  }
}
