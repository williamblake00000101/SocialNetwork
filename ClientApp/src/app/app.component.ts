import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { User } from './shared/models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'SocialNetwork';

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    if (!token) return;
    const user: User = JSON.parse(token);
    this.accountService.setCurrentUser(user);

  }

}
