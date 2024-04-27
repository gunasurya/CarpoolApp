import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthGuardService } from 'src/app/services/auth-guard.service';
import { UserService } from 'src/app/services/user.service';
import { Booking, Ride } from '../models/Models';

@Component({
  selector: 'app-book-ride',
  templateUrl: './book-ride.component.html',
  styleUrls: ['./book-ride.component.css'],
})
export class BookRideComponent {
  rideData: any;
  matchRides!: Array<any>;
  userId: any;
  user: any;
  selectedRide: any;
  noMatchRidesMessage: string = '';

  constructor(
    private userService: UserService,
    private authServie: AuthGuardService,
    private modalService: NgbModal
  ) {}

  // Handle submission of the book ride form
  submitBookRideForm(event: any) {
    // Get the user's ID
    this.userId = this.authServie.extractUserData();
    this.rideData = event.value;

    // Create a Ride object from the form data
    const ride = new Ride(this.rideData, this.userId);

    // Fetch and display matched rides
    this.getAllMatchRides(ride);

    // Subscribe to updates in booked rides and refresh the list when changes occur
    this.userService.bookedRidesAdded$.subscribe(() => {
      this.getAllMatchRides(ride);
    });
  }

  // Get all matched rides for a given ride
  getAllMatchRides(ride: Ride) {
    this.userService.getMatchRides(ride).subscribe(
      (res) => {
        this.matchRides = res;
        if (this.matchRides.length === 0) {
          this.noMatchRidesMessage = 'No Rides to Display!!!!';
        }
      },
      (error) => {
        this.userService.openSnackBar(error);
      }
    );
  }

  // Open the modal with ride details
  openSm(content: any, ride: any) {
    this.selectedRide = ride;
    this.modalService.open(content, { size: 'sm' });
  }

  // Submit the ride booking
  submitRide() {
    // Get the user's ID
    this.userId = this.authServie.extractUserData();

    if (this.selectedRide) {
      // Create a Booking object from the selected ride and user
      const booking = new Booking(this.selectedRide, this.userId);

      // Add the booked ride and notify observers of the change
      this.userService.addBookRide(booking).subscribe(
        (res) => {
          this.userService.notifyBookedRideAdded();
          this.userService.openSnackBar('Booked Ride Successfully!!!!!!!!!');
        },
        (error) => {
          this.userService.openSnackBar(error);
        }
      );

      // Reset the selected ride and close the modal
      this.selectedRide = null;
    }
    this.modalService.dismissAll();
  }
}
