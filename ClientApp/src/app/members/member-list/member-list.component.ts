import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/shared/models/member';
import { Pagination } from 'src/app/shared/models/pagination';
import { Specialization } from 'src/app/shared/models/specialization';
import { UserParams } from 'src/app/shared/models/userParams';
import { MembersService } from '../members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss']
})
export class MemberListComponent implements OnInit{

  @ViewChild('search') searchTerm?: ElementRef;
  members: Member[] = [];
  pagination: Pagination | undefined;
  userParams: UserParams | undefined;
  specializations: Specialization[] = [];
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]

  constructor(private memberService: MembersService) {
    this.userParams = this.memberService.getUserParams();
  }
  ngOnInit(): void {
    this.getSpecializations()
    this.loadMembers()
  }

  loadMembers() {
    if (this.userParams) {
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next: response => {
          if (response.result && response.pagination) {
            this.members = response.result;
            this.pagination = response.pagination;
          }
        }
      })
    }
  }

  onPageChanged(event: any) {
    if (this.userParams && this.userParams?.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }

  onReset() {
    if (this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  onSearch() {
    const params = this.memberService.getUserParams();
    params!.search = this.searchTerm?.nativeElement.value;
    params!.pageNumber = 1;
    this.memberService.setUserParams(params!);
    this.userParams = params;
    this.loadMembers();
  }

  getSpecializations() {
    this.memberService.getSpecializationTypes().subscribe({
      next: response => this.specializations = [{id: 0, name: 'All'}, ...response],
      error: error => console.log(error)
    })
  }

  onSpecializationSelected(specializationId: number) {
    const params = this.memberService.getUserParams();
    params!.specializationId = specializationId;
    params!.pageNumber = 1;
    this.memberService.setUserParams(params!);
    this.userParams = params;
    this.loadMembers();
  }
}
