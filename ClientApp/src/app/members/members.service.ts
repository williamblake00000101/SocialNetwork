import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { Member } from '../shared/models/member';
import { Pagination } from '../shared/models/pagination';
import { Specialization } from '../shared/models/specialization';
import { User } from '../shared/models/user';
import { UserParams } from '../shared/models/userParams';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map();
  user: User | undefined;
  specializations: Specialization[] = [];
  pagination?: Pagination<Member[]>;
  userParams: UserParams | undefined;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.userParams = new UserParams(user);
          this.user = user;
        }
      }
    })
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = { ...this.members[index], ...member }
      })
    )
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }

  getSpecializationTypes() {
    if (this.specializations.length > 0) return of(this.specializations);

    return this.http.get<Specialization[]>(this.baseUrl + 'users/types').pipe(
      map(specializations => this.specializations = specializations)
    );
  }



}
