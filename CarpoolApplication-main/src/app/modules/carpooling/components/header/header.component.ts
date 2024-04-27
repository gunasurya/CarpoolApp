import { Component, OnInit } from '@angular/core';
import { AuthGuardService } from 'src/app/services/auth-guard.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  userId!: number;
  user: any;

  constructor(
    private authServie: AuthGuardService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    // Subscribe to user updates and fetch user data
    this.userService.userUpdated$.subscribe(() => {
      this.getUserData();
    });

    // Fetch user data initially
    this.getUserData();
  }

  // Log the user out
  logOut() {
    this.authServie.logOut();
  }

  // Get user data and update the user object
  getUserData() {
    // Get the user's ID
    this.userId = this.authServie.extractUserData();

    // Fetch user data based on the ID
    this.userService.getUserById(this.userId).subscribe((res) => {
      this.user = res;
    });
  }
}
