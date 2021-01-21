import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {API_URL} from '../app-injection-tokens';
import {Observable} from 'rxjs';
import {User} from '../Models/user';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private baseApiUrl = `${this.apiUrl}`;

  constructor(private http: HttpClient, @Inject(API_URL) private apiUrl: string) { }

  getUsers(): Observable<User[]>{
    return this.http.get<User[]>(`${this.baseApiUrl}/Users`);
  }

  getUser(id: string): Observable<User>{
    return this.http.get<User>(`${this.baseApiUrl}/Users/${id}`);
  }

  updateUser(id: string, user: User | undefined): Observable<User>  {
    return this.http.put<User>(`${this.baseApiUrl}/Users/${id}`, user);
  }

  addUser(user: User): Observable<User>  {
    return this.http.post<User>(`${this.baseApiUrl}/Users/`, user);
  }

  deleteUser(id: string): Observable<boolean>  {
    return this.http.delete<boolean>(`${this.baseApiUrl}/Users/${id}`);
  }
}
