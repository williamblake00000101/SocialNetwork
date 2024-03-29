import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Router} from '@angular/router';
import {PresenceService} from '../core/services/presence.service';
import {BehaviorSubject, map, of, ReplaySubject} from 'rxjs';
import {User} from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presenceService: PresenceService, private router: Router) {
  }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
    this.presenceService.createHubConnection(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presenceService.stopHubConnection();
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get<boolean>(this.baseUrl + 'account/emailExists?email=' + email);
  }

  checkUsernameExists(username: string) {
    return this.http.get<boolean>(this.baseUrl + 'account/usernameExists?username=' + username)
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]))
  }
}
