import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map, Observable, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { Member } from '../shared/models/member';
import { Pagination } from '../shared/models/pagination';
import { PaginationResult } from '../shared/models/pagination-result';
import {getPaginatedResult, getPaginationHeaders } from '../shared/models/paginationHelper';
import { Specialization } from '../shared/models/specialization';
import { User } from '../shared/models/user';
import { UserParams } from '../shared/models/userParams';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map<string, PaginationResult<Member[]>>();
  user: User | undefined;
  specializations: Specialization[] = [];

  pagination?: PaginationResult<Member[]>;

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

  getMembers(useCache = true, userParams: UserParams) : Observable<PaginationResult<Member[]>> {
    if (!useCache) this.memberCache = new Map();

    if (this.memberCache.size > 0 && useCache) {
      if (this.memberCache.has(Object.values(userParams).join('-'))) {
        this.pagination = this.memberCache.get(Object.values(userParams).join('-'));
        if(this.pagination) return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (userParams.specializationId) params = params.append('specializationId', userParams.specializationId);
    params = params.append('orderBy', userParams.orderBy);
    params = params.append('pageIndex', userParams.pageNumber);
    params = params.append('pageSize', userParams.pageSize);
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    if (userParams.search) params = params.append('search', userParams.search);

    return this.http.get<PaginationResult<Member[]>>(this.baseUrl + 'users', {params}).pipe(
      map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response)
        this.pagination = response;
        return response;
      })
    )
  }

  getMember(userName: string) {

    /*
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => {
        return {...arr, ...elem.data.find(x => x.userName === username)}
      }, {} as Member)

    if (member) return of(member);
*/
    const member = this.members.find(x => x.userName === userName)
    if (member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + userName);
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

  getLikes(predicate: string, pageNumber: number, pageSize: number) {
    let params = getPaginationHeaders(pageNumber, pageSize);

    params = params.append('predicate', predicate);

    return getPaginatedResult<Member[]>(this.baseUrl + 'likes', params, this.http);
  }

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    if (this.user) {
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }
    return;
  }

}
