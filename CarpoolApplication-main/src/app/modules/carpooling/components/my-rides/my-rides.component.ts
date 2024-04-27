import { Component, OnInit } from '@angular/core';
import { AuthGuardService } from 'src/app/services/auth-guard.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-my-rides',
  templateUrl: './my-rides.component.html',
  styleUrls: ['./my-rides.component.css'],
})
export class MyRidesComponent implements OnInit {
  userId!: number;
  bookedRides?: any;
  offeredRides?: any;
  NoBookedRidesMessage: string = '';
  NoOfferedRidesMessage: string = '';

  constructor(
    private userService: UserService,
    private authService: AuthGuardService
  ) {}

  ngOnInit(): void {
    // Get the user's ID from the authentication service
    this.userId = this.authService.extractUserData();

    // Subscribe to updates in booked rides and refresh the list when changes occur
    this.userService.bookedRidesAdded$.subscribe(() => {
      this.getAllBookedRides();
    });

    // Subscribe to updates in offered rides and refresh the list when changes occur
    this.userService.offeredRidesAdded$.subscribe(() => {
      this.getAllOfferedRides();
    });

    // Fetch and display booked and offered rides initially
    this.getAllBookedRides();
    this.getAllOfferedRides();
  }

  // Get all booked rides by the user
  getAllBookedRides() {
    this.userService.getBookedRides(this.userId).subscribe((res) => {
      this.bookedRides = res;
      if (this.bookedRides.length == 0) {
        this.NoBookedRidesMessage = "No Booked Rides to Display!!!!!!";
      }
    });
  }

  // Get all offered rides by the user
  getAllOfferedRides() {
    this.userService.getOfferedRides(this.userId).subscribe((res) => {
      this.offeredRides = res;
      if (this.offeredRides.length == 0) {
        this.NoOfferedRidesMessage = "No Offered Rides to Display!!!!";
      }
    });
  }
}
