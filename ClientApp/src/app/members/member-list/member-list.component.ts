import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
  userParams: UserParams | undefined;
  specializations: Specialization[] = [];
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]

  constructor(private memberService: MembersService) {
    this.userParams = this.memberService.getUserParams();
  }
  ngOnInit(): void {
  }

  onPageChanged(event: any) {
    if (this.userParams && this.userParams?.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }
  
  onSearch() {
    const params = this.memberService.getUserParams();
    params!.search = this.searchTerm?.nativeElement.value;
    params!.pageNumber = 1;
    this.memberService.setUserParams(params!);
    this.userParams = params;
    this.loadMembers();
  }

  onReset() {
    if (this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  loadMembers() {

  }

}
