import { Component, OnInit } from '@angular/core';
import { MembersService } from '../members/members.service';
import { Member } from '../shared/models/member';
import { Pagination } from '../shared/models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.scss']
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 6;
  

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.members = response.result;
      }
    })
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }
}
