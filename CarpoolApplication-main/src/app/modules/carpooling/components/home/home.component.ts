import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuardService } from 'src/app/services/auth-guard.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  userId: number = 0;
  userName?: string;

  constructor(
    private authService: AuthGuardService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Get the user's ID from the authentication service
    this.userId = this.authService.extractUserData();

    // Fetch user data based on the ID
    this.userService.getUserById(this.userId).subscribe((res) => {
      // If the user doesn't have a username, navigate to the profile page for setup
      if (!res.userName) {
        this.router.navigate(['/carpool/profile']);
      } else {
        // Otherwise, set the username for display
        this.userName = res.userName;
      }
    });
  }
}
