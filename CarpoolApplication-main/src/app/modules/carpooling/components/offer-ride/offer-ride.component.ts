import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-offer-ride',
  templateUrl: './offer-ride.component.html',
  styleUrls: ['./offer-ride.component.css'],
})
export class OfferRideComponent {
  constructor(private userService: UserService) {}

  // Handle submission of the offer ride form
  submitOfferRideForm($event: FormGroup<any>) {
    const offerRide = $event;

    // Add the offered ride and notify observers of the change
    this.userService.addOfferRide(offerRide).subscribe(
      (res) => {
        this.userService.notifyOfferedRideAdded();
        this.userService.openSnackBar('Offered Ride Successfully!!!!!!');
      },
      (error) => {
        this.userService.openSnackBar(error);
      }
    );
  }
}
